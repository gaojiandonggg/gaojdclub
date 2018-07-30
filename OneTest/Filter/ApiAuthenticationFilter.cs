using GaoJD.Club.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GaoJD.Club.OneTest.Filter
{
    public class ApiAuthenticationFilter : Attribute, IAuthorizationFilter
    {

        private IRedisClient _redisClient;
        public ApiAuthenticationFilter(IRedisClient redisClient)
        {
            this._redisClient = redisClient;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var isAnonymous = context.Filters.Where(c => c.GetType() == typeof(AllowAnonymousFilter));
            if (isAnonymous.Count() > 0)
            {
                return;
            }
            string token = "";
            StringValues list;
            context.HttpContext.Request.Headers.TryGetValue("Token", out list);
            if (list.Count <= 0)
            {
                var result = new JsonResult(new { Success = false, Code = "401", Msg = "没有认证信息" });
                context.Result = result;
            }
            else
            {
                token = list.First();
                try
                {

                    //var ticke = EncryptHelper.Decrypt(token, ConfigSetting.OpenKey);
                    string redisToken = _redisClient.Get<string>(token);
                    if (string.IsNullOrEmpty(redisToken))
                    {

                        var result = new JsonResult(new { Success = false, Code = "402", Msg = "Token过期" });
                        context.Result = result;
                    }
                    else
                    {
                        _redisClient.Set(token, token, TimeSpan.FromMinutes(30));//token   redis续期
                    }
                }
                catch (Exception ex)
                {
                    var result = new JsonResult(new { Success = false, Code = "401", Msg = "认证失败" });
                    context.Result = result;
                }

            }
        }
    }
}

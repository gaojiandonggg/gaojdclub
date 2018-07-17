using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GaoJD.Club.BusinessEntity;
using GaoJD.Club.Redis;
using GaoJD.Club.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using GaoJD.Club.OneTest;
using GaoJD.Club.OneTest.Filter;


namespace OneTest.Controllers
{
    [TypeFilter(typeof(ApiAuthenticationFilter))]
    public class BaseApiController : ControllerBase
    {

        private readonly IRedisClient _redisClient;
        public BaseApiController(IRedisClient redisClient)
        {
            _redisClient = redisClient;
        }
        public TokenUserInfo UserInfo
        {
            get
            {
                string token = HttpContext.Request.Headers["Token"].ToString();
                if (!string.IsNullOrEmpty(token))
                {
                    string redisToken = _redisClient.Get<string>(token);
                    if (!string.IsNullOrEmpty(redisToken))
                    {
                        try
                        {
                            var ticket = EncryptHelper.Decrypt(token, OpenConfiguration.Configuration["AppSettings:OpenKey"]);
                            TokenUserInfo userInfo = JsonConvert.DeserializeObject<TokenUserInfo>(ticket);
                            return userInfo;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                return null;
            }
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace GaoJD.Club.Utility
{
    public class UserHelper
    {
        /// <summary>
        /// 获取页面请求地址
        /// </summary>
        /// <returns></returns>
        public static string GetRequestUrl()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                return HttpContext.Current.Request.Path.Value;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取页面请求绝对地址
        /// </summary>
        /// <returns></returns>
        public static string GetRequestUrlAbsolutePath()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                return "";//  return HttpContext.Current.Request.Url.AbsolutePath;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取页面请求来路地址
        /// </summary>
        /// <returns></returns>
        public static string GetRequestUrlReferrerAbsoluteUri()
        {
            if (HttpContext.Current != null && HttpContext.Current.Request != null && HttpContext.Current.Request.Headers != null)
            {
                return ((Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http.HttpRequestHeaders)HttpContext.Current.Request.Headers).HeaderReferer;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取当前服务器名称
        /// </summary>
        /// <returns></returns>
        public static string GetServerName()
        {
            if (HttpContext.Current != null)
            {
                return System.Environment.MachineName;
            }
            else
            {
                return "127.0.0.1";
            }
        }

        /// <summary>
        /// 获取用户IP地址
        /// </summary>
        /// <returns>用户IP地址</returns>
        public static string GetUserIp()
        {

            try
            {
                return HttpContext.Current.Connection?.RemoteIpAddress.ToString();
            }
            catch (Exception)
            {

                throw;
            }
            //    try
            //    {
            //        string userHostAddress = "";
            //        if ((HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null
            //&& HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != String.Empty))
            //        {
            //            //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            //            userHostAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            //            //否则直接读取REMOTE_ADDR获取客户端IP地址
            //            if (string.IsNullOrEmpty(userHostAddress))
            //            {
            //                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //            }
            //            //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
            //            if (string.IsNullOrEmpty(userHostAddress))
            //            {
            //                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            //            }

            //        }
            //        else
            //        {
            //            userHostAddress = HttpContext.Current.Request.UserHostAddress;
            //        }
            //        //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
            //        if (!string.IsNullOrEmpty(userHostAddress) && IsIP(userHostAddress))
            //        {
            //            return userHostAddress;
            //        }
            //        return "127.0.0.1";
            //    }
            //    catch (Exception ex)
            //    {
            //        return "";
            //    }

        }
        /// <summary>
        /// 检查IP地址格式
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }
}

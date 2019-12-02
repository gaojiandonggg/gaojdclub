using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GaoJD.Club.Core.Utility
{
    public static class HttpUtils
    {
        /// <summary>
        /// 发送 GET 请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            return Get(url, null);
        }

        /// <summary>
        /// 发送 GET 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string Get(string url, IDictionary<string, string> parameters)
        {
            return Get(url, parameters, null);
        }

        /// <summary>
        /// 发送 GET 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string Get(string url, IDictionary<string, string> parameters, IDictionary<string, string> headers)
        {
            return Encoding.UTF8.GetString(GetAsync(url, parameters, headers).Result);
        }

        /// <summary>
        /// 发送 GET 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetAsync(string url, IDictionary<string, string> parameters, IDictionary<string, string> headers)
        {
            return await DoRequest(url, "GET", parameters, headers);
        }

        /// <summary>
        /// 发送 POST 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string Post(string url, IDictionary<string, string> parameters)
        {
            return Post(url, parameters, null);
        }

        /// <summary>
        /// 发送 POST 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string Post(string url, IDictionary<string, string> parameters, IDictionary<string, string> headers)
        {
            return Encoding.UTF8.GetString(PostAsync(url, parameters, headers).Result);
        }

        /// <summary>
        /// 发送 POST 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<byte[]> PostAsync(string url, IDictionary<string, string> parameters, IDictionary<string, string> headers)
        {
            return await DoRequest(url, "POST", parameters, headers);
        }

        /// <summary>
        /// 发送 POST JSON 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string PostJson(string url, string json)
        {
            return PostJson(url, "POSTJSON", null);
        }

        /// <summary>
        /// 发送 POST JSON 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static string PostJson(string url, string json, IDictionary<string, string> headers)
        {
            return Encoding.UTF8.GetString(PostJosnAsync(url, json, headers).Result);
        }

        /// <summary>
        /// 发送 POST JSON 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<byte[]> PostJosnAsync(string url, string json, IDictionary<string, string> headers)
        {
            Assert.NotNull(json, "json");

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("json", json);
            return await DoRequest(url, "POSTJSON", parameters, headers);
        }

        /// <summary>
        /// 发送 http 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="parameters"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public static async Task<byte[]> DoRequest(string url, string method, IDictionary<string, string> parameters, IDictionary<string, string> headers)
        {
            Assert.NotNull(url, "url");
            Assert.NotNull(method, "method");

            method = method.ToUpper();

            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(3);

                var clientHeaders = client.DefaultRequestHeaders;

                if (headers != null)
                {
                    foreach (var key in headers.Keys)
                    {
                        if (!clientHeaders.Contains(key))
                        {
                            clientHeaders.Add(key, headers[key]);
                        }
                    }
                }

                switch (method)
                {
                    case "GET":
                        {
                            if (parameters != null)
                            {
                                int index = url.LastIndexOf('?');
                                if (index == -1)
                                {
                                    url += "?";
                                }

                                string temp = string.Empty;
                                foreach (var key in parameters.Keys)
                                {
                                    temp += string.Format("&{0}={1}", key, parameters[key]);
                                }
                                if (index == -1)
                                {
                                    url += temp.TrimStart('&');
                                }
                                else
                                {
                                    url += temp;
                                }
                            }

                            return await client.GetByteArrayAsync(url);
                        }
                    case "POST":
                        {
                            HttpContent content = null;
                            if (parameters != null)
                            {
                                content = new FormUrlEncodedContent(parameters);
                            }

                            using (var response = await client.PostAsync(url, content))
                            {
                                if (!response.IsSuccessStatusCode)
                                {
                                    throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode}({response.StatusCode.ToString()}).");
                                }
                                return await response.Content.ReadAsByteArrayAsync();
                            }
                        }
                    case "POSTJSON":
                        {
                            HttpContent content = new ByteArrayContent(Encoding.UTF8.GetBytes(parameters["json"]));
                            content.Headers.Add("Content-Type", "application/json");

                            using (var response = await client.PostAsync(url, content))
                            {
                                if (!response.IsSuccessStatusCode)
                                {
                                    throw new HttpRequestException($"Response status code does not indicate success: {(int)response.StatusCode}({response.StatusCode.ToString()}).");
                                }
                                return await response.Content.ReadAsByteArrayAsync();
                            }
                        }
                    default:
                        throw new NotSupportedException("不支持的 method 类型");
                }
            }
        }
    }
}

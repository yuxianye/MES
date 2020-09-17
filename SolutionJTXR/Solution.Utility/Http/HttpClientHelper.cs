using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Solution.Utility.OAuth;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Solution.Utility.Http
{
    public class HttpClientHelper
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private static OAuth2Token oAuth2Token;
        private static string m_Uri;
        private static string m_ClientId;
        private static string m_ClientSecret;
        private static string m_UserName;
        private static string m_Password;
        static HttpClientHelper()
        {
            httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            httpClient.Timeout = new TimeSpan(0, 0, 50000);
        }

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string url, out string statusCode)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            //var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
              new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            statusCode = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        /// <summary>
        /// 获取Access Token字符串
        /// </summary>
        /// <param name="uriStr">"http://localhost:13800/"</param>
        /// <param name="clientId">ClientId字符串</param>
        /// <param name="clientSecret">密钥</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public static async Task<string> GetAccessToken(string uriStr, string clientId, string clientSecret, string username, string password)
        {
            var _httpClient = new OAuth2Client(new Uri(uriStr), clientId, clientSecret);
            m_Uri = uriStr;
            m_ClientId = clientId;
            m_ClientSecret = clientSecret;
            m_UserName = username;
            m_Password = password;
            _httpClient.BaseAddress = new Uri(uriStr);
            try
            {
                oAuth2Token = await _httpClient.RequestToken(m_UserName, m_Password);
            }
            catch (Exception ex)
            {
                string sss = ex.ToString();
            }
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oAuth2Token.AccessToken);
            return oAuth2Token.AccessToken;

        }

        public static async Task<string> ReRefreshToken()
        {
            var _httpClient = new OAuth2Client(new Uri(m_Uri), m_ClientId, m_ClientSecret);
            _httpClient.BaseAddress = new Uri(m_Uri);
            oAuth2Token = await _httpClient.RefreshToken(oAuth2Token.RefreshToken);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oAuth2Token.AccessToken);
            return oAuth2Token.AccessToken;
        }

        public static void SetAccessTokenToHttpClient(string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public static string RestfulGet(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            // Get response
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream
                StreamReader reader = new StreamReader(response.GetResponseStream());
                // Console application output
                return reader.ReadToEnd();
            }
        }

        public static T GetResponse<T>(string url)
           where T : class, new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            //var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            //
            //解决库房管理 单据号超时为空的问题。
            ///////////////
            if ((int)response.StatusCode == 401)
            {
                ReRefreshToken().Wait();
                if (oAuth2Token != null && !Equals(oAuth2Token.AccessToken, null))
                {
                    httpClient.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                    response = httpClient.GetAsync(url).Result;
                }
            }
            ///////////////

            T result = default(T);

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = JsonConvert.DeserializeObject<T>(s);
            }
            return result;
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static string PostResponse(string url, string postData, out string statusCode)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";

            HttpClient httpClient = new HttpClient();
            //httpClient..setParameter(HttpMethodParams.HTTP_CONTENT_CHARSET, "utf-8");

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            statusCode = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }

            return null;
        }

        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static T PostResponse<T>(string url, string postData)
            where T : class, new()
        {

            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //HttpClient httpClient = new HttpClient();

            T result = default(T);
#if DEBUG

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
#if DEBUG

            stopwatch.Stop();
            Utility.LogHelper.Info("连接网络用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

#endif
            Utility.LogHelper.Info("response结果：" + response.ToString());
            if ((int)response.StatusCode == 401)
            {
                ReRefreshToken().Wait();
                if (oAuth2Token != null && ! Equals( oAuth2Token.AccessToken,null ))
                {
                    httpContent = new StringContent(postData);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = httpClient.PostAsync(url, httpContent).Result;
                }
            }

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = JsonConvert.DeserializeObject<T>(s);
            }
#if DEBUG

            stopwatch.Stop();
            Utility.LogHelper.Info("JSON反序列化用时（毫秒）：" + stopwatch.ElapsedMilliseconds);

#endif
            return result;
        }

        /// <summary>
        /// 发起post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url">url</param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public async static Task<T> AsyncPostResponse<T>(string url, string postData)
            where T : class, new()
        {

            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //HttpClient httpClient = new HttpClient();

            T result = default(T);
#if DEBUG

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
#endif
            var response = await httpClient.PostAsync(url, httpContent);


            //HttpResponseMessage response = await httpClient.PostAsync(url, httpContent).Result;
#if DEBUG

            stopwatch.Stop();
            Utility.LogHelper.Info("连接网络用时（毫秒）：" + stopwatch.ElapsedMilliseconds);
            stopwatch.Restart();

#endif
            Utility.LogHelper.Info("response结果：" + response.ToString());
            if ((int)response.StatusCode == 401)
            {
                ReRefreshToken().Wait();
                if (oAuth2Token != null && !Equals(oAuth2Token.AccessToken, null))
                {
                    httpContent = new StringContent(postData);
                    httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await httpClient.PostAsync(url, httpContent);
                }
            }

            if (response.IsSuccessStatusCode)
            {
                //var responseString = await response.Content.ReadAsStringAsync();
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = JsonConvert.DeserializeObject<T>(s);
            }
#if DEBUG

            stopwatch.Stop();
            Utility.LogHelper.Info("JSON反序列化用时（毫秒）：" + stopwatch.ElapsedMilliseconds);

#endif
            return result;
        }

        /// <summary>
        /// 异步post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonParam"></param>
        /// <param name="encode"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public async static Task<string> PostJsonAsync(string url, string jsonParam, string encode, Dictionary<string, string> dic)
        {
            using (var client = new HttpClient())
            {
                foreach (KeyValuePair<string, string> item in dic)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
                // string strDecodeBody = HttpUtility.UrlEncode(jsonParam);
                HttpContent content = new StringContent(jsonParam);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = await client.PostAsync(url, content);

                var responseString = await response.Content.ReadAsStringAsync();
                return responseString;
            }
        }

        /// <summary>
        /// 反序列化Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T XmlDeserialize<T>(string xmlString)
            where T : class, new()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(xmlString))
                {
                    return (T)ser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XmlDeserialize发生异常：xmlString:" + xmlString + "异常信息：" + ex.Message);
            }

        }

        public static string PostResponse(string url, string postData, string token, string appId, string serviceURL, out string statusCode)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            httpContent.Headers.ContentType.CharSet = "utf-8";

            httpContent.Headers.Add("token", token);
            httpContent.Headers.Add("appId", appId);
            httpContent.Headers.Add("serviceURL", serviceURL);


            //HttpClient httpClient = new HttpClient();
            //httpClient..setParameter(HttpMethodParams.HTTP_CONTENT_CHARSET, "utf-8");

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            statusCode = response.StatusCode.ToString();
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }

            return null;
        }

        /// <summary>
        /// 修改API
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string KongPatchResponse(string url, string postData)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "PATCH";

            byte[] btBodys = Encoding.UTF8.GetBytes(postData);
            httpWebRequest.ContentLength = btBodys.Length;
            httpWebRequest.GetRequestStream().Write(btBodys, 0, btBodys.Length);

            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            var streamReader = new StreamReader(httpWebResponse.GetResponseStream());
            string responseContent = streamReader.ReadToEnd();

            httpWebResponse.Close();
            streamReader.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();

            return responseContent;
        }

        /// <summary>
        /// 创建API
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string KongAddResponse(string url, string postData)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "utf-8" };
            var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        /// <summary>
        /// 删除API
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool KongDeleteResponse(string url)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.DeleteAsync(url).Result;
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// 删除API
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Task<string> Delete(string url, Dictionary<string, string> dic)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            using (var client = new HttpClient())
            {
                foreach (KeyValuePair<string, string> item in dic)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }

                HttpResponseMessage response = client.DeleteAsync(url).Result;
                return Task.Run(() =>
                {
                    return response.StatusCode.ToString();
                });
            }


        }

        /// <summary>
        /// 修改或者更改API?    ?    
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <returns></returns>
        public static string KongPutResponse(string url, string postData)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "utf-8" };

            //var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.PutAsync(url, httpContent).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        /// <summary>
        /// 检索API
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string KongSerchResponse(string url)
        {
            if (url.StartsWith("https"))
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            //var httpClient = new HttpClient();
            HttpResponseMessage response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetResponse(string url)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            //HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = httpClient.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post数据</param>
        /// <returns></returns>
        public static string PostResponse(string url, string postData)
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(postData);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
            return null;
        }

        /// <summary>
        /// V3接口全部为Xml形式，故有此方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T PostXmlResponse<T>(string url, string xmlString)
            where T : class, new()
        {
            if (url.StartsWith("https"))
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;

            HttpContent httpContent = new StringContent(xmlString);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //HttpClient httpClient = new HttpClient();

            T result = default(T);

            HttpResponseMessage response = httpClient.PostAsync(url, httpContent).Result;

            if (response.IsSuccessStatusCode)
            {
                Task<string> t = response.Content.ReadAsStringAsync();
                string s = t.Result;

                result = XmlDeserialize<T>(s);
            }
            return result;
        }

    }
}
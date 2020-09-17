using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Solution.Utility.Http
{
    public class HttpHelper
    {
        private CookieContainer cc;

        public CookieContainer CC
        {
            get
            {
                return cc;
            }
            set
            {
                this.cc = value;
            }
        }

        public HttpHelper()
        {
            this.cc = new CookieContainer();
        }

        public HttpHelper(CookieContainer cc)
        {
            this.cc = cc;
        }

        ///<summary>
        /// 使用post方式访问目标网页，返回stream二进制流
        ///</summary>
        public Stream PostAndGetStream(string targetURL, string formData, string contentType, string referer, bool allowAutoRedirect)
        {
            //数据编码
            ASCIIEncoding encoding = new ASCIIEncoding();
            //UTF8Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(formData);

            //请求目标网页
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetURL);
            request.CookieContainer = cc;
            request.Method = "POST";    //使用post方式发送数据
            request.ContentType = "application/x-www-form-urlencoded";
            request.Referer = referer;
            request.AllowAutoRedirect = allowAutoRedirect;
            request.ContentLength = data.Length;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 2.0.1124)";
            //request.UnsafeAuthenticatedConnectionSharing = false;
            //模拟一个UserAgent
            Stream newStream = request.GetRequestStream();
            newStream.Write(data, 0, data.Length);
            newStream.Close();

            //获取网页响应结果
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cc.Add(response.Cookies);
            Stream stream = response.GetResponseStream();
            return stream;
        }

        ///<summary>
        /// 使用post方式访问目标网页，返回字节数组
        ///</summary>
        public byte[] PostAndGetByte(string targetURL, string formData, string contentType, string referer, bool allowAutoRedirect)
        {
            Stream stream = PostAndGetStream(targetURL, formData, contentType, referer, allowAutoRedirect);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        ///<summary>
        /// 使用post方式访问目标网页，返回图片
        ///</summary>
        public Image PostAndGetBitmap(string targetURL, string formData, string contentType, string referer, bool allowAutoRedirect)
        {
            Stream stream = PostAndGetStream(targetURL, formData, contentType, referer, allowAutoRedirect);
            Image image = Image.FromStream(stream);
            return image;
        }

        ///<summary>
        /// 使用post方式访问目标网页，返回文件
        ///</summary>
        public void PostAndGetBitmap(string targetURL, string formData, string contentType, string referer, bool allowAutoRedirect, string fileName)
        {
            byte[] bytes = PostAndGetByte(targetURL, formData, contentType, referer, allowAutoRedirect);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }


        ///<summary>
        /// 使用get方式访问目标网页，返回stream二进制流
        ///</summary>
        public Stream GetAndGetStream(string targetURL, string contentType, string referer, bool allowAutoRedirect)
        {
            //请求目标网页
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetURL);
            request.CookieContainer = cc;
            CC = cc;
            request.Method = "GET";    //使用get方式发送数据
            request.ContentType = contentType;
            request.Referer = referer;
            request.AllowAutoRedirect = allowAutoRedirect;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; SV1; .NET CLR 2.0.1124)";


            //获取网页响应结果
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cc.Add(response.Cookies);
            Stream stream = response.GetResponseStream();
            return stream;
        }

        ///<summary>
        /// 使用get方式访问目标网页，返回字节数组
        ///</summary>
        public byte[] GetAndGetByte(string targetURL, string contentType, string referer, bool allowAutoRedirect)
        {
            Stream stream = GetAndGetStream(targetURL, contentType, referer, allowAutoRedirect);
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        ///<summary>
        /// 使用get方式访问目标网页，返回图片
        ///</summary>
        public Image GetAndGetBitmap(string targetURL, string contentType, string referer, bool allowAutoRedirect)
        {
            Stream stream = GetAndGetStream(targetURL, contentType, referer, true);
            Image image = Image.FromStream(stream);
            return image;
        }

        ///<summary>
        /// 使用get方式访问目标网页，返回文件
        ///</summary>
        public void GetAndGetFile(string targetURL, string contentType, string referer, bool allowAutoRedirect, string fileName)
        {
            byte[] bytes = GetAndGetByte(targetURL, contentType, referer, allowAutoRedirect);
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        ///<summary>
        /// 使用get方式访问目标网页，返回html页面
        ///</summary>
        public string GetAndGetHtml(string targetURL, string contentType, string referer, bool allowAutoRedirect, Encoding encoding)
        {
            Stream stream = GetAndGetStream(targetURL, contentType, referer, allowAutoRedirect);
            string html = new StreamReader(stream, encoding).ReadToEnd();
            return html;
        }

        ///<summary>
        /// 使用post方式访问目标网页，返回html页面
        ///</summary>
        public string PostAndGetHtml(string targetURL, string formData, string contentType, string referer, bool allowAutoRedirect, Encoding encoding)
        {
            Stream stream = PostAndGetStream(targetURL, formData, contentType, referer, allowAutoRedirect);
            string html = new StreamReader(stream, encoding).ReadToEnd();
            return html;
        }

        public T GetAndGetEntiy<T>(string targetURL, string contentType, string referer, bool allowAutoRedirect, Encoding encoding)
            where T : class, new()
        {
            Stream stream = GetAndGetStream(targetURL, contentType, referer, allowAutoRedirect);
            string html = new StreamReader(stream, encoding).ReadToEnd();

            T result = default(T);
            result = JsonConvert.DeserializeObject<T>(html);
            return result;
        }

        ///<summary>
        /// 使用post方式访问目标网页，返回html页面
        ///</summary>
        public T PostAndGetEntity<T>(string targetURL, string formData, string contentType, string referer, bool allowAutoRedirect, Encoding encoding)
             where T : class, new()
        {
            Stream stream = PostAndGetStream(targetURL, formData, contentType, referer, allowAutoRedirect);
            string html = new StreamReader(stream, encoding).ReadToEnd();

            T result = default(T);
            result = JsonConvert.DeserializeObject<T>(html);
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace AutoPoke
{
    public class PostClass
    {
        private static CookieContainer cc = new CookieContainer();

        //POST işlemleri için
        public static string HttpRequest(string Address, string Data)
        {
            string source;

            string strBuffer = Data;

            byte[] buffer = System.Text.Encoding.ASCII.GetBytes(strBuffer);

            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(Address);
            WebReq.CookieContainer = cc;
            WebReq.Method = "POST";

            WebReq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3";

            WebReq.ContentType = "application/x-www-form-urlencoded";
            WebReq.ContentLength = buffer.Length;
            WebReq.Referer = "http://www.facebook.com/";
            WebReq.KeepAlive = true;

            WebReq.AllowAutoRedirect = true;


            Stream PostData = WebReq.GetRequestStream();
            PostData.Write(buffer, 0, buffer.Length);
            PostData.Close();

            HttpWebResponse response = (HttpWebResponse)WebReq.GetResponse();

            cc.SetCookies(WebReq.Address, "Set-Cookie");

            foreach (string cookie in response.Headers.AllKeys)
            {
                string val = response.Headers[cookie].ToString();
                response.Cookies.Add(new Cookie(cookie, val));
            }

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                source = reader.ReadToEnd();
            }

            return source;
        }

        //GET işlemleri için
        public static string HttpRequest(string Address)
        {
            string source;

            try
            {
                HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(Address);
                WebReq.CookieContainer = cc;
                WebReq.Method = "GET";

                WebReq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3";

                WebReq.ContentType = "application/x-www-form-urlencoded";
                WebReq.Referer = "http://www.facebook.com/";
                WebReq.KeepAlive = true;

                WebReq.AllowAutoRedirect = true;

                HttpWebResponse response = (HttpWebResponse)WebReq.GetResponse();

                cc.SetCookies(WebReq.Address, "Set-Cookie");

                foreach (string cookie in response.Headers.AllKeys)
                {
                    string val = response.Headers[cookie].ToString();
                    response.Cookies.Add(new Cookie(cookie, val));
                }

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    source = reader.ReadToEnd();
                }
            }
            finally
            {
                ;
            }

            return source;
        }

        //GET işlemleri için. Dönüş tipi Stream.
        public static void HttpRequest(string Address, out Stream str)
        {
            HttpWebRequest WebReq = (HttpWebRequest)WebRequest.Create(Address);
            WebReq.CookieContainer = cc;
            WebReq.Method = "GET";

            WebReq.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1.3) Gecko/20090824 Firefox/3.5.3";

            WebReq.ContentType = "application/x-www-form-urlencoded";
            WebReq.Referer = "http://www.facebook.com/";
            WebReq.KeepAlive = true;

            WebReq.AllowAutoRedirect = true;

            HttpWebResponse response = (HttpWebResponse)WebReq.GetResponse();

            cc.SetCookies(WebReq.Address, "Set-Cookie");

            foreach (string cookie in response.Headers.AllKeys)
            {
                string val = response.Headers[cookie].ToString();
                response.Cookies.Add(new Cookie(cookie, val));
            }

            str = response.GetResponseStream();
        }
    }
}

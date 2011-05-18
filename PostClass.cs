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

        //UserID uzunluğunu alan function
        public static int StrCount(string text, string pattern)
        {
            int num = 0;
            int startIndex = 0;
            while ((startIndex = text.IndexOf(pattern, startIndex)) != -1)
            {
                startIndex += pattern.Length;
                num++;
            }
            return num;
        }

        //UserID alanını text'ten ayıran function
        public static string Split(string msg, string delim, int part)
        {
            try
            {
                for (int i = 0; i < part; i++)
                {
                    msg = msg.Substring(msg.IndexOf(delim) + delim.Length);
                }
                if (msg.IndexOf(delim) == -1)
                {
                    return msg;
                }
                return msg.Substring(0, msg.IndexOf(delim));
            }
            catch
            {
                return "";
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AutoPoke
{
    public class Data
    {
        public string Name { get; set; }
    }

    public class Facebook
    {
        //UserID'ye göre kullanıcı ismini veren fonksiyon
        public static string GetNameByUserID(string UID)
        {
            string result = PostClass.HttpRequest(String.Format("https://graph.facebook.com/{0}", UID));

            JObject j = JObject.Parse(result);

            List<Data> profiles = new List<Data>();

            profiles.Add(JsonConvert.DeserializeObject<Data>(j.ToString()));

            foreach (Data item in profiles)
            {
                return item.Name;
            }

            return UID;
        }
    }
}

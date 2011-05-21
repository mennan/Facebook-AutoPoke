using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.IO;

namespace AutoPoke
{
    public class Data
    {
        public string Name { get; set; }
    }

    public class FacebookFriends
    {
        public string i { get; set; }
        public string t { get; set; }
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

        public static List<FacebookFriends> GetFriendList()
        {
            string fl = PostClass.HttpRequest("http://www.facebook.com/ajax/typeahead_friends.php?&__a=1");

            fl = fl.Replace("for (;;);{\"__ar\":1,\"payload\":", String.Empty);

            JObject j = JObject.Parse(fl);
            JArray ss = (JArray)j["friends"];

            List<FacebookFriends> profiles = new List<FacebookFriends>();

            foreach (var item in ss)
            {
                profiles.Add(JsonConvert.DeserializeObject<FacebookFriends>(item.ToString()));
            }

            return profiles;
        }

        public static Image GetUserProfilePhoto(string UID)
        {
            Stream result;
            PostClass.HttpRequest(String.Format("http://graph.facebook.com/{0}/picture", UID), out result);

            Image img = Image.FromStream(result);

            return img;
        }
    }
}

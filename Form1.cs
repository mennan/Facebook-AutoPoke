using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace AutoPoke
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Login

            string result = PostClass.HttpRequest("http://m.facebook.com/login.php", "email=mennan.kose&pass=fenerbahce1907");

            if (result.Contains("inputpassword"))
                MessageBox.Show("Kullanıcı adı veya şifre hatalı");
            else
                MessageBox.Show("Giriş yapıldı.");
            
            Regex rg = new Regex("poke=(.*?)&amp;");
            MatchCollection m = rg.Matches(result);

            List<string> UserID = new List<string>();

            foreach (Match item in m)
            {
                string r = item.Groups[0].ToString();
                string[] id = r.Split('=');
                id = id[1].Split('&');

                UserID.Add(id[0]);
            }

            foreach (string item in UserID)
            {
                PostClass.HttpRequest("http://www.facebook.com/ajax/poke.php?__a=1", String.Format("post_form_id=7a0fb076716c78523c08d48672af3f3f&uid={0}&pokeback=1&opp=&pk01=Poke&__d=1&fb_dtsg=kVdpn&lsd&post_form_id_source=AsyncRequest", item));
            }
        }
    }
}

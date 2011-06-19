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
using System.Security.Permissions;

namespace AutoPoke
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string result = null;
        private delegate void Delege(string ID);
        Action method = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.SizeChanged += new EventHandler(Form1_SizeChanged);
        }

        void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(String.Format("{0} Giriş yapılıyor...", DateTime.Now.ToShortTimeString()));

            Application.DoEvents();

            result = PostClass.HttpRequest("http://m.facebook.com/login.php", String.Format("email={0}&pass={1}", textBox1.Text, textBox2.Text));

            if (result.Contains("password"))
            {
                MessageBox.Show("Kullanıcı adı veya şifre hatalı");
                return;
            }
            else
            {
                listBox1.Items.Add(String.Format("{0} Giriş yapıldı...", DateTime.Now.ToShortTimeString()));
                timer1.Interval = Convert.ToInt32(numericUpDown1.Value) * 1000;
                timer1.Enabled = true;
                Poke();
            }
        }

        private void GetLastPokeYou()
        {
            result = PostClass.HttpRequest("http://m.facebook.com");
        }

        private void AddItemToListBox(string item)
        {
            if (listBox1.InvokeRequired)
            {
                method = delegate
                {
                    listBox1.Items.Add(item);
                };

                listBox1.Invoke(method);
            }
        }

        private void Poke()
        {
            GetLastPokeYou();

            Regex rg = new Regex("poke=(?<uid>[^<]*)&amp;gfid=(?<gfid>[^<]*)&amp;refid=(?<refid>[^<]*)");
            MatchCollection m = rg.Matches(result);

            List<string> UserID = new List<string>();
            List<string> Gfid = new List<string>();
            List<string> RefID = new List<string>();

            foreach (Match item in m)
            {
                UserID.Add(item.Groups[1].Value);
                Gfid.Add(item.Groups[2].Value);
                RefID.Add(item.Groups[3].Value);
            }

            //Poke

            //Eğer dürtülecek kişi yoksa mesaj ver.
            if (UserID.Count == 0)
            {
                listBox1.Items.Add(String.Format("{0} Dürtülecek kişi bulunamadı.", DateTime.Now.ToShortTimeString()));
            }

            for (int i = 0; i < UserID.Count; i++)
            {
                string poke_result = PostClass.HttpRequest(String.Format("http://m.facebook.com/a/notifications.php?poke={0}&gfid={1}&refid=17\"", UserID[i], Gfid[i]));
                string user_name = Facebook.GetNameByUserID(UserID[i]);
                listBox1.Items.Add(String.Format("{0} {1} dürtüldü.", DateTime.Now.ToShortTimeString(), user_name));
            }

            timer1.Enabled = false;

            List<string> users = XML.ReadXML();

            foreach (string item in users)
            {
                Delege d = new Delege(Poke);
                d.BeginInvoke(item, null, this);
            }

            timer1.Enabled = true;
        }

        //UserID bilgisine göre dürtme işlemi
        private void Poke(string UID)
        {
            string r = PostClass.HttpRequest(String.Format("http://www.facebook.com/"));
           
            Regex rg = new Regex("autocomplete=\"off\" name=\"post_form_id\" value=\"(?<gfid>[^<]*)\"");
            Match m = rg.Match(r);

            List<string> form_id = new List<string>();
            List<string> fb_dtsg = new List<string>();

            form_id.Add(m.Groups[1].Value);

            rg = new Regex("type=\"hidden\" name=\"fb_dtsg\" value=\"(?<gfid>[^<]*)\" autocomplete=\"off\"");
            m = rg.Match(r);

            fb_dtsg.Add(m.Groups[1].Value);

            string poke_result = PostClass.HttpRequest("http://www.facebook.com/ajax/poke.php?__a=1", String.Format("post_form_id={0}&uid={1}&pokeback=1&opp=&pk01=Poke&__d=1&fb_dtsg={2}&lsd&post_form_id_source=AsyncRequest", form_id[0], UID, fb_dtsg[0]));

            string user_name = Facebook.GetNameByUserID(UID);

            if (poke_result.Contains("You have poked"))
                AddItemToListBox(String.Format("{0} {1} dürtüldü.", DateTime.Now.ToShortTimeString(), user_name));
            else if (poke_result.Contains("has not received your last poke yet"))
                AddItemToListBox(String.Format("{0} {1} zaten dürtülmüş!", DateTime.Now.ToShortTimeString(), user_name));
            else
                AddItemToListBox(String.Format("{0} Hata oluştu!!!", DateTime.Now.ToShortTimeString()));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Poke();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void facebookAutoPokeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            friendList f = new friendList();
            f.ShowDialog();
        }
    }
}

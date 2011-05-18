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
        private const int WM_SYSCOMMAND = 0x0112;
        private const int SC_MINIMIZE = 0xF020;

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(String.Format("{0} Giriş yapılıyor...", DateTime.Now.ToShortTimeString()));

            Application.DoEvents();

            result = PostClass.HttpRequest("http://m.facebook.com/login.php", String.Format("email={0}&pass={1}", textBox1.Text, textBox2.Text));

            if (result.Contains("inputpassword"))
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
            result = PostClass.HttpRequest("http://m.facebook.com", "");
        }

        private void Poke()
        {
            GetLastPokeYou();

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

            //Poke

            //Eğer dürtülecek kişi yoksa mesaj ver.
            if (UserID.Count == 0)
            {
                listBox1.Items.Add(String.Format("{0} Dürtülecek kişi bulunamadı.", DateTime.Now.ToShortTimeString()));
                return;
            }

            //Dürtülecek kişi varsa dürt!!!
            foreach (string item in UserID)
            {
                //textBox3.Text = PostClass.HttpRequest("http://facebook.com/ajax/poke.php?__a=1", String.Format("post_form_id=7a0fb076716c78523c08d48672af3f3f&uid={0}&pokeback=1&opp=&pk01=Poke&__d=1&fb_dtsg=kVdpn&lsd&post_form_id_source=AsyncRequest", item));
                string poke_result = PostClass.HttpRequest(String.Format("http://m.facebook.com/a/notifications.php?poke={0}&gfid=9a33de6421&refid=0", item));

                listBox1.Items.Add(String.Format("{0} {1} dürtüldü.", DateTime.Now.ToShortTimeString(), item));
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Poke();
        }

        //Form Minimize edildiği zaman çalışacak fonksiyon
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = m.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MINIMIZE)
                    {
                        this.Hide();
                        return;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowDialog();
        }
    }
}

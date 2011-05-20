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
            result = PostClass.HttpRequest("http://m.facebook.com");
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
                return;
            }

            for (int i = 0; i < UserID.Count; i++)
            {
                string poke_result = PostClass.HttpRequest(String.Format("http://m.facebook.com/a/notifications.php?poke={0}&gfid={1}&refid={2}\"", UserID[i], Gfid[i], RefID[i]));

                listBox1.Items.Add(String.Format("{0} {1} dürtüldü.", DateTime.Now.ToShortTimeString(), Facebook.GetNameByUserID(UserID[i])));
            }
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
    }
}

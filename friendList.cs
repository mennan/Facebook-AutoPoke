using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace AutoPoke
{
    public partial class friendList : Form
    {
        public delegate void FacebookDelegate();
        Action method = null;

        public friendList()
        {
            InitializeComponent();
        }

        private void Isaretle()
        {
            List<string> users = new List<string>();

            foreach (var item in panel1.Controls)
            {
                if (item is CheckBox)
                {
                    CheckBox c = item as CheckBox;

                    if (c.Checked)
                        users.Add(c.Name.ToString());
                }
            }

            if (users.Count != 0)
                XML.WriteXML(users);
        }

        private void friendList_Load(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(ArkadasListesiniAl));
            t.Start();

            panel1.AutoScroll = true;
        }

        private void ArkadasListesiniAl()
        {
            List<FacebookFriends> f = Facebook.GetFriendList();
            List<string> xml = XML.ReadXML();
            int top = 0;

            foreach (FacebookFriends item in f)
            {
                CheckBox c = new CheckBox();
                c.Name = item.i;
                c.Top = top;
                c.Text = "";
                c.Width = 20;

                string id = xml.Find(x => x == item.i);
                if (!String.IsNullOrEmpty(id))
                    c.Checked = true;

                Image img = Facebook.GetUserProfilePhoto(item.i);

                PictureBox p = new PictureBox();
                p.Top = top;
                p.Image = img;
                p.Left = 35;

                Label lbl = new Label();
                lbl.Text = item.t;
                lbl.Top = top;
                lbl.Left = 140;

                KontrolEkle(c, p, lbl);
                top += 70;
            }
        }

        public void KontrolEkle(params Control[] ctrl)
        {
            if (panel1.InvokeRequired)
            {
                foreach (Control item in ctrl)
                {
                    method = delegate
                    {
                        panel1.Controls.Add(item);
                    };

                    panel1.Invoke(method);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Isaretle();

            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            open.Filter = "XML Dosyası|*.xml";
            DialogResult d = open.ShowDialog();

            if (d == System.Windows.Forms.DialogResult.OK)
                File.Copy(open.FileName, Application.StartupPath + @"\users.xml");

            foreach (Control item in panel1.Controls)
            {
                if (item is CheckBox)
                    (item as CheckBox).Checked = false;
            }

            Isaretle();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "XML Dosyası|*.xml";
            DialogResult d = save.ShowDialog();

            if (d == System.Windows.Forms.DialogResult.OK)
                File.Copy(Application.StartupPath + @"\users.xml", save.FileName);
        }
    }
}

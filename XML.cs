using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace AutoPoke
{
    class XML
    {
        public static void WriteXML(List<string> UID)
        {
            XmlTextWriter xw = new XmlTextWriter("users.xml", Encoding.UTF8);

            xw.WriteStartDocument();
            xw.Formatting = Formatting.Indented;

            xw.WriteStartElement("users");

            foreach (string item in UID)
            {
                xw.WriteStartElement("user");
                xw.WriteElementString("UID", item);
                xw.WriteEndElement();
            }

            xw.WriteEndElement();
            xw.WriteEndDocument();
            xw.Flush();
            xw.Close();
        }

        public static List<string> ReadXML()
        {
            List<string> UID = new List<string>();

            XDocument doc = XDocument.Load("users.xml");

            var sorgu =  from id in doc.Elements("users").Elements("user")
                         select id;

            foreach (XElement item in sorgu)
            {
                UID.Add(item.Element("UID").Value);
            }

            return UID;
        }
    }
}

using System;
using System.IO;
#if !WEB
using System.Xml;
#endif
using System.Collections.Generic;

namespace UmbrellaToolsKit.Storage
{
    public class Load
    {
#if !WEB
        public XmlDocument doc = new XmlDocument();
        XmlTextReader reader;
        string curlFile = @"Setting.Umbrella";
#endif

        public Load()
        {
#if !WEB
            if (!File.Exists(this.curlFile))
            {
                File.AppendAllText(this.curlFile, "umbrella");
                doc.AppendChild(doc.CreateElement("Umbrella"));
                this.Save();
            } else
            {
                doc.Load(this.curlFile);
            }
#endif
        }

        public void Save()
        {
#if !WEB
            doc.Save(this.curlFile);
#endif
        }

#if !WEB
        private void CreateList(string Node, string Type)
        {
            if (doc.GetElementsByTagName(Node).Count < 1)
            {
                XmlElement elem = doc.CreateElement(Node);
                elem.SetAttribute("type", Type);
                doc.GetElementsByTagName("Umbrella")[0].AppendChild(elem);
            }

            XmlNodeList ElementNode = doc.GetElementsByTagName(Node);
            ElementNode[0].RemoveAll();
            XmlAttribute attr = doc.CreateAttribute("type");
            attr.Value = Type;
            ElementNode[0].Attributes.SetNamedItem(attr);
        }

        public List<string> getItemsString(string Node)
        {
            List<string> ListReturn = new List<string>();

            XmlNodeList ElementNode = doc.GetElementsByTagName(Node);
            if (ElementNode.Count > 0 && ElementNode[0].Attributes[0].Value == "string" && ElementNode[0].HasChildNodes)
                foreach (XmlNode xxNode in ElementNode[0].ChildNodes)
                    ListReturn.Add(xxNode.InnerText);

            return ListReturn;
        }

        public List<float> getItemsFloat(string Node)
        {
            List<float> ListReturn = new List<float>();

            XmlNodeList ElementNode = doc.GetElementsByTagName(Node);
            if (ElementNode.Count > 0 && ElementNode[0].Attributes[0].Value == "float" && ElementNode[0].HasChildNodes)
                foreach (XmlNode xxNode in ElementNode[0].ChildNodes)
                    ListReturn.Add(float.Parse(xxNode.InnerText, System.Globalization.CultureInfo.InvariantCulture));

            return ListReturn;
        }

        public List<bool> getItemsBool(string Node)
        {
            List<bool> ListReturn = new List<bool>();

            XmlNodeList ElementNode = doc.GetElementsByTagName(Node);
            if (ElementNode.Count > 0 && ElementNode[0].Attributes[0].Value == "bool" && ElementNode[0].HasChildNodes)
                foreach (XmlNode xxNode in ElementNode[0].ChildNodes)
                    ListReturn.Add(bool.Parse(xxNode.InnerText));

            return ListReturn;
        }


        public void AddItemString(string Node, List<string> ContentList)
        {
            this.CreateList(Node, "string");

            XmlNodeList ElementNode = doc.GetElementsByTagName(Node);

            foreach (string content in ContentList)
            {
                XmlElement NewElement = doc.CreateElement("item");
                NewElement.InnerText = content;
                ElementNode[0].AppendChild(NewElement);
            }
        }

        public void AddItemFloat(string Node, List<float> ContentList)
        {
            this.CreateList(Node, "float");

            XmlNodeList ElementNode = doc.GetElementsByTagName(Node);

            foreach (float content in ContentList)
            {
                XmlElement NewElement = doc.CreateElement("item");
                NewElement.InnerText = content.ToString();
                ElementNode[0].AppendChild(NewElement);
            }
        }

        public void AddItemBool(string Node, List<bool> ContentList)
        {
            this.CreateList(Node, "bool");

            XmlNodeList ElementNode = doc.GetElementsByTagName(Node);

            foreach (bool content in ContentList)
            {
                XmlElement NewElement = doc.CreateElement("item");
                NewElement.InnerText = content.ToString();
                ElementNode[0].AppendChild(NewElement);
            }
        }
#endif

    }
}

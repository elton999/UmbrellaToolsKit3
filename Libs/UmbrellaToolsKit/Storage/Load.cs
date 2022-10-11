using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace UmbrellaToolsKit.Storage
{
    public class Load
    {
        private XmlTextReader reader;
        private string curlFile = @"Setting.Umbrella";

        public XmlDocument doc = new XmlDocument();

        public Load()
        {
            if (!File.Exists(this.curlFile))
            {
                File.AppendAllText(this.curlFile, "umbrella");
                doc.AppendChild(doc.CreateElement("Umbrella"));
                this.Save();
                return;
            }
            doc.Load(this.curlFile);
        }

        public void Save() => doc.Save(this.curlFile);

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

        public void AddItemString(string Node, List<string> ContentList) => AddAItem(Node, "string", ContentList);

        public void AddItemFloat(string Node, List<float> ContentList) => AddAItem(Node, "float", ContentList);

        public void AddItemBool(string Node, List<bool> ContentList) => AddAItem(Node, "bool", ContentList);

        public void AddAItem(string Node, string typeObject, IEnumerable ContentList)
        {
            CreateList(Node, typeObject);

            XmlNodeList ElementNode = doc.GetElementsByTagName(Node);

            foreach (object content in ContentList)
            {
                XmlElement NewElement = doc.CreateElement("item");
                NewElement.InnerText = content.ToString();
                ElementNode[0].AppendChild(NewElement);
            }
        }
    }
}

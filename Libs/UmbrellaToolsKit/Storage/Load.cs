﻿using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System;

namespace UmbrellaToolsKit.Storage
{
    public class Load : IDisposable
    {
        private XmlTextReader reader;
        private string _curlFile = @"Setting.Umbrella";

        public XmlDocument doc;

        public event Action OnSave;

        public Load() => LoadFile();

        public Load(string curlFile)
        {
            _curlFile = curlFile;
            LoadFile();
        }

        public void SetFilename(string filename) => _curlFile = filename;

        private void LoadFile()
        {
            doc = new XmlDocument();
            if (!File.Exists(_curlFile))
            {
                File.AppendAllText(_curlFile, "umbrella");
                doc.AppendChild(doc.CreateElement("Umbrella"));
                Save();
                return;
            }
            doc.Load(_curlFile);
        }

        public void Save()
        {
            OnSave?.Invoke();
            doc.Save(_curlFile);
            LoadFile();
        }

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

        public void DeleteNode(string node)
        {
            XmlNodeList elementNode = doc.GetElementsByTagName(node);
            if (elementNode.Count == 0) return;
            elementNode[0].ParentNode.RemoveChild(elementNode[0]);
            Save();
        }

        public void SetString(string node, string value) => AddItemString(node, new List<string> { value == null ? "" : value });

        public void AddItemString(string Node, List<string> ContentList) => AddAItem(Node, "string", ContentList);

        public void AddItemString(string Node, string value)
        {
            var contentList = getItemsString(Node);
            contentList.Add(value);
            AddAItem(Node, "string", contentList);
        }

        public void SetFloat(string node, float value) => AddItemFloat(node, new List<float> { value });

        public void AddItemFloat(string Node, List<float> ContentList) => AddAItem(Node, "float", ContentList);

        public void AddItemFloat(string Node, float value)
        {
            var contentList = getItemsFloat(Node);
            contentList.Add(value);
            AddAItem(Node, "float", contentList);
        }

        public void SetBool(string node, bool value) => AddItemBool(node, new List<bool> { value });

        public void AddItemBool(string Node, List<bool> ContentList) => AddAItem(Node, "bool", ContentList);

        public void AddItemBool(string Node, bool value)
        {
            var contentList = getItemsBool(Node);
            contentList.Add(value);
            AddAItem(Node, "bool", contentList);
        }

        public void AddAItem(string Node, string typeObject, IEnumerable ContentList)
        {
            CreateList(Node, typeObject);

            XmlNodeList ElementNode = doc.GetElementsByTagName(Node);

            foreach (object content in ContentList)
            {
                if (content is not null)
                {
                    XmlElement NewElement = doc.CreateElement("item");
                    NewElement.InnerText = content.ToString();
                    ElementNode[0].AppendChild(NewElement);
                }
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(doc);
        }
    }
}

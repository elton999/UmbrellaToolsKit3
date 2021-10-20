using System;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

[Serializable()]
[XmlRoot("localization")]
public class Location
{
    [XmlElement("Tags")]
    public Item tags { get; set; }

    [XmlElement("Languages")]
    public Item languages { get; set; }

    [XmlElement("English")]
    public Item translations_en { get; set; }

    [XmlElement("Portugues")]
    public Item translations_pt { get; set; }
}

public class Item
{
    [XmlElement("item")]
    public string[] item { get; set; }
}


using System;
using System.Xml.Serialization;

[Serializable()]
[XmlRoot("map")]
public class ConfigMap
{
    [XmlAttribute("orientation")]
    public string orientation { get; set; }
    [XmlAttribute("renderorder")]
    public string renderorder { get; set; }
    [XmlAttribute("width")]
    public int width { get; set; }
    [XmlAttribute("height")]
    public int height { get; set; }
    [XmlAttribute("tilewidth")]
    public int tilewidth { get; set; }
    [XmlAttribute("tileheight")]
    public int tileheight { get; set; }

    [XmlElement("tileset")]
    public ConfigMapTileSet[] tileset { get; set; }
    [XmlElement("group")]
    public ConfigMapGroup[] group { get; set; }
}

public class ConfigMapTileSet
{
    [XmlAttribute("firstgid")]
    public int firstgid { get; set; }
    [XmlAttribute("source")]
    public string source { get; set; }
}

public class ConfigMapLayer
{
    [XmlAttribute("id")]
    public int id { get; set; }
    [XmlAttribute("name")]
    public string name { get; set; }
    [XmlAttribute("width")]
    public int width { get; set; }
    [XmlAttribute("height")]
    public int height { get; set; }
    [XmlElement("data")]
    public string data { get; set; }
}

public class ConifgObjects
{
    [XmlAttribute("id")]
    public int id { get; set; }
    [XmlAttribute("name")]
    public string name { get; set; }
    [XmlAttribute("type")]
    public string type { get; set; }
    [XmlAttribute("x")]
    public float x { get; set; }
    [XmlAttribute("y")]
    public float y { get; set; }
    [XmlAttribute("width")]
    public float width { get; set; }
    [XmlAttribute("height")]
    public float height { get; set; }
}

public class ConfigGroupsObjects
{
    [XmlAttribute("id")]
    public int id { get; set; }
    [XmlAttribute("name")]
    public string name { get; set; }
    [XmlElement("object")]
    public ConifgObjects[] objects { get; set; }
}

public class ConfigMapGroup
{
    [XmlAttribute("id")]
    public int id { get; set; }
    [XmlAttribute("name")]
    public string name { get; set; }
    [XmlElement("objectgroup")]
    public ConfigGroupsObjects[] GroupsObjects { get; set; }
    [XmlElement("layer")]
    public ConfigMapLayer[] layer { get; set; }
}



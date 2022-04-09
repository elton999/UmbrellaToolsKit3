using System;
using System.Xml.Serialization;


[Serializable()]
[XmlRoot("tileset")]
public class ConfigTileSet
{
    [XmlAttribute("name")]
    public string name { get; set; }
    [XmlAttribute("tilewidth")]
    public int tilewidth { get; set; }
    [XmlAttribute("tileheight")]
    public int tileheight { get; set; }
    [XmlAttribute("tilecount")]
    public int tilecount { get; set; }
    [XmlAttribute("columns")]
    public int columns { get; set; }

    [XmlElement("image")]
    public ConfigTileSetImagem image { get; set; }

    [XmlElement("tile")]
    public ConfigTileSetTile[] tiles { get; set; }
}


public class ConfigTileSetTile
{
    [XmlAttribute("id")]
    public int id { get; set; }

    [XmlArray("objectgroup")]
    [XmlArrayItem("object")]
    public ConfigTileSetObejctGroup[] objectgroup { get; set; }
}

public class ConfigTileSetObejctGroup
{
    [XmlAttribute("id")]
    public int id { get; set; }
    [XmlAttribute("name")]
    public string name { get; set; }
    [XmlAttribute("type")]
    public string type { get; set; }
    [XmlAttribute("x")]
    public int x { get; set; }
    [XmlAttribute("y")]
    public int y { get; set; }
    [XmlAttribute("width")]
    public int width { get; set; }
    [XmlAttribute("height")]
    public int height { get; set; }
}

public class ConfigTileSetImagem
{
    [XmlAttribute("source")]
    public string source { get; set; }
    [XmlAttribute("width")]
    public int width { get; set; }
    [XmlAttribute("height")]
    public int height { get; set; }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

public class OgmoModel
{
    public String name;
    public String ogmoVersion;
    public List<String> levelPaths;
    public String backgroundColor;
    public ogmoVector2 layerGridDefaultSize;
    public List<OgmoModelLayers> layers;
}

public class OgmoModelLayers
{
    public String exportID;
    public String definition;
    public String name;
    public ogmoVector2 gridSize;
    public int exportMode;
    public int arrayMode;
    public String defaultTileset;
}

public class OgmoModelEntity
{
    public String exportID;
    public String name;
    public Point size;
    public List<String> tags;
}

public class ogmoVector2
{
    public float x;
    public float y;
}

public class OgmoLevel
{
    public int width;
    public int height;
    public int offsetX;
    public int offsetY;
    public List<OgmoLevelLayer> layers;
}

public class OgmoLevelLayer
{
    public String name;
    public String _eid;
    public int offsetX;
    public int offsetY;
    public int gridCellWidth;
    public int gridCellHeight;
    public int gridCellsX;
    public int gridCellsY;
    public List<List<int>> data2D;
    public List<List<string>> grid2D;
    public List<List<List<int>>> dataCoords2D;
    public List<OgmoLevelLayerEntities> entities;
}

public class OgmoLevelLayerEntities
{
    public String name;
    public String _eid;
    public int x;
    public int y;
    public int originX;
    public int originY;
    public int width;
    public int height;
    public Dictionary<string, string> values { get; set; }
    public List<ogmoVector2> nodes;
}

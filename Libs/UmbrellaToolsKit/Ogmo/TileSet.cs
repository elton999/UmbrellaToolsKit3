using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Ogmo
{
    public class TileSet
    {
        public String name;
        public String ogmoVersion;
        public List<String> levelPaths;
        public String backgroundColor;
        public Vector2 layerGridDefaultSize;
        public List<TileSetLayer> layers;
    }

    public class TileSetLayer
    {
        public String exportID;
        public String definition;
        public String name;
        public Vector2 gridSize;
        public int exportMode;
        public int arrayMode;
        public String defaultTileset;
    }
}

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Ogmo
{
    public class TileMapEntity
    {
        public String name;
        public String _eid;
        public int x;
        public int y;
        public int originX;
        public int originY;
        public int width;
        public int height;
        public List<Vector2> nodes;
        public Dictionary<string, string> values;
    }
}

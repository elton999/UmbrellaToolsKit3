using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace UmbrellaToolsKit.Ogmo
{
    public class TileMap
    {
        public int width;
        public int height;
        public int offsetX;
        public int offsetY;
        public List<TileMapLayers> layers;
        public ContentManager Content;
    }
}

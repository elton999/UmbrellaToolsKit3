using System;
using System.Collections.Generic;

namespace UmbrellaToolsKit.Ogmo
{
    public class TileMapLayers
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
        public List<TileMapEntity> entities;
    }
}

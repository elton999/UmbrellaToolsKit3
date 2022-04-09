using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace UmbrellaToolsKit.Ogmo
{
    public class TileSetReader : ContentTypeReader<TileSet>
    {
        protected override TileSet Read(ContentReader input, TileSet existingInstance)
        {
            TileSet tileSet = new TileSet();

            tileSet.name = input.ReadString();
            tileSet.ogmoVersion = input.ReadString();
            tileSet.backgroundColor = input.ReadString();
            int layerGridDefaultSizeX = input.ReadInt32();
            int layerGridDefaultSizeY = input.ReadInt32();
            tileSet.layerGridDefaultSize = new Vector2(layerGridDefaultSizeX, layerGridDefaultSizeY);

            /// layers
            tileSet.layers = new List<TileSetLayer>();

            int layersLenght = input.ReadInt32();
            for (var i = 0; i < layersLenght; i++)
            {
                TileSetLayer layer = new TileSetLayer();
                layer.exportID = input.ReadString();
                layer.definition = input.ReadString();
                layer.name = input.ReadString();
                layer.gridSize = new Vector2();
                layer.gridSize.X = input.ReadInt32();
                layer.gridSize.Y = input.ReadInt32();
                layer.exportMode = input.ReadInt32();
                layer.arrayMode = input.ReadInt32();
                tileSet.layers.Add(layer);
            }

            return tileSet;
        }
    }
}

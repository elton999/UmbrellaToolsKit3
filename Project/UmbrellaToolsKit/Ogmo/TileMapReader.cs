using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace UmbrellaToolsKit.Ogmo
{
    public class TileMapReader : ContentTypeReader<TileMap>
    {
        protected override TileMap Read(ContentReader input, TileMap existingInstance)
        {
            int width = input.ReadInt32();
            int height = input.ReadInt32();
            int offsetX = input.ReadInt32();
            int offsetY = input.ReadInt32();

            // layers
            List<TileMapLayers> tileMapLayers = new List<TileMapLayers>();
            int _layersLenght = input.ReadInt32();
            for(var i = 0; i < _layersLenght; i++)
            {
                TileMapLayers TileMapLayer = new TileMapLayers();

                TileMapLayer.name = input.ReadString();
                TileMapLayer._eid = input.ReadString();
                TileMapLayer.offsetX = input.ReadInt32();
                TileMapLayer.offsetY = input.ReadInt32();
                TileMapLayer.gridCellWidth = input.ReadInt32();
                TileMapLayer.gridCellHeight = input.ReadInt32();
                TileMapLayer.gridCellsX = input.ReadInt32();
                TileMapLayer.gridCellsY = input.ReadInt32();

                // data2D
                TileMapLayer.data2D = new List<List<int>>();
                int _data2DLenght_x = input.ReadInt32();
                for (var x = 0; x < _data2DLenght_x; x++)
                {
                    TileMapLayer.data2D.Add(new List<int>());
                    int _data2DLenght_y = input.ReadInt32();

                    for(var y = 0; y < _data2DLenght_y; y++)
                    {
                        TileMapLayer.data2D[x].Add(input.ReadInt32());
                    }
                }

                // grid2D
                TileMapLayer.grid2D = new List<List<string>>();
                int _grid2DLenght_x = input.ReadInt32();
                for (var x = 0; x < _grid2DLenght_x; x++)
                {
                    TileMapLayer.grid2D.Add(new List<string>());
                    int _grid2DLenght_y = input.ReadInt32();

                    for (var y = 0; y < _grid2DLenght_y; y++)
                        TileMapLayer.grid2D[x].Add(input.ReadString());
                }

                // dataCoords2D
                TileMapLayer.dataCoords2D = new List<List<List<int>>>();
                int _dataCoords2DLenght_x = input.ReadInt32();
                for (var x = 0; x < _dataCoords2DLenght_x; x++)
                {
                    TileMapLayer.dataCoords2D.Add(new List<List<int>>());
                    int _dataCoords2DLenght_y = input.ReadInt32();

                    for (var y = 0; y < _dataCoords2DLenght_y; y++)
                    {
                        int _dataCoords2DLenght_yy = input.ReadInt32();
                        TileMapLayer.dataCoords2D[x].Add(new List<int>());

                        for (var yy = 0; yy < _dataCoords2DLenght_yy; yy++)
                            TileMapLayer.dataCoords2D[x][y].Add(input.ReadInt32());
                    }
                }

                // entities
                List<TileMapEntity> entities = new List<TileMapEntity>();
                int _entitiesLenght = input.ReadInt32();

                for (var e = 0; e < _entitiesLenght; e++)
                {
                    entities.Add(new TileMapEntity());

                    entities[e].name = input.ReadString();
                    entities[e]._eid = input.ReadString();
                    entities[e].x = input.ReadInt32();
                    entities[e].y = input.ReadInt32();
                    entities[e].originX = input.ReadInt32();
                    entities[e].originY = input.ReadInt32();
                    entities[e].width = input.ReadInt32();
                    entities[e].height = input.ReadInt32();

                    entities[e].values = new Dictionary<string, string>();
                    int _valuesLenght = input.ReadInt32();
                    for (var n = 0; n < _valuesLenght; n++)
                    {
                        string _value = input.ReadString();
                        entities[e].values.Add(_value.Split(":".ToCharArray())[0], _value.Split(":".ToCharArray())[1]);
                    }

                    entities[e].nodes = new List<Vector2>();
                    int _nodesLenght = input.ReadInt32();
                    for (var n = 0; n < _nodesLenght; n++)
                    {
                        float xx = input.ReadInt32();
                        float yy = input.ReadInt32();
                        entities[e].nodes.Add(new Vector2(TileMapLayer.offsetX + xx, TileMapLayer.offsetY + yy));

                    }
                }
                TileMapLayer.entities = entities;
                tileMapLayers.Add(TileMapLayer);
            }

            TileMap tileMap = new TileMap();
            tileMap.width = width;
            tileMap.height = height;
            tileMap.offsetX = offsetX;
            tileMap.offsetY = offsetY;
            tileMap.layers = tileMapLayers;

            return tileMap;
        }
    }
}

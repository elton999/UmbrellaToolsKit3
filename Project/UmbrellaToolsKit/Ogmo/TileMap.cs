using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit.Collision;
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


        private Scene _Scene;
        private Ogmo.TileMap _TileMap;

        public void Create(Scene scene, Ogmo.TileMap tileMap, Texture2D tilemapSprite)
        {
            this._Scene = scene;
            this._TileMap = tileMap;

            this._Scene.ScreemOffset = new Point(this._TileMap.offsetX, this._TileMap.offsetY);
            this._Scene.LevelSize = new Vector2(this._TileMap.width, this._TileMap.height);

            foreach (TileMapLayers layer in this._TileMap.layers)
            {
                if (layer.grid2D.Count() > 0)
                {
                    this._Scene.Grid = new Grid();
                    this._Scene.Grid.GridCollides = layer.grid2D;
                    this._Scene.Grid.Scene = this._Scene;
                    this._Scene.Grid.Origin = new Vector2(this._TileMap.offsetX, this._TileMap.offsetY);

                }
                else if (layer.dataCoords2D.Count() > 0)
                {
                    Sprite.Layer _layerTiles = new Sprite.Layer();
                    _layerTiles.Sprite = tilemapSprite;
                    _layerTiles.tiles = layer.dataCoords2D;
                    _layerTiles.Scene = this._Scene;
                    _layerTiles.Origin = new Vector2(this._TileMap.offsetX, this._TileMap.offsetY);

                    this._Scene.Backgrounds.Add(_layerTiles);
                }
                else if (layer.entities.Count() > 0)
                {
                    System.Console.WriteLine($"Loading Entities: {layer.name} ");
                    foreach (TileMapEntity entity in layer.entities)
                    {
                        System.Console.Write(".");
                        if (AssetManagement.Instance != null)
                        {
                            AssetManagement.Instance.addEntityOnScene(
                                entity.name,
                                new Vector2(entity.x + this._Scene.ScreemOffset.X, entity.y + this._Scene.ScreemOffset.Y),
                                new Point(entity.width, entity.height),
                                entity.values,
                                entity.nodes,
                                this._Scene
                            );
                        }
                    }
                }
            }


        }
    }


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

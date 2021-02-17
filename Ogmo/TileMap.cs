using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolKit.Collision;
using Microsoft.Xna.Framework.Content;


namespace UmbrellaToolKit.Ogmo
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
        private AssetManagement _Assets;
        private Ogmo.TileSet _TileSet;
        private Ogmo.TileMap _TileMap;

        public void Create(Scene scene, AssetManagement assets, TileSet tileSet, Ogmo.TileMap tileMap, Texture2D tilemapSprite)
        {
            this._Scene = scene;
            this._Assets = assets;
            this._TileSet = tileSet;
            this._TileMap = tileMap;

            this._Scene.ScreemOffset = new Point(this._TileMap.offsetX, this._TileMap.offsetY);

            foreach (TileMapLayers layer in this._TileMap.layers)
            {
                if (layer.grid2D.Count() > 0){
                    this._Scene.Grid.GridCollides = layer.grid2D;
                    this._Scene.Grid.Scene = this._Scene;
                    this._Scene.Grid.Sprite = this._Scene.Content.Load<Texture2D>("Engine/tiles");

                } else if(layer.dataCoords2D.Count() > 0){
                    Sprite.Layer _layerTiles = new Sprite.Layer();
                    _layerTiles.Sprite = tilemapSprite;
                    _layerTiles.tiles = layer.dataCoords2D;
                    _layerTiles.Scene = this._Scene;

                    this._Scene.Backgrounds.Add(_layerTiles);
                }else if (layer.entities.Count() > 0){
                    foreach (TileMapEntity entity in layer.entities)
                    {
                        this._Assets.addEntityOnScene(
                            entity.name,
                            new Vector2(entity.x, entity.y),
                            new Point(entity.width, entity.height)
                        );
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

    }
}

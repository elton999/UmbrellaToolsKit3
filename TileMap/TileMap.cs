using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Game1;

namespace UmbrellaToolKit.Tiled
{
    public class TileMap
    {
        public int Layer;
        private ContentManager Content;

        public TileMap (ContentManager Content, string File, AssetManagement AssetManagement)
        {
            this.Content = Content;
            this.File = File;
            this.AssetManagement = AssetManagement;
            this.LoadMap();
        }

        public string File;
        public TileSet TileSet;
        public AssetManagement AssetManagement;
        private ConfigMap Config;

        public Point GridSize;

        public void LoadMap()
        {
            var serializer = new XmlSerializer(typeof(ConfigMap));
            this.Config = new ConfigMap();

            this.Config = Content.Load<ConfigMap>(this.File);
            this.GridSize = new Point(this.Config.width, this.Config.height);
            this.TileSet = new TileSet(Content, this.Config.tileset[0].source.Replace("../", "").Replace(".tsx", ""));
            this.SetAllLayers();
        }

        public TileMapLayer[] TileMapLayers;
        private void SetAllLayers()
        {
            this.AssetManagement.ClearAll();
            this.TileMapLayers = new TileMapLayer[this.Config.group.Length];
            for (int g = 0; g < this.Config.group.Length; g++)
            {
                if (this.Config.group[g].layer != null)
                {
                    for (int l = 0; l < this.Config.group[g].layer.Length; l++)
                    {
                        this.SetTiles(g, l, g);
                    }
                } 
                if(this.Config.group[g].GroupsObjects != null)
                {
                    for (int l = 0; l < this.Config.group[g].GroupsObjects.Length; l++)
                    {
                        this.SetObjects(g, l, g);
                    }
                }
            }
        }

        private void SetTiles(int g, int l, int i)
        {
            int LenthTiles = this.Config.group[g].layer[l].data.Split(',').Length;
            string[] tilesPositions = this.Config.group[g].layer[l].data.Replace(" ", "").Split(',');

            int x = 0;
            int y = 0;

            this.TileMapLayers[i] = new TileMapLayer();
            this.TileMapLayers[i].Group = this.Config.group[g].name;
            this.TileMapLayers[i].TilesPosition = new Tile[this.Config.width, this.Config.height];
            this.TileMapLayers[i].TileSet = this.TileSet;
            this.TileMapLayers[i].GridSize = this.GridSize;

            foreach (var tilePosition in tilesPositions)
            {
                if (Int32.Parse(tilePosition) > 0) this.TileMapLayers[i].TilesPosition[x, y] = this.TileSet.GetTile(Int32.Parse(tilePosition));
                if (x == this.Config.group[g].layer[l].width - 1)
                {
                    y++;
                    x = 0;
                }
                else x++;
            }

           
        }

        private void SetObjects(int g, int l, int i)
        {
            if (this.AssetManagement != null)
            {
                if (this.Config.group[g].GroupsObjects != null && this.Config.group[g].GroupsObjects.Length > 0)
                {
                    for (int yy = 0; yy < this.Config.group[g].GroupsObjects[l].objects.Length; yy++)
                    {
                        AssetObject assetObject = new AssetObject {
                            Name = this.Config.group[g].GroupsObjects[l].objects[yy].name,
                            Layer = this.Config.group[g].name,
                            Position = new Vector2(this.Config.group[g].GroupsObjects[l].objects[yy].x, this.Config.group[g].GroupsObjects[l].objects[yy].y)
                        };
                        this.AssetManagement.LevelAssetsList.Add(assetObject);
                    }
                }
            }
        }

    }

    public class TileMapLayer : GameObject
    {
        public string Group;
        public bool Player;
        public Tile[,] TilesPosition;
        public Point GridSize { set; get; }
        public TileSet TileSet;

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < this.GridSize.X; x++)
            {
                for (int y = 0; y < this.GridSize.Y; y++)
                {
                    if (this.TilesPosition[x, y] != null)
                    {
                        Vector2 positionTile = new Vector2(x * this.TileSet.TileSize.X * this.Scale, y * this.TileSet.TileSize.Y * this.Scale);
                        spriteBatch.Draw(this.TileSet.Sprite, positionTile, this.TilesPosition[x, y].TilePosition, this.SpriteColor, this.Rotation, Vector2.Zero, this.Scale, this.spriteEffect, 0);
                    }
                }
            }
        }
    }
}

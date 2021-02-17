using System;
using System.IO;
using System.Xml.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Game1;

namespace UmbrellaToolKit.Tiled
{
    public class TileSet
    {
        private Tile[] Tiles;
        public Texture2D Sprite;
        ContentManager Content;
        String File;

        public TileSet(ContentManager Content, string File)
        {
            this.Content = Content;
            this.File = File;
            this.LoadTiles();
        }


        public ConfigTileSet Config;
        public Point TileSize;

        private void LoadTiles()
        {  
            this.Config = Content.Load<ConfigTileSet>("Maps/"+this.File);
            this.TileSize = new Point(this.Config.tilewidth, this.Config.tileheight);

            this.Sprite = Content.Load<Texture2D>(this.Config.image.source.Replace(".png", "").Replace("../", ""));
            this.SetAllTiles();
        }

        private void SetAllTiles()
        {
            int cols = this.Config.columns;
            int lines = this.Config.tilecount / this.Config.columns;
            int tileWidth = this.Config.tilewidth;
            int tileheight = this.Config.tileheight;
            int x = 0;
            int y = 0;
            int id= 0;

            this.Tiles = new Tile[this.Config.tilecount];

            while(y < lines)
            {
                while(x < cols)
                {
                    Rectangle TilePosition = new Rectangle(
                        new Point(x * tileWidth, y * tileheight),
                        new Point(tileWidth, tileheight));
                    Rectangle TileCollision = new Rectangle(new Point(0,0), new Point(0,0));
                    bool collision = false;
                    string Tagname = "";

                    if(this.Config.tiles.Length > 0)
                    {
                        ConfigTileSetObejctGroup collisions = this.HasCollision(id);
                        if (collisions != null && collisions.type == "collision")
                        {
                            TileCollision = new Rectangle(new Point(collisions.x, collisions.y), new Point(collisions.width, collisions.height));
                            collision = true;
                            Tagname = collisions.name;
                        }
                    }

                    this.Tiles[id] = new Tile(TilePosition, TileCollision, collision, Tagname);

                    id++;
                    x++;
                }
                x = 0;
                y++;
            }
        }

        private ConfigTileSetObejctGroup HasCollision(int id)
        {
            for (int i = 0;  i < this.Config.tiles.Length; i++)
            {
                if (this.Config.tiles[i].id == id)
                {
                    for(int e = 0;  e < this.Config.tiles[i].objectgroup.Length; e++)
                    {
                      return this.Config.tiles[i].objectgroup[e];
                    }
                }
            }
            return null;
        }

        public Tile GetTile(int i) {
            i = i - 1;
            if (this.Tiles.Length > 0 && i >= 0 && this.Tiles.Length > i)
            {
                return this.Tiles[i];
            }
            else return null;
        }

        public void UnloadContent()
        {
            this.File = null;
            this.Sprite.Dispose();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolKit.Collision
{
    public class Grid : GameObject
    {
        public List<List<string>> GridCollides = new List<List<string>>();
        public Scene Scene;
        public List<string> Collides = new List<string>();
        public Point GridSize;

        public Grid()
        {
            this.Collides.Add("1");
            this.Transparent = 0.5f;
        }

        public bool checkOverlap(Point size, Vector2 position)
        {
            Actor actor = new Actor();
            actor.size = size;
            actor.Position = position;
            if (this.checkOverlapActor(actor))
                return true;
            else
                return false;
        }

        int RowGrid;
        int WidthGrid;
        int ColumnGrid;
        int HeightGrid;

        public bool checkOverlapActor(Actor actor = null)
        {
            RowGrid = this.getcell(actor.Left);
            WidthGrid = this.getcell(actor.Right);
            ColumnGrid = this.getcell(actor.Top);
            HeightGrid = this.getcell(actor.Bottom);

            RowGrid = RowGrid < 0 ?  0 : RowGrid;
            ColumnGrid = ColumnGrid < 0 ? 0 : ColumnGrid;
            WidthGrid =  WidthGrid >= this.GridCollides.Count() ? this.GridCollides.Count() - 1 :  WidthGrid;
            HeightGrid = HeightGrid >= this.GridCollides[0].Count() ? this.GridCollides[0].Count() - 1 : HeightGrid;

            bool rt = false;
                
            for (int x = RowGrid; x <= WidthGrid; x++)
            {
                for (int y = ColumnGrid; y <= HeightGrid; y++)
                {
                    if (this.check(actor.size, actor.Position, new Point(8,8), new Vector2(x*8, y*8)))
                    {
                        if (this.Collides.Contains(this.GridCollides[y][x]))
                            rt = true;
                    }
                }
            }

            return rt;
        }


        public bool check(Point size1, Vector2 position1, Point size2, Vector2 position2)
        {
            bool AisToTheRightOfB = position1.X >= position2.X + size2.X;
            bool AisToTheLeftOfB = position1.X + size1.X <= position2.X;
            bool AisAboveB = position1.Y + size1.Y <= position2.Y ;
            bool AisBelowB = position1.Y >= position2.Y + size2.Y;
            
            return !(AisToTheRightOfB
                || AisToTheLeftOfB
                || AisAboveB
                || AisBelowB);
        }

        public List<Actor> GetAllRidingActors()
        {
            List<Actor> rt = new List<Actor>();
            int i = 0;
            while (i < this.Scene.AllActors.Count)
            {
                if (this.Scene.AllActors[i].isRidingGrid(this))
                    rt.Add(this.Scene.AllActors[i]);
                i++;
            }
            return rt;
        }

        public int getcell(float position)
        {
            int cell = (int)(position / 8);
            return cell;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = RowGrid; x <= WidthGrid; x++)
            {
                for (int y = ColumnGrid; y <= HeightGrid; y++)
                {
                    if (this.check(this.Scene.AllActors[0].size, this.Scene.AllActors[0].Position, new Point(8, 8), new Vector2(x * 8, y * 8)))
                    {
                        if (this.Collides.Contains(this.GridCollides[x][y]))
                        {
                            this.Body = new Rectangle(new Point(0,0), new Point(8,8));
                            this.Position = new Vector2(x*8, y*8);
                            this.DrawSprite(spriteBatch);
                        }
                    }
                }
            }
        }
    }
}

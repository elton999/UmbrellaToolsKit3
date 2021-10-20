using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Collision
{
    public class Grid : GameObject
    {
        public List<List<string>> GridCollides = new List<List<string>>();
        public Scene Scene;
        public List<string> Collides = new List<string>();
        public List<string> CollidesRamps = new List<string>();
        public Point GridSize;

        public Grid()
        {
            this.Collides.Add("1");
            this.Transparent = 0.5f;
        }

        public bool checkOverlap(Point size, Vector2 position, Actor ActorReal, bool checkRamps = true)
        {
            Actor actor = new Actor();
            actor.size = size;
            actor.Position = position;
            if (this.checkOverlapActor(actor, checkRamps))
            {
                ActorReal.EdgesIsCollision = actor.EdgesIsCollision;
                return true;
            }
            else
                return false;
        }

        int RowGrid;
        int WidthGrid;
        int ColumnGrid;
        int HeightGrid;

        public bool checkOverlapActor(Actor actor = null, bool checkRamps = true)
        {
            // setting false edges collision to false
            actor.SetFalseAllEdgeCollision();

            RowGrid = this.getcell(actor.Left - this.Origin.X);
            WidthGrid = this.getcell(actor.Right - this.Origin.X);
            ColumnGrid = this.getcell(actor.Top - this.Origin.Y);
            HeightGrid = this.getcell(actor.Bottom - -this.Origin.Y);

            RowGrid = RowGrid < 0 ?  0 : RowGrid;
            ColumnGrid = ColumnGrid < 0 ? 0 : ColumnGrid;
            WidthGrid =  WidthGrid >= this.GridCollides[0].Count() ? this.GridCollides[0].Count() - 1 :  WidthGrid;
            HeightGrid = HeightGrid >= this.GridCollides.Count() ? this.GridCollides.Count() - 1 : HeightGrid;

            bool rt = false;
                
            for (int x = RowGrid; x <= WidthGrid; x++)
            {
                for (int y = ColumnGrid; y <= HeightGrid; y++)
                {
                    if (this.check(actor.size, actor.Position, new Point(this.Scene.CellSize, this.Scene.CellSize), new Vector2(x*this.Scene.CellSize, y* this.Scene.CellSize)))
                    {
                        if (this.Collides.Contains(this.GridCollides[y][x]))
                            rt = true;
                        if (this.CollidesRamps.Contains(this.GridCollides[y][x]) && checkRamps)
                        {
                            // check ramps
                            if(this.CollidesRamps[0] == this.GridCollides[y][x])
                            {
                                // ramp right bottom check
                                if(actor.Bottom - this.Origin.Y- (y * this.Scene.CellSize) > (this.Scene.CellSize - ((actor.Right - this.Origin.X) - x * this.Scene.CellSize)))
                                {
                                    rt = true;
                                    actor.EdgesIsCollision[Actor.EDGES.BOTTOM_RIGHT] = true;
                                }
                            } else if (this.CollidesRamps[1] == this.GridCollides[y][x])
                            {
                                // ramp right bottom check
                                if (actor.Bottom - this.Origin.Y - (y * this.Scene.CellSize) > (((actor.Left - this.Origin.X) - x * this.Scene.CellSize)))
                                {
                                    rt = true;
                                    actor.EdgesIsCollision[Actor.EDGES.BOTTOM_LEFT] = true;
                                }
                            }
                        }
                    }
                }
            }

            return rt;
        }


        public bool check(Point size1, Vector2 position1, Point size2, Vector2 position2)
        {
            bool AisToTheRightOfB = position1.X - this.Origin.X >= position2.X + size2.X;
            bool AisToTheLeftOfB = position1.X - this.Origin.X + size1.X <= position2.X;
            bool AisAboveB = position1.Y - this.Origin.Y + size1.Y <= position2.Y ;
            bool AisBelowB = position1.Y - this.Origin.Y >= position2.Y + size2.Y;
            
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
            int cell = (int)(position / this.Scene.CellSize);
            return cell;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (this.Scene.AllActors.Count > 0)
            {
                for (int x = RowGrid; x <= WidthGrid; x++)
                {
                    for (int y = ColumnGrid; y <= HeightGrid; y++)
                    {
                        if (this.check(this.Scene.AllActors[0].size, this.Scene.AllActors[0].Position,
                            new Point(this.Scene.CellSize, this.Scene.CellSize),
                            new Vector2(x * this.Scene.CellSize, y * this.Scene.CellSize)))
                        {
                            if (this.Collides.Contains(this.GridCollides[x][y]))
                            {
                                this.Body = new Rectangle(new Point(0, 0), new Point(this.Scene.CellSize, this.Scene.CellSize));
                                this.Position = new Vector2(x * this.Scene.CellSize, y * this.Scene.CellSize);
                                this.DrawSprite(spriteBatch);
                            }
                        }
                    }
                }
            }
        }
    }
}

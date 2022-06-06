using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.Collision
{
    public class Grid : GameObject
    {
        public List<List<string>> GridCollides = new List<List<string>>();
        public List<string> Collides = new List<string>();
        public List<string> CollidesRamps = new List<string>();
        public Point GridSize;

        public Grid()
        {
            Collides.Add("1");
            Transparent = 0.5f;
        }

        public bool checkOverlap(Point size, Vector2 position, Actor ActorReal, bool checkRamps = true)
        {
            Actor actor = new Actor();
            actor.size = size;
            actor.Position = position;

            if (checkOverlapActor(actor, checkRamps))
            {
                ActorReal.EdgesIsCollision = actor.EdgesIsCollision;
                return true;
            }
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

            RowGrid = getcell(actor.Left - Origin.X);
            WidthGrid = getcell(actor.Right - Origin.X);
            ColumnGrid = getcell(actor.Top - Origin.Y);
            HeightGrid = getcell(actor.Bottom - -Origin.Y);

            RowGrid = RowGrid < 0 ? 0 : RowGrid;
            ColumnGrid = ColumnGrid < 0 ? 0 : ColumnGrid;
            WidthGrid = WidthGrid >= GridCollides[0].Count() ? GridCollides[0].Count() - 1 : WidthGrid;
            HeightGrid = HeightGrid >= GridCollides.Count() ? GridCollides.Count() - 1 : HeightGrid;

            bool rt = false;

            for (int x = RowGrid; x <= WidthGrid; x++)
            {
                for (int y = ColumnGrid; y <= HeightGrid; y++)
                {
                    if (check(actor.size, actor.Position, new Point(Scene.CellSize, Scene.CellSize), new Vector2(x * Scene.CellSize, y * Scene.CellSize)))
                    {
                        if (Collides.Contains(GridCollides[y][x]))
                            rt = true;
                        if (CollidesRamps.Contains(GridCollides[y][x]) && checkRamps)
                        {
                            // check ramps
                            if (CollidesRamps[0] == GridCollides[y][x])
                            {
                                // ramp right bottom check
                                if (actor.Bottom - Origin.Y - (y * Scene.CellSize) > (Scene.CellSize - ((actor.Right - Origin.X) - x * Scene.CellSize)))
                                {
                                    rt = true;
                                    actor.EdgesIsCollision[Actor.EDGES.BOTTOM_RIGHT] = true;
                                }
                            }

                            if (CollidesRamps[1] == GridCollides[y][x])
                            {
                                // ramp right bottom check
                                if (actor.Bottom - Origin.Y - (y * Scene.CellSize) > (((actor.Left - Origin.X) - x * Scene.CellSize)))
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
            bool AisToTheRightOfB = position1.X - Origin.X >= position2.X + size2.X;
            bool AisToTheLeftOfB = position1.X - Origin.X + size1.X <= position2.X;
            bool AisAboveB = position1.Y - Origin.Y + size1.Y <= position2.Y;
            bool AisBelowB = position1.Y - Origin.Y >= position2.Y + size2.Y;

            return !(AisToTheRightOfB
                || AisToTheLeftOfB
                || AisAboveB
                || AisBelowB);
        }

        public List<Actor> GetAllRidingActors()
        {
            List<Actor> rt = new List<Actor>();
            int i = 0;
            while (i < Scene.AllActors.Count)
            {
                if (Scene.AllActors[i].isRidingGrid(this))
                    rt.Add(Scene.AllActors[i]);
                i++;
            }
            return rt;
        }

        public int getcell(float position) => (int)(position / Scene.CellSize);

        public override void Dispose()
        {
            GridCollides.Clear();
            Collides.Clear();
            CollidesRamps.Clear();
            base.Dispose();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Scene.AllActors.Count > 0)
            {
                for (int x = RowGrid; x <= WidthGrid; x++)
                {
                    for (int y = ColumnGrid; y <= HeightGrid; y++)
                    {
                        if (check(Scene.AllActors[0].size, Scene.AllActors[0].Position,
                            new Point(Scene.CellSize, Scene.CellSize),
                            new Vector2(x * Scene.CellSize, y * Scene.CellSize)))
                        {
                            if (this.Collides.Contains(this.GridCollides[x][y]))
                            {
                                Body = new Rectangle(new Point(0, 0), new Point(Scene.CellSize, Scene.CellSize));
                                Position = new Vector2(x * Scene.CellSize, y * Scene.CellSize);
                                DrawSprite(spriteBatch);
                            }
                        }
                    }
                }
            }
        }
    }
}

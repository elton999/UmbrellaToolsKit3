using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit.Components.Physics;

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

        public bool checkOverlap(Point size, Vector2 position, ActorComponent realActor, bool checkRamps = true)
        {
            ActorComponent actor = new ActorComponent();
            actor.size = size;
            actor.Position = position;

            if (CheckOverlapActor(actor, checkRamps))
            {
                realActor.EdgesIsCollision.Clear();
                realActor.EdgesIsCollision = actor.EdgesIsCollision;
                return true;
            }
            return false;
        }

        int RowGrid;
        int WidthGrid;
        int ColumnGrid;
        int HeightGrid;

        public bool CheckOverlapActor(ActorComponent actor = null, bool checkRamps = true)
        {
            // setting false edges collision to false
            actor.SetFalseAllEdgeCollision();

            RowGrid = GetCell(actor.Left - Origin.X);
            WidthGrid = GetCell(actor.Right - Origin.X);
            ColumnGrid = GetCell(actor.Top - Origin.Y);
            HeightGrid = GetCell(actor.Bottom - Origin.Y);

            RowGrid = RowGrid < 0 ? 0 : RowGrid;
            ColumnGrid = ColumnGrid < 0 ? 0 : ColumnGrid;
            WidthGrid = WidthGrid >= GridCollides[0].Count() ? GridCollides[0].Count() - 1 : WidthGrid;
            HeightGrid = HeightGrid >= GridCollides.Count() ? GridCollides.Count() - 1 : HeightGrid;

            bool rt = false;

            for (int x = RowGrid; x <= WidthGrid; x++)
                for (int y = ColumnGrid; y <= HeightGrid; y++)
                    if (Check(actor.size, actor.Position, new Point(Scene.CellSize), new Vector2(x * Scene.CellSize, y * Scene.CellSize)))
                        rt = CheckRamps(actor, checkRamps, x, y) || Collides.Contains(GridCollides[y][x]) ? true : rt;

            return rt;
        }

        private bool CheckRamps(ActorComponent actor, bool checkRamps, int x, int y)
        {
            if (CollidesRamps.Count() == 0) return false;

            if (!CollidesRamps.Contains(GridCollides[y][x]) || !checkRamps)
                return false;

            bool rt = false;
            // check ramps
            // ramp right bottom check
            if (CollidesRamps[0] == GridCollides[y][x])
                if (actor.Bottom - Origin.Y - (y * Scene.CellSize) > (Scene.CellSize - ((actor.Right - Origin.X) - x * Scene.CellSize)))
                    rt = actor.EdgesIsCollision[ActorComponent.EDGES.BOTTOM_RIGHT] = true;

            // ramp right bottom check
            if (CollidesRamps[1] == GridCollides[y][x])
                if (actor.Bottom - Origin.Y - (y * Scene.CellSize) > (((actor.Left - Origin.X) - x * Scene.CellSize)))
                    rt = actor.EdgesIsCollision[ActorComponent.EDGES.BOTTOM_LEFT] = true;

            return rt;
        }

        public bool Check(Point size1, Vector2 position1, Point size2, Vector2 position2)
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

        public List<ActorComponent> GetAllRidingActors()
        {
            var actors = new List<ActorComponent>();
            int i = 0;
            while (i < Scene.AllActors.Count)
            {
                if (Scene.AllActors[i].IsRidingGrid(this))
                    actors.Add(Scene.AllActors[i]);
                i++;
            }
            return actors;
        }

        public int GetCell(float position) => (int)(position / Scene.CellSize);

        public override void Dispose()
        {
            GridCollides.Clear();
            Collides.Clear();
            CollidesRamps.Clear();
            base.Dispose();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Scene.AllActors.Count <= 0)
                return;

            for (int x = RowGrid; x <= WidthGrid; x++)
            {
                for (int y = ColumnGrid; y <= HeightGrid; y++)
                {
                    if (!Check(Scene.AllActors[0].size, Scene.AllActors[0].Position,
                        new Point(Scene.CellSize),
                        new Vector2(x, y) * Scene.CellSize))
                        continue;

                    if (!Collides.Contains(GridCollides[x][y]))
                        continue;

                    Body = new Rectangle(new Point(0), new Point(Scene.CellSize));
                    Position = new Vector2(x, y) * Scene.CellSize;
                    DrawSprite(spriteBatch);
                }
            }

        }
    }
}

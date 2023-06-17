using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Collision
{
    public class Solid : GameObject
    {
        public bool Collidable = true;

        private float xRemainder = 0;
        private float yRemainder = 0;

        public int Right
        {
            get => (int)(Position.X + size.X);
        }

        public int Left
        {
            get => (int)(Position.X);
        }

        public int Top
        {
            get => (int)(Position.Y);
        }

        public int Bottom
        {
            get => (int)(Position.Y + size.Y);
        }

        public override void Start()
        {
            Scene.AllSolids.Add(this);
            base.Start();
        }

        public void move(float x, float y)
        {
            xRemainder += x;
            yRemainder += y;

            int moveX = (int)Math.Round(xRemainder);
            int moveY = (int)Math.Round(yRemainder);

            if (moveX == 0 && moveY == 0)
                return;

            Collidable = false;

            List<Actor> riding = GetAllRidingActors();

            if (moveX != 0)
            {
                xRemainder -= moveX;
                Position = Position + Vector2.UnitX * moveX;

                if (moveX > 0)
                {
                    int i = 0;
                    for (i = 0; i < Scene.AllActors.Count; i++)
                    {
                        // Push top
                        if (overlapCheck(Scene.AllActors[i]))
                            Scene.AllActors[i].moveX(Right - Scene.AllActors[i].Left, Scene.AllActors[i].squish);
                        // Carry right
                        else if (riding.Contains(Scene.AllActors[i]))
                            Scene.AllActors[i].moveX(moveX, null);
                    }

                }
                else
                {
                    int i = 0;
                    for (i = 0; i < Scene.AllActors.Count; i++)
                    {
                        // Push left
                        if (overlapCheck(Scene.AllActors[i]))
                            Scene.AllActors[i].moveX(Left - Scene.AllActors[i].Right, Scene.AllActors[i].squish);
                        // Carry left
                        else if (riding.Contains(Scene.AllActors[i]))
                            Scene.AllActors[i].moveX(moveX, null);
                    }
                }
            }

            if (moveY != 0)
            {

                yRemainder -= moveY;
                Position = Position + moveY * Vector2.UnitY;

                if (moveY > 0)
                {
                    int i = 0;
                    for (i = 0; i < Scene.AllActors.Count; i++)
                    {
                        if (overlapCheck(Scene.AllActors[i]))
                            Scene.AllActors[i].moveY(Bottom - Scene.AllActors[i].Top, Scene.AllActors[i].squish);
                        else if (riding.Contains(Scene.AllActors[i]))
                            Scene.AllActors[i].moveY(moveY, null);
                        i++;
                    }
                }
                else
                {
                    int i = 0;
                    for (i = 0; i < Scene.AllActors.Count; i++)
                    {
                        if (overlapCheck(Scene.AllActors[i]))
                            Scene.AllActors[i].moveY(Top - Scene.AllActors[i].Bottom, Scene.AllActors[i].squish);
                        else if (riding.Contains(Scene.AllActors[i]))
                            Scene.AllActors[i].moveY(moveY, null);
                    }
                }
            }
            Collidable = true;
        }

        public bool overlapCheck(Actor actor)
        {
            bool AisToTheRightOfB = actor.Left >= Right;
            bool AisToTheLeftOfB = actor.Right <= Left;
            bool AisAboveB = actor.Bottom <= Top;
            bool AisBelowB = actor.Top >= Bottom;
            return !(AisToTheRightOfB
                || AisToTheLeftOfB
                || AisAboveB
                || AisBelowB);
        }

        public bool check(Point size, Vector2 position)
        {
            bool AisToTheRightOfB = position.X >= Right;
            bool AisToTheLeftOfB = position.X + size.X <= Left;
            bool AisAboveB = position.Y + size.Y <= Top;
            bool AisBelowB = position.Y >= Bottom;
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
                if (Scene.AllActors[i].isRiding(this))
                    rt.Add(Scene.AllActors[i]);
                i++;
            }
            return rt;
        }

        public override Solid GetSolid() => this;
    }
}

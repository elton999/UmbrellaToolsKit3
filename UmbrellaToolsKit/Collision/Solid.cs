using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            get => (int)(this.Position.X + this.size.X);
        }

        public int Left
        {
            get => (int)(this.Position.X);
        }

        public int Top
        {
            get => (int)(this.Position.Y);
        }

        public int Bottom
        {
            get => (int)(this.Position.Y + this.size.Y);
        }


        public void move(float x, float y)
        {
            xRemainder += x;
            yRemainder += y;

            int moveX = (int)Math.Round(xRemainder);
            int moveY = (int)Math.Round(yRemainder);


            if (moveX != 0 || moveY != 0)
            {
                this.Collidable = false;

                List<Actor> riding = this.GetAllRidingActors();

                if (moveX != 0)
                {
                    xRemainder -= moveX;
                    this.Position = new Vector2(this.Position.X + moveX, this.Position.Y);

                    if (moveX > 0)
                    {
                        int i = 0;
                        for (i = 0; i < this.Scene.AllActors.Count; i++)
                        {
                            if (overlapCheck(this.Scene.AllActors[i]))
                            {
                                // Push top
                                this.Scene.AllActors[i].moveX(this.Right - this.Scene.AllActors[i].Left, this.Scene.AllActors[i].squish);
                            }
                            else if (riding.Contains(this.Scene.AllActors[i]))
                            {
                                // Carry right
                                this.Scene.AllActors[i].moveX(moveX, null);
                            }
                        }

                    }
                    else
                    {
                        int i = 0;
                        for (i = 0; i < this.Scene.AllActors.Count; i++)
                        {
                            if (overlapCheck(this.Scene.AllActors[i]))
                            {
                                // Push left
                                this.Scene.AllActors[i].moveX(this.Left - this.Scene.AllActors[i].Right, this.Scene.AllActors[i].squish);
                            }
                            else if (riding.Contains(this.Scene.AllActors[i]))
                            {
                                // Carry left
                                this.Scene.AllActors[i].moveX(moveX, null);
                            }
                        }
                    }
                }

                if (moveY != 0)
                {

                    yRemainder -= moveY;
                    this.Position = new Vector2(this.Position.X, this.Position.Y + moveY);

                    if (moveY > 0)
                    {
                        int i = 0;
                        for (i = 0; i < this.Scene.AllActors.Count; i++)
                        {
                            if (overlapCheck(this.Scene.AllActors[i]))
                                this.Scene.AllActors[i].moveY(this.Bottom - this.Scene.AllActors[i].Top, this.Scene.AllActors[i].squish);
                            else if (riding.Contains(this.Scene.AllActors[i]))
                                this.Scene.AllActors[i].moveY(moveY, null);
                            i++;
                        }
                    }
                    else
                    {
                        int i = 0;
                        for (i = 0; i < this.Scene.AllActors.Count; i++)
                        {
                            if (overlapCheck(this.Scene.AllActors[i]))
                                this.Scene.AllActors[i].moveY(this.Top - this.Scene.AllActors[i].Bottom, this.Scene.AllActors[i].squish);
                            else if (riding.Contains(this.Scene.AllActors[i]))
                                this.Scene.AllActors[i].moveY(moveY, null);
                        }
                    }
                }
                this.Collidable = true;
            }
        }

        public bool overlapCheck(Actor actor)
        {
            bool AisToTheRightOfB = actor.Left >= this.Right;
            bool AisToTheLeftOfB = actor.Right <= this.Left;
            bool AisAboveB = actor.Bottom <= this.Top;
            bool AisBelowB = actor.Top >= this.Bottom;
            return !(AisToTheRightOfB
                || AisToTheLeftOfB
                || AisAboveB
                || AisBelowB);
        }

        public bool check(Point size, Vector2 position)
        {
            bool AisToTheRightOfB = position.X >= this.Right;
            bool AisToTheLeftOfB = position.X + size.X <= this.Left;
            bool AisAboveB = position.Y + size.Y <= this.Top;
            bool AisBelowB = position.Y >= this.Bottom;
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
                if (this.Scene.AllActors[i].isRiding(this))
                    rt.Add(this.Scene.AllActors[i]);
                i++;
            }
            return rt;
        }
    }
}

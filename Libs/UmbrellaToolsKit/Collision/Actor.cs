using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Collision
{
    public class Actor : GameObject
    {
        public bool active = true;
        public override void UpdateData(GameTime gameTime)
        {
            base.UpdateData(gameTime);
            this.gravity((float)gameTime.ElapsedGameTime.TotalMilliseconds);
        }

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

        public enum EDGES { TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT };
        public Dictionary<EDGES, bool> EdgesIsCollision = new Dictionary<EDGES, bool> {
            { EDGES.TOP_LEFT, false },
            { EDGES.TOP_RIGHT, false },
            { EDGES.BOTTOM_LEFT, false },
            { EDGES.BOTTOM_RIGHT, false },
        };

        public void SetFalseAllEdgeCollision()
        {
            EdgesIsCollision[EDGES.TOP_LEFT] = false;
            EdgesIsCollision[EDGES.TOP_RIGHT] = false;
            EdgesIsCollision[EDGES.BOTTOM_LEFT] = false;
            EdgesIsCollision[EDGES.BOTTOM_RIGHT] = false;
        }

        public bool AnyCollisionRamps()
        {
            return EdgesIsCollision[EDGES.TOP_LEFT] ||
                EdgesIsCollision[EDGES.TOP_RIGHT] ||
                EdgesIsCollision[EDGES.BOTTOM_LEFT] ||
                EdgesIsCollision[EDGES.BOTTOM_RIGHT];
        }

        public Vector2 gravity2D = new Vector2(0, 8);
        public Vector2 velocity = new Vector2(0, 0);
        public float gravityScale = 1;
        public float velocityDecrecentY = 200;
        public float velocityDecrecentX = 200;
        private void gravity(float deltaTime)
        {
            float maxVelocity = 0.5f;
            velocity += ((gravity2D * gravityScale) * deltaTime);
            float v = velocity.Length();
            if (v > maxVelocity)
            {
                float vs = maxVelocity / v;
                velocity.X = velocity.X * vs;
                velocity.Y = velocity.Y * vs;
            }
            moveX(velocity.X * deltaTime);
            moveY(velocity.Y * deltaTime);
        }

        float xRemainder = 0;
        public void moveX(float amount, Action<string?> onCollideFunction = null)
        {
            xRemainder += amount;
            int move = (int)Math.Round(xRemainder);

            if (move != 0)
            {
                xRemainder -= move;
                int sign = Math.Sign((double)move);
                while (move != 0)
                {
                    Vector2 _position = new Vector2(this.Position.X + sign, this.Position.Y);
                    if (!collideAt(this.Scene.AllSolids, _position) || AnyCollisionRamps())
                    {
                        if (this.EdgesIsCollision[EDGES.BOTTOM_RIGHT] && (sign > 0 || gravity2D.Y == 0))
                            this.Position.Y -= sign;

                        if (this.EdgesIsCollision[EDGES.BOTTOM_LEFT] && (sign < 0 || gravity2D.Y == 0))
                            this.Position.Y += sign;

                        this.Position.X += sign;
                        move -= sign;
                    }
                    else
                    {
                        if (onCollideFunction != null)
                            onCollideFunction(null);
                        break;
                    }
                }
            }
        }

        float yRemainder = 0;
        public void moveY(float amount, Action<string?> onCollideFunction = null)
        {
            yRemainder += amount;
            int move = (int)Math.Round(yRemainder);

            if (move != 0)
            {
                yRemainder -= move;
                int sign = Math.Sign((double)move);
                while (move != 0)
                {
                    Vector2 _position = new Vector2(this.Position.X, this.Position.Y + sign);
                    if (!collideAt(this.Scene.AllSolids, _position))
                    {
                        this.Position.Y += sign;
                        move -= sign;
                    }
                    else
                    {
                        if (onCollideFunction != null)
                            onCollideFunction(null);
                        break;
                    }
                }
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

        public bool overlapCheckPixel(Actor actor)
        {
            bool AisToTheRightOfB = actor.Left > this.Right;
            bool AisToTheLeftOfB = actor.Right < this.Left;
            bool AisAboveB = actor.Bottom < this.Top;
            bool AisBelowB = actor.Top > this.Bottom;
            return !(AisToTheRightOfB
                || AisToTheLeftOfB
                || AisAboveB
                || AisBelowB);
        }

        private bool collideAt(List<Solid> solids, Vector2 position)
        {
            bool rt = false;
            foreach (Solid solid in solids)
            {
                if (solid.check(this.size, position))
                {
                    solid.OnCollision(this.tag);
                    rt = true;
                }
            }
            if (this.Scene.Grid.checkOverlap(this.size, position, this))
                rt = true;

            return rt;
        }

        public virtual bool isRiding(Solid solid)
        {
            if (solid.check(this.size, new Vector2(this.Position.X, this.Position.Y + 1)))
                return true;

            return false;
        }

        public virtual bool isRidingGrid(Grid grid)
        {
            if (grid.checkOverlap(this.size, new Vector2(this.Position.X, this.Position.Y + 1), this))
                return true;

            return false;
        }

        public virtual void squish(string tag = null) { }
    }
}

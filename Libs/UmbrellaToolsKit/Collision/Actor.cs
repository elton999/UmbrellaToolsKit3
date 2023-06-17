using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.Collision
{
    public class Actor : GameObject
    {
        public bool active = true;

        public override void Start()
        {
            Scene.AllActors.Add(this);
            base.Start();
        }

        public override void UpdateData(GameTime gameTime)
        {
            base.UpdateData(gameTime);
            Gravity((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public int Right { get => (int)(Position.X + size.X); }
        public int Left { get => (int)(Position.X); }
        public int Top { get => (int)(Position.Y); }
        public int Bottom { get => (int)(Position.Y + size.Y); }

        public enum EDGES { TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT };
        public Dictionary<EDGES, bool> EdgesIsCollision = new Dictionary<EDGES, bool> {
            { EDGES.TOP_LEFT, false },
            { EDGES.TOP_RIGHT, false },
            { EDGES.BOTTOM_LEFT, false },
            { EDGES.BOTTOM_RIGHT, false },
        };

        public Vector2 Gravity2D = new Vector2(0, 8);
        public Vector2 Velocity = new Vector2(0, 0);
        public float GravityScale = 1;
        public float GravityFallMultiplier = 0.5f;
        public float MaxVelocity = 0.5f;
        public bool YMaxVelocity = true;

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

        public void Gravity(float deltaTime)
        {
            if (Velocity.Length() < 0)
                Velocity += Gravity2D * GravityScale;
            else
                Velocity += Gravity2D * GravityScale * GravityFallMultiplier;

            if (YMaxVelocity)
            {
                float v = (new Vector2(0, Velocity.Y)).Length();
                if (v > MaxVelocity)
                {
                    float vs = MaxVelocity / v;
                    Velocity.Y = Velocity.Y * vs;
                }
            }

            if (!YMaxVelocity)
            {
                float v = Velocity.Length();
                if (v > MaxVelocity)
                {
                    float vs = MaxVelocity / v;
                    Velocity *= vs;
                }
            }

            moveX(Velocity.X * deltaTime);
            moveY(Velocity.Y * deltaTime);
        }

        float xRemainder = 0;
        public void moveX(float amount, Action<string?> onCollideFunction = null)
        {
            xRemainder += amount;
            int move = (int)Math.Round(xRemainder);

            if (move == 0)
                return;

            xRemainder -= move;
            int sign = Math.Sign((double)move);
            while (move != 0)
            {
                Vector2 _position = new Vector2(Position.X + sign, Position.Y);
                if (!collideAt(Scene.AllSolids, _position) || AnyCollisionRamps())
                {
                    Position = Position + Vector2.UnitX * sign;
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

        float yRemainder = 0;
        public void moveY(float amount, Action<string?> onCollideFunction = null)
        {
            yRemainder += amount;
            int move = (int)Math.Round(yRemainder);

            if (move == 0)
                return;

            yRemainder -= move;
            int sign = Math.Sign((double)move);
            while (move != 0)
            {
                var position = Position + Vector2.UnitY * sign;
                if (!collideAt(Scene.AllSolids, position))
                {
                    Position = Position + Vector2.UnitY * sign;
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

        public bool overlapCheckPixel(Actor actor)
        {
            bool AisToTheRightOfB = actor.Left > Right;
            bool AisToTheLeftOfB = actor.Right < Left;
            bool AisAboveB = actor.Bottom < Top;
            bool AisBelowB = actor.Top > Bottom;
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
                if (solid.check(size, position))
                {
                    solid.OnCollision(tag);
                    rt = true;
                }
            }
            if (Scene.Grid != null && Scene.Grid.checkOverlap(size, position, this))
                return true;

            return rt;
        }

        public virtual bool isRiding(Solid solid)
        {
            if (solid.check(size, new Vector2(Position.X, Position.Y + 1)))
                return true;

            return false;
        }

        public virtual bool isRidingGrid(Grid grid)
        {
            if (grid.checkOverlap(size, new Vector2(Position.X, Position.Y + 1), this))
                return true;

            return false;
        }

        public virtual void squish(string tag = null) { }

        public override Actor GetActor() => this;
    }
}

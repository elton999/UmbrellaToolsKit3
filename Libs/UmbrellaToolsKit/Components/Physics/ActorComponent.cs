using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.Collision;
using UmbrellaToolsKit.EditorEngine.Attributes;

namespace UmbrellaToolsKit.Components.Physics
{
    public class ActorComponent : Component
    {
        private float _xRemainder = 0;
        private float _yRemainder = 0;
        
        public Vector2 Position { get => GameObject.Position; set => GameObject.Position = value; }
        public Scene Scene => GameObject.Scene;

        public int Right { get => (int)(Position.X + GameObject.size.X); }
        public int Left { get => (int)Position.X; }
        public int Top { get => (int)Position.Y; }
        public int Bottom { get => (int)(GameObject.Position.Y + GameObject.size.Y); }

        public enum EDGES { TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT };
        public Dictionary<EDGES, bool> EdgesIsCollision = new Dictionary<EDGES, bool> {
            { EDGES.TOP_LEFT, false },
            { EDGES.TOP_RIGHT, false },
            { EDGES.BOTTOM_LEFT, false },
            { EDGES.BOTTOM_RIGHT, false },
        };
        [ShowEditor, Category("Actor")] public Point size = new Point(16, 16);
        [ShowEditor, Category("Actor")] public bool HasGravity = true;
        [ShowEditor, Category("Actor")] public Vector2 Gravity2D = new Vector2(0, 8);
        [ShowEditor, Category("Actor")] public Vector2 Velocity = new Vector2(0, 0);
        [ShowEditor, Category("Actor")] public float GravityScale = 1;
        [ShowEditor, Category("Actor")] public float GravityFallMultiplier = 0.5f;
        [ShowEditor, Category("Actor")] public float MaxVelocity = 0.5f;
        [ShowEditor, Category("Actor")] public bool YMaxVelocity = true;

        public override void Start()
        {
            //Scene.AllActors.Add(this);
            base.Start();
        }

        public override void UpdateData(float deltaTime)
        {
            base.UpdateData(deltaTime);
            if (HasGravity)
            {
                Gravity(deltaTime);
                return;
            }

            MoveX(Velocity.X * deltaTime);
            MoveY(Velocity.Y * deltaTime);
        }


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
                float v = new Vector2(0, Velocity.Y).Length();
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

            MoveX(Velocity.X * deltaTime);
            MoveY(Velocity.Y * deltaTime);
        }

        public void MoveX(float amount, Action<string> onCollideFunction = null)
        {
            _xRemainder += amount;
            int move = (int)Math.Round(_xRemainder);

            if (move == 0)
                return;

            _xRemainder -= move;
            int sign = Math.Sign((double)move);
            while (move != 0)
            {
                Vector2 _position = new Vector2(Position.X + sign, Position.Y);
                if (!CollideAt(Scene.AllSolids, _position) || AnyCollisionRamps())
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

        public void MoveY(float amount, Action<string> onCollideFunction = null)
        {
            _yRemainder += amount;
            int move = (int)Math.Round(_yRemainder);

            if (move == 0)
                return;

            _yRemainder -= move;
            int sign = Math.Sign((double)move);
            while (move != 0)
            {
                var position = Position + Vector2.UnitY * sign;
                if (!CollideAt(Scene.AllSolids, position))
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

        public bool OverlapCheck(ActorComponent actor) => OverlapCheck(actor.size, actor.Position);

        public bool OverlapCheck(Point size, Vector2 position)
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

        public bool OverlapCheckPixel(ActorComponent actor)
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

        private bool CollideAt(List<SolidComponent> solids, Vector2 position)
        {
            bool rt = false;
            foreach (SolidComponent solid in solids)
            {
                if (solid.Check(size, position, this))
                {
                    solid.GameObject.OnCollision(GameObject.tag);
                    rt = true;
                }
            }
            if (Scene.Grid != null && Scene.Grid.checkOverlap(size, position, this, false))
                return true;

            return rt;
        }

        public virtual bool IsRiding(SolidComponent solid) => solid.Check(size, Position + Vector2.UnitY);

        public virtual bool IsRidingGrid(Grid grid) => grid.checkOverlap(size, Position + Vector2.UnitY, this);

        public virtual void Squish(string tag = null) { }
    }
}

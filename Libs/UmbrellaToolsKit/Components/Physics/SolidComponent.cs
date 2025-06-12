using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using UmbrellaToolsKit.EditorEngine.Attributes;

namespace UmbrellaToolsKit.Components.Physics
{
    public class SolidComponent : Component
    {
        private float _yRemainder = 0;
        private float _xRemainder = 0;

        [ShowEditor, Category("Solid")] public Point Size = new Point(16, 16);
        [ShowEditor, Category("Solid")] public bool Collidable = true;
        public Vector2 Position { get => GameObject.Position; set => GameObject.Position = value; }
        public Scene Scene => GameObject.Scene;

        public int Right => (int)(Position.X + Size.X);
        public int Left => (int)Position.X;
        public int Top => (int)Position.Y;
        public int Bottom => (int)(Position.Y + Size.Y);

        public override void Start()
        {
            Scene.AllSolids.Add(this);
            base.Start();
        }

        public void Move(float x, float y)
        {
            _xRemainder += x;
            _yRemainder += y;

            int moveX = (int)Math.Round(_xRemainder);
            int moveY = (int)Math.Round(_yRemainder);

            if (moveX == 0 && moveY == 0)
                return;

            Collidable = false;

            List<ActorComponent> riding = GetAllRidingActors();

            if (moveX != 0)
            {
                _xRemainder -= moveX;
                Position = Position + Vector2.UnitX * moveX;

                if (moveX > 0)
                {
                    int i = 0;
                    for (i = 0; i < Scene.AllActors.Count; i++)
                    {
                        var actor = Scene.AllActors[i];
                        // Push top
                        if (Utils.Collision.OverlapCheck(actor.Size, actor.Position, Size, Position))
                            actor.MoveX(Right - actor.Left, actor.Squish);
                        // Carry right
                        else if (riding.Contains(actor))
                            actor.MoveX(moveX, null);
                    }

                }
                else
                {
                    int i = 0;
                    for (i = 0; i < Scene.AllActors.Count; i++)
                    {
                        var actor = Scene.AllActors[i];
                        // Push left
                        if (Utils.Collision.OverlapCheck(actor.Size, actor.Position, Size, Position))
                            actor.MoveX(Left - actor.Right, actor.Squish);
                        // Carry left
                        else if (riding.Contains(Scene.AllActors[i]))
                            actor.MoveX(moveX, null);
                    }
                }
            }

            if (moveY != 0)
            {
                _yRemainder -= moveY;
                Position = Position + moveY * Vector2.UnitY;

                if (moveY > 0)
                {
                    int i = 0;
                    for (i = 0; i < Scene.AllActors.Count; i++)
                    {
                        var actor = Scene.AllActors[i];
                        if (Utils.Collision.OverlapCheck(actor.Size, actor.Position, Size, Position))
                            actor.MoveY(Bottom - actor.Top, actor.Squish);
                        else if (riding.Contains(actor))
                            actor.MoveY(moveY, null);
                        i++;
                    }
                }
                else
                {
                    int i = 0;
                    for (i = 0; i < Scene.AllActors.Count; i++)
                    {
                        var actor = Scene.AllActors[i];
                        if (Utils.Collision.OverlapCheck(actor.Size, actor.Position, Size, Position))
                            actor.MoveY(Top - actor.Bottom, actor.Squish);
                        else if (riding.Contains(actor))
                            actor.MoveY(moveY, null);
                    }
                }
            }
            Collidable = true;
        }

        public virtual bool Check(Point size, Vector2 position, ActorComponent actor = null)
        {
            return Utils.Collision.OverlapCheck(size, position, Size, Position);
        }

        public List<ActorComponent> GetAllRidingActors()
        {
            List<ActorComponent> rt = new List<ActorComponent>();
            int i = 0;
            while (i < Scene.AllActors.Count)
            {
                if (Scene.AllActors[i].IsRiding(this))
                    rt.Add(Scene.AllActors[i]);
                i++;
            }
            return rt;
        }
    }
}

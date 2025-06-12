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

        [ShowEditor, Category("Solid")] public Point size = new Point(16, 16);
        [ShowEditor, Category("Solid")] public bool Collidable = true;
        public Vector2 Position { get => GameObject.Position; set => GameObject.Position = value; }
        public Scene Scene => GameObject.Scene;

        public int Right
        {
            get => (int)(Position.X + size.X);
        }

        public int Left
        {
            get => (int)Position.X;
        }

        public int Top
        {
            get => (int)Position.Y;
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
                        // Push top
                        if (OverlapCheck(Scene.AllActors[i]))
                            Scene.AllActors[i].MoveX(Right - Scene.AllActors[i].Left, Scene.AllActors[i].Squish);
                        // Carry right
                        else if (riding.Contains(Scene.AllActors[i]))
                            Scene.AllActors[i].MoveX(moveX, null);
                    }

                }
                else
                {
                    int i = 0;
                    for (i = 0; i < Scene.AllActors.Count; i++)
                    {
                        // Push left
                        if (OverlapCheck(Scene.AllActors[i]))
                            Scene.AllActors[i].MoveX(Left - Scene.AllActors[i].Right, Scene.AllActors[i].Squish);
                        // Carry left
                        else if (riding.Contains(Scene.AllActors[i]))
                            Scene.AllActors[i].MoveX(moveX, null);
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
                        if (OverlapCheck(Scene.AllActors[i]))
                            Scene.AllActors[i].MoveY(Bottom - Scene.AllActors[i].Top, Scene.AllActors[i].Squish);
                        else if (riding.Contains(Scene.AllActors[i]))
                            Scene.AllActors[i].MoveY(moveY, null);
                        i++;
                    }
                }
                else
                {
                    int i = 0;
                    for (i = 0; i < Scene.AllActors.Count; i++)
                    {
                        if (OverlapCheck(Scene.AllActors[i]))
                            Scene.AllActors[i].MoveY(Top - Scene.AllActors[i].Bottom, Scene.AllActors[i].Squish);
                        else if (riding.Contains(Scene.AllActors[i]))
                            Scene.AllActors[i].MoveY(moveY, null);
                    }
                }
            }
            Collidable = true;
        }

        public bool OverlapCheck(ActorComponent actor)
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

        public virtual bool Check(Point size, Vector2 position, ActorComponent actor = null)
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

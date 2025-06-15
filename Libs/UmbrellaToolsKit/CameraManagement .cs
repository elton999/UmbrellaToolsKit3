using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit
{
    public class CameraController
    {
        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public float Zoom { get; set; }
        public Vector2 Origin { get => new Vector2(Scene.Sizes.X / 2, Scene.Sizes.Y / 2) / 1; }
        public Vector2 Target { get; set; }
        public Vector2 ScreenSize { get; set; }
        public Vector2 ScreenTargetAreaLimits { get; set; }
        public Vector2 ScreenSizeLimits { get; set; }
        public float MoveSpeed { get; set; }
        public Vector2 maxPosition;
        public Vector2 minPosition;
        public Scene Scene;

        public CameraController()
        {
            Zoom = 1f;
            Scale = 1f;
            MoveSpeed = 8f;
            Rotation = 0f;
        }

        public void update(float deltaTime)
        {

            InitialPosition = _position;
            Shake(deltaTime);

            if (Target != Vector2.Zero)
            {
                moveX(deltaTime);
                moveY(deltaTime);
                if (Scene.PixelArt) Position = Position.ToPoint().ToVector2();
            }

        }

        public void CheckActorAndSolids()
        {
            /*Collision.Actor _actorCamera = new Collision.Actor();
            _actorCamera.size = Scene.Sizes;
            _actorCamera.Position = new Vector2(Position.X - Origin.X, Position.Y - Origin.Y);
            List<Collision.Actor> _actorsList = new List<Collision.Actor>();
            _actorsList.AddRange(Scene.AllActors);

            foreach (Collision.Actor actor in _actorsList)
                if (actor.overlapCheck(_actorCamera))
                    actor.IsVisible();
                else
                    actor.IsNotvisible();*/
        }

        public bool UseLevelLimits = true;
        public void moveX(float delta)
        {
            _position.X = Microsoft.Xna.Framework.MathHelper.Lerp(Position.X, Target.X, MoveSpeed * delta);
            if (UseLevelLimits)
            {
                float maxValue = Scene.LevelSize.X + Scene.ScreenOffset.X - Origin.X;
                float minValue = Scene.ScreenOffset.X + Origin.X;
                _position.X = Math.Max(_position.X, minValue);
                _position.X = Math.Min(_position.X, maxValue);
            }
        }

        public void moveY(float delta)
        {
            _position.Y = Microsoft.Xna.Framework.MathHelper.Lerp(Position.Y, Target.Y, MoveSpeed * delta);

            if (UseLevelLimits)
            {
                float maxValue = Scene.LevelSize.Y + Scene.ScreenOffset.Y - Origin.Y;
                float minValue = Scene.ScreenOffset.Y + Origin.Y;
                _position.Y = Math.Min(_position.Y, maxValue);
                _position.Y = Math.Max(_position.Y, minValue);

            }
        }

        #region shake

        public float TimeShake;
        public static readonly Random getRandom = new Random();
        public Vector2 InitialPosition;
        public float ShakeMagnitude = 0.05f;

        private void Shake(float deltaTime)
        {
            if (TimeShake > 0)
            {
                int randomX = getRandom.Next(-5, 5);
                int randomY = getRandom.Next(-5, 5);

                Target = new Vector2(
                    InitialPosition.X + randomX * ShakeMagnitude,
                    InitialPosition.Y + randomY * ShakeMagnitude
                );
                TimeShake -= 1;
            }
        }
        #endregion

        public Matrix TransformMatrix()
        {
            return Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
                Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, Zoom));

        }

    }
}

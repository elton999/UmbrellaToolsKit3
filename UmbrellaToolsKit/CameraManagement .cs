using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit
{
    public class CameraManagement
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
        public Vector2 Origin { get => new Vector2(this.Scene.Sizes.X / 2, this.Scene.Sizes.Y / 2) / 1; }
        public Vector2 Target { get; set; }
        public Vector2 ScreemSize { get; set; }
        public Vector2 ScreemTargetAreaLimits { get; set; }
        public Vector2 ScreemSizeLimits { get; set; }
        public float MoveSpeed { get; set; }
        public Vector2 maxPosition;
        public Vector2 minPosition;
        public Scene Scene;

        public CameraManagement()
        {
            Zoom = 1f;
            Scale = 1f;
            MoveSpeed = 8f;
            Rotation = 0f;
        }

        public void update(GameTime gameTime)
        {

            this.InitialPosition = this._position;
            this.Shake(gameTime);

            if (this.Target != Vector2.Zero)
            {
                var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.moveX(delta);
                this.moveY(delta);
            }

        }

        public void CheckActorAndSolids()
        {
            Collision.Actor _actorCamera = new Collision.Actor();
            _actorCamera.size = this.Scene.Sizes;
            _actorCamera.Position = new Vector2(this.Position.X - this.Origin.X, this.Position.Y - this.Origin.Y);
            List<Collision.Actor> _actorsList = new List<Collision.Actor>();
            _actorsList.AddRange(this.Scene.AllActors);

            foreach (Collision.Actor actor in _actorsList)
                if (actor.overlapCheck(_actorCamera))
                    actor.Isvisible();
                else
                    actor.IsNotvisible();
        }

        public bool UseLevelLimits = true;
        public void moveX(float delta)
        {
            this._position.X = MathHelper.Lerp(this.Position.X, this.Target.X, this.MoveSpeed * delta);
            if (this.UseLevelLimits)
            {
                float maxValue = this.Scene.LevelSize.X + this.Scene.ScreemOffset.X - this.Origin.X;
                float minValue = this.Scene.ScreemOffset.X + this.Origin.X;
                this._position.X = Math.Max(this._position.X, minValue);
                this._position.X = Math.Min(this._position.X, maxValue);
            }
        }

        public void moveY(float delta)
        {
            this._position.Y = MathHelper.Lerp(this.Position.Y, this.Target.Y, this.MoveSpeed * delta);

            if (this.UseLevelLimits)
            {
                float maxValue = this.Scene.LevelSize.Y - this.Scene.ScreemOffset.Y - this.Origin.Y;
                float minValue = this.Scene.ScreemOffset.Y + this.Origin.Y;
                this._position.Y = Math.Min(this._position.Y, maxValue);
                this._position.Y = Math.Max(this._position.Y, minValue);

            }
        }

        #region shake

        public float TimeShake;
        public static readonly Random getRandom = new Random();
        public Vector2 InitialPosition;
        public float ShakeMagnitude = 0.05f;

        private void Shake(GameTime gameTime)
        {
            if (this.TimeShake > 0)
            {
                int randomX = getRandom.Next(-5, 5);
                int randomY = getRandom.Next(-5, 5);

                this.Target = new Vector2(
                    this.InitialPosition.X + randomX * this.ShakeMagnitude,
                    this.InitialPosition.Y + randomY * this.ShakeMagnitude
                );
                this.TimeShake -= 1;
            }
        }
        #endregion

        public Matrix TransformMatrix()
        {
            return Matrix.CreateRotationZ(this.Rotation) *
                Matrix.CreateTranslation(-(int)this.Position.X, -(int)this.Position.Y, 0) *
                Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, Zoom));

        }

    }
}

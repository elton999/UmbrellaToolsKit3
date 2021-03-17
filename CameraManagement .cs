using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace UmbrellaToolKit
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
            if (this.Target != null)
            {
                var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this.Target.X + this.Origin.X < this.Scene.LevelSize.X + this.Scene.ScreemOffset.X && Target.X - this.Origin.X > this.Scene.ScreemOffset.X)
                    this.moveX(delta);
                if (this.Target.Y + this.Origin.Y < this.Scene.LevelSize.Y + this.Scene.ScreemOffset.X && Target.Y - this.Origin.Y > this.Scene.ScreemOffset.Y)
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

        public void moveX(float delta)
        {
            this._position.X = lerp(this.Position.X, this.Target.X, this.MoveSpeed * delta);
        }

        public void moveY(float delta)
        {
            this._position.Y = lerp(this.Position.Y, this.Target.Y, this.MoveSpeed * delta);
        }

        public float lerp(float min, float max, float value){
            return min + (max - min) * value;
	    }

    public Matrix TransformMatrix()
        {
            return Matrix.CreateRotationZ(this.Rotation) * 
                Matrix.CreateTranslation(-(int)this.Position.X, -(int)this.Position.Y, 0) * 
                Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                Matrix.CreateScale(new Vector3(Zoom, Zoom, Zoom));

        }

    }
}

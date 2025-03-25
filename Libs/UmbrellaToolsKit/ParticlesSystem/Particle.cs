using System;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.ParticlesSystem
{
    public class Particle : GameObject
    {
        public float LifeTime = 10000f;
        public Vector2 Velocity = new Vector2(0, 1);
        public float Angle = 0;
        public bool DecreaseScale = false;
        public float DecreaseScaleSpeed = 10.0f;

        public override void Start()
        {
            var spriteSize = new Vector2(Sprite.Width, Sprite.Height);
            Origin = -MathUtils.Divide(spriteSize);
            base.Start();
        }

        public override void Update(float deltaTime)
        {
            LifeTime -= deltaTime;
            Rotation += Angle * deltaTime;
            Position += Velocity * deltaTime;
            if (DecreaseScale) Scale = Math.Max(Scale - deltaTime * DecreaseScaleSpeed, 0.0f);

            Origin = MathUtils.Divide(Vector2.One);
        }
    }
}
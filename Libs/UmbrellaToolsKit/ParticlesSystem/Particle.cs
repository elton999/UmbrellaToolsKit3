using System.Numerics;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.ParticlesSystem
{
    public class Particle : GameObject
    {
        public float LifeTime = 10000f;
        public Vector2 Velocity = new Vector2(0, 1);
        public float Angle = 0;

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            LifeTime -= deltaTime;
            Rotation += Angle * deltaTime;
            Position += Velocity * deltaTime;

            Origin = Vector2.One / 2f;
        }
    }
}
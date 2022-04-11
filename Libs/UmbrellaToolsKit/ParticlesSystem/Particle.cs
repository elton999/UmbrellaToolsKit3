using System.Numerics;
using Microsoft.Xna.Framework;

namespace UmbrellaToolsKit.ParticlesSystem
{
    public class Particle : GameObject
    {
        public float LifeTime = 10000f;
        public Vector2 Velocity = new Vector2(0, 1);
        public Vector2 Angle = new Vector2(0, 1);

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            LifeTime -= deltaTime;
            Position += Velocity * Scale * deltaTime * Angle;
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.ParticlesSystem
{
    public class ParticlesSystem : GameObject
    {
        private float _timer = 0.0f;

        public bool IsPlaying { get => (IsOnTime && Particles.Count > 0) || EmitsFor == TypeEmitter.INFINITE; }

        private bool IsOnTime { get => EmitsFor == TypeEmitter.FOR_TIME && _timer >= 0; }

        public enum TypeEmitter { FOR_TIME, INFINITE }

        public List<Particle> Particles = new List<Particle>();
        public List<Texture2D> Sprites = new List<Texture2D>();
        public float EmitterTime = 0.0f;
        public TypeEmitter EmitsFor = TypeEmitter.INFINITE;

        public int MaxParticles = 5;

        public float ParticleVelocityAngle = 90f;
        public float ParticleAngleEmitter = 180f - (90f / 2f);
        public float ParticleMaxScale = 20;
        public float ParticleAngle = 90f;
        public float ParticleLifeTime = 1800f;
        public float ParticleTransparent = 0.5f;
        public float ParticleVelocity = 200f;
        public float ParticleAngleRotation = 90f;
        public float ParticleRadiusSpawn = 10f;

        public void Restart() => _timer = EmitterTime;

        public override void Update(GameTime gameTime)
        {
            _timer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (IsOnTime || EmitsFor == TypeEmitter.INFINITE)
                ImitParticles();

            CheckLifeTimeParticles(gameTime);
        }

        public void CheckLifeTimeParticles(GameTime gameTime)
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].Update(gameTime);
                if (Particles[i].LifeTime <= 0f)
                {
                    Particles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void ImitParticles()
        {
            for (int i = 0; i < MaxParticles; i++)
                Particles.Add(CreateParticle());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            BeginDraw(spriteBatch);
            DrawParticles(spriteBatch);
            EndDraw(spriteBatch);
        }

        public void DrawParticles(SpriteBatch spriteBatch)
        {
            foreach (var particle in Particles)
                particle.DrawSprite(spriteBatch);
        }

        public virtual Particle CreateParticle()
        {
            var random = new Random();
            var velocityDirection = new Vector2(
                (float)Math.Sin((double)MathHelper.ToRadians((float)random.NextDouble() * ParticleVelocityAngle + ParticleAngleEmitter)),
                (float)Math.Cos((double)MathHelper.ToRadians((float)random.NextDouble() * ParticleVelocityAngle + ParticleAngleEmitter)));

            return new Particle()
            {
                Position = Position + ParticleRadiusSpawn * velocityDirection * (float)random.NextDouble(),
                Scale = (float)random.NextDouble() * ParticleMaxScale,
                Angle = MathHelper.ToRadians((float)random.NextDouble() * ParticleAngle / 100f),
                Transparent = ParticleTransparent,
                Velocity = velocityDirection * ParticleVelocity / 1000f,
                Sprite = Sprites[random.Next(0, Sprites.Count - 1)],
                LifeTime = (float)random.NextDouble() * ParticleLifeTime
            };
        }
    }
}
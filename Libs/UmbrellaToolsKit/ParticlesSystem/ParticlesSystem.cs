using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UmbrellaToolsKit.ParticlesSystem
{
    public class ParticlesSystem : GameObject
    {
        private float _timer = 0.0f;
        private bool _isPlaying = false;
        public bool IsPlaying { get => _isPlaying && ((IsOnTime && Particles.Count > 0) || EmitsFor == TypeEmitter.INFINITE); }
        private bool IsOnTime { get => EmitsFor == TypeEmitter.FOR_TIME && _timer > 0.0f; }

        public enum TypeEmitter { FOR_TIME, INFINITE }

        public List<Particle> Particles = new List<Particle>();
        public List<Texture2D> Sprites = new List<Texture2D>();
        [ShowEditor] public float EmitterTime = 0.0f;
        public TypeEmitter EmitsFor = TypeEmitter.INFINITE;

        [ShowEditor] public int MaxParticles = 5;

        [ShowEditor] public float ParticleVelocityAngle = 90f;
        [ShowEditor] public float ParticleAngleEmitter = 180f - (90f / 2f);
        [ShowEditor] public float ParticleMaxScale = 20;
        [ShowEditor] public float ParticleAngle = 90f;
        [ShowEditor] public float ParticleLifeTime = 1800f;
        [ShowEditor] public float ParticleTransparent = 0.5f;
        [ShowEditor] public float ParticleVelocity = 200f;
        [ShowEditor] public float ParticleAngleRotation = 90f;
        [ShowEditor] public float ParticleRadiusSpawn = 10f;
        [ShowEditor] public bool ParticleDecreaseScale = false;
        [ShowEditor] public float ParticleScaleSpeed = 10.0f;

        public override void Start() => Tag = nameof(ParticlesSystem);

        public void Restart() => _timer = EmitterTime;

        public void Play()
        {
            _isPlaying = true;
            Restart();
        }

        public override void Update(float deltaTime)
        {
            deltaTime = MathUtils.SecondsToMilliseconds(deltaTime);
            _timer -= deltaTime;

            if (IsOnTime || EmitsFor == TypeEmitter.INFINITE)
                ImitParticles();

            CheckLifeTimeParticles(deltaTime);
        }

        public void CheckLifeTimeParticles(float deltaTime)
        {
            for (int i = 0; i < Particles.Count; i++)
            {
                Particles[i].Update(deltaTime);
                if (Particles[i].LifeTime <= 0f)
                {
                    Particles[i].Dispose();
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
            BeginDraw(spriteBatch, Layer != Layers.UI);
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
                (float)Math.Sin(MathHelper.ToRadians((float)random.NextDouble() * ParticleVelocityAngle + ParticleAngleEmitter)),
                (float)Math.Cos(MathHelper.ToRadians((float)random.NextDouble() * ParticleVelocityAngle + ParticleAngleEmitter)));

            return new Particle()
            {
                Position = Position + ParticleRadiusSpawn * velocityDirection * (float)random.NextDouble(),
                Scale = (float)random.NextDouble() * ParticleMaxScale,
                Angle = MathHelper.ToRadians((float)random.NextDouble() * ParticleAngle / 100f),
                Transparent = ParticleTransparent,
                Velocity = velocityDirection * ParticleVelocity / 1000f,
                Sprite = Sprites[random.Next(0, Sprites.Count - 1)],
                LifeTime = (float)random.NextDouble() * ParticleLifeTime,
                DecreaseScale = ParticleDecreaseScale,
                DecreaseScaleSpeed = ParticleScaleSpeed,
            };
        }
    }
}
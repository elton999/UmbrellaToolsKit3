using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using UmbrellaToolsKit.Interfaces;
using UmbrellaToolsKit.Utils;
using Microsoft.Xna.Framework.Graphics;
using UmbrellaToolsKit.EditorEngine.Attributes;

namespace UmbrellaToolsKit.Components.ParticlesSystem
{
    public class ParticlesComponent : Component
    {
        [ShowEditor] private ParticlesConfig _particlesConfigDebug = new ParticlesConfig();
        [ShowEditor] private IParticle _particleConfig;
        private IObjectPooling _pool;

        [ShowEditor] private float _timer = 0.0f;
        [ShowEditor] private bool _isPlaying = false;

        public bool IsPlaying { get => _isPlaying; }
        public bool IsOnTime { get => _particleConfig != null && _particleConfig.EmitsFor == EmiterType.FOR_TIME && _timer > 0.0f; }

        public IParticle ParticleConfig => _particleConfig;
        [ShowEditor] public List<ParticleRender> Particles = new List<ParticleRender>();

        public void SetPartileConfig(IParticle particleConfig) => _particleConfig = _particlesConfigDebug.UpdateConfig(particleConfig);

        public void UpdateParticleSettings() => _particleConfig = _particlesConfigDebug;

        public void Restart() => _timer = _particleConfig.EmitterTime;

        public void Play()
        {
            _isPlaying = true;
            Restart();
        }

        public void Pause() => _isPlaying = false;

        public override void Start() => GameObject.ExtraDraw += DrawParticles;

        public override void OnDestroy() => GameObject.ExtraDraw -= DrawParticles;

        public override void Update(float deltaTime)
        {
            if (!_isPlaying) return;

            deltaTime = MathUtils.SecondsToMilliseconds(deltaTime);
            _timer -= deltaTime;

            if (IsOnTime || _particleConfig.EmitsFor == EmiterType.INFINITE)
                ImitParticles();

            CheckLifeTimeParticles(deltaTime);
        }

        public void DrawParticles(SpriteBatch spriteBatch)
        {
            foreach (var particle in Particles)
                particle.DrawSprite(spriteBatch);
        }

        private void CheckLifeTimeParticles(float deltaTime)
        {
            for (int particleIndex = 0; particleIndex < Particles.Count; particleIndex++)
            {
                Particles[particleIndex].Update(deltaTime);
                if (Particles[particleIndex].LifeTime <= 0.0f)
                {
                    Particles.RemoveAt(particleIndex);
                    particleIndex--;
                }
            }
        }

        private void ImitParticles()
        {
            for (int particleCount = 0; particleCount < _particleConfig.MaxParticles; particleCount++)
                Particles.AddIfNew(GetParticle());
        }

        private ParticleRender GetParticle()
        {
            if (_pool == null)
                _pool = new ObjectPooling<ParticleRender>(_particleConfig.MaxRenderParticles);

            var random = new Random();
            var velocityDirection = new Vector2(
                (float)Math.Sin(MathHelper.ToRadians((float)random.NextDouble() * _particleConfig.ParticleVelocityAngle + _particleConfig.ParticleAngleEmitter)),
                (float)Math.Cos(MathHelper.ToRadians((float)random.NextDouble() * _particleConfig.ParticleVelocityAngle + _particleConfig.ParticleAngleEmitter)));

            var particle = (ParticleRender)_pool.GetObject();

            particle.Position = GameObject.Position + _particleConfig.ParticleRadiusSpawn * velocityDirection * (float)random.NextDouble();
            particle.Scale = (float)random.NextDouble() * _particleConfig.ParticleMaxScale;
            particle.Angle = MathHelper.ToRadians((float)random.NextDouble() * _particleConfig.ParticleAngle / 100f);
            particle.Transparent = _particleConfig.ParticleTransparent;
            particle.Velocity = velocityDirection * MathUtils.MillisecondsToSeconds(_particleConfig.ParticleVelocity);
            particle.Sprite = _particleConfig.Sprites[random.Next(0, _particleConfig.Sprites.Count - 1)];
            particle.LifeTime = (float)random.NextDouble() * _particleConfig.ParticleLifeTime;
            particle.DecreaseScale = _particleConfig.ParticleDecreaseScale;
            particle.DecreaseScaleSpeed = _particleConfig.ParticleScaleSpeed;

            return particle;
        }

    }
}

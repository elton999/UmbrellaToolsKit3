using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using UmbrellaToolsKit.EditorEngine.Attributes;

namespace UmbrellaToolsKit.Components.ParticlesSystem
{
    [System.Serializable]
    public class ParticlesConfig : IParticle
    {
        [ShowEditor] private float _emitterTime;
        [ShowEditor] private EmiterType _emitsFor;
        [ShowEditor] private int _maxParticles;
        [ShowEditor] private float _particleVelocityAngle;
        [ShowEditor] private float _particleAngleEmitter;
        [ShowEditor] private float _particleMaxScale;
        [ShowEditor] private float _particleAngle;
        [ShowEditor] private float _particleLifeTime;
        [ShowEditor] private float _particleTransparent;
        [ShowEditor] private float _particleVelocity;
        [ShowEditor] private float _particleAngleRotation;
        [ShowEditor] private float _particleRadiusSpawn;
        [ShowEditor] private bool _particleDecreaseScale;
        [ShowEditor] private float _particleScaleSpeed;
        [ShowEditor] private int _maxRenderParticles;

        public List<Texture2D> Sprites { get; set; }
        public float EmitterTime { get => _emitterTime; set => _emitterTime = value; }
        public EmiterType EmitsFor { get => _emitsFor; set => _emitsFor = value; }
        public int MaxParticles { get => _maxParticles; set => _maxParticles = value; }
        public float ParticleVelocityAngle { get => _particleVelocityAngle; set => _particleVelocityAngle = value; }
        public float ParticleAngleEmitter { get => _particleAngleEmitter; set => _particleAngleEmitter = value; }
        public float ParticleMaxScale { get => _particleMaxScale; set => _particleMaxScale = value; }
        public float ParticleAngle { get => _particleAngle; set => _particleAngle = value; }
        public float ParticleLifeTime { get => _particleLifeTime; set => _particleLifeTime = value; }
        public float ParticleTransparent { get => _particleTransparent; set => _particleTransparent = value; }
        public float ParticleVelocity { get => _particleVelocity; set => _particleVelocity = value; }
        public float ParticleAngleRotation { get => _particleAngleRotation; set => _particleAngleRotation = value; }
        public float ParticleRadiusSpawn { get => _particleRadiusSpawn; set => _particleRadiusSpawn = value; }
        public bool ParticleDecreaseScale { get => _particleDecreaseScale; set => _particleDecreaseScale = value; }
        public float ParticleScaleSpeed { get => _particleScaleSpeed; set => _particleScaleSpeed = value; }
        public int MaxRenderParticles { get => _maxRenderParticles; set => _maxRenderParticles = value; }

        public IParticle UpdateConfig(IParticle particle)
        {
            Sprites = particle.Sprites;
            EmitterTime = particle.EmitterTime;
            EmitsFor = particle.EmitsFor;
            MaxParticles = particle.MaxParticles;
            ParticleVelocityAngle = particle.ParticleVelocityAngle;
            ParticleAngleEmitter = particle.ParticleAngleEmitter;
            ParticleMaxScale = particle.ParticleMaxScale;
            ParticleAngle = particle.ParticleAngle;
            ParticleLifeTime = particle.ParticleLifeTime;
            ParticleTransparent = particle.ParticleTransparent;
            ParticleVelocity = particle.ParticleVelocity;
            ParticleAngleRotation = particle.ParticleAngleRotation;
            ParticleRadiusSpawn = particle.ParticleRadiusSpawn;
            ParticleDecreaseScale = particle.ParticleDecreaseScale;
            ParticleScaleSpeed = particle.ParticleScaleSpeed;
            MaxRenderParticles = particle.MaxRenderParticles;

            return this;
        }
    }
}

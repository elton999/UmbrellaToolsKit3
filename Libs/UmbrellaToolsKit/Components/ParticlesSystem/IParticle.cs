using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace UmbrellaToolsKit.Components.ParticlesSystem
{
    public interface IParticle
    {
        public List<Texture2D> Sprites { get; set; }
        public float EmitterTime { get; set; }
        public EmiterType EmitsFor { get; set; }

        public int MaxParticles { get; set; }
        public int MaxRenderParticles { get; set; }

        public float ParticleVelocityAngle { get; set; }
        public float ParticleAngleEmitter { get; set; }
        public float ParticleMaxScale { get; set; }
        public float ParticleAngle { get; set; }
        public float ParticleLifeTime { get; set; }
        public float ParticleTransparent { get; set; }
        public float ParticleVelocity { get; set; }
        public float ParticleAngleRotation { get; set; }
        public float ParticleRadiusSpawn { get; set; }
        public bool ParticleDecreaseScale { get; set; }
        public float ParticleScaleSpeed { get; set; }
    }
}

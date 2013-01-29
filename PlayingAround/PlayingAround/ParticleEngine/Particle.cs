using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlayingAround.ParticleEngine
{
    public class Particle
    {
        private readonly TexturedQuad textureQuad;

        public Particle(int lifespan, TexturedQuad texQuad, GraphicsDevice graphicsDevice)
        {
            textureQuad = texQuad;
            Lifespan = lifespan;
        }

        public void Update(double totalMilliseconds)
        {
            Position += Velocity;

            float particleAge = (float)(totalMilliseconds - Inception) / Lifespan;
            Alpha = MathHelper.Lerp(1, 0, particleAge);

            if (Lifespan < (totalMilliseconds - Inception))
                IsAlive = false;
        }

        public void Draw(GraphicsDevice graphicsDevice, Matrix viewMatrix, Matrix projectionMatrix)
        {
            textureQuad.Draw(viewMatrix, projectionMatrix, Matrix.CreateTranslation(Position), Alpha, .01f);
        }

        public bool IsAlive { get; set; }
        public float Inception { get; set; }
        public float Lifespan { get; private set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        public float Alpha
        {
            get;
            set;
        }
    }
}

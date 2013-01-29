using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PlayingAround.ParticleEngine
{
    public class TexturedQuad
    {
        private static readonly Vector2 UpperLeft = new Vector2(0, 0);
        private static readonly Vector2 UpperRight = new Vector2(1, 0);
        private static readonly Vector2 BottomLeft = new Vector2(0, 1);
        private static readonly Vector2 BottomRight = new Vector2(1, 1);

        private readonly VertexBuffer vertexBuffer;
        private readonly BasicEffect effect;


        public TexturedQuad(GraphicsDevice graphicsDevice, Texture2D texture, int width, int height)
        {
            
            VertexPositionColorTexture[] vertices = CreateQuadVertices(width, height);
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData(vertices);

            effect = new BasicEffect(graphicsDevice) { TextureEnabled = true, Texture = texture };
        }

        public TexturedQuad(GraphicsDevice graphicsDevice, Texture2D texture, Vector3 BottomLeft, Vector3 BottomRight, Vector3 TopLeft, Vector3 TopRight)
        {
            VertexPositionColorTexture[] vertices = CreateQuadVertices(BottomLeft, BottomRight, TopLeft, TopRight);
            vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData(vertices);

            effect = new BasicEffect(graphicsDevice) { TextureEnabled = true, Texture = texture };
        }

        private VertexPositionColorTexture[] CreateQuadVertices(Vector3 BL, Vector3 BR, Vector3 TL, Vector3 TR)
        {

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];
            vertices[0] = new VertexPositionColorTexture(TL, Color.Red, UpperLeft);
            vertices[1] = new VertexPositionColorTexture(TR, Color.Red, UpperRight);
            vertices[2] = new VertexPositionColorTexture(BL, Color.Red, BottomLeft);
            vertices[3] = new VertexPositionColorTexture(BR, Color.Red, BottomRight);
            return vertices;

        }

        private static VertexPositionColorTexture[] CreateQuadVertices(int width, int height)
        {
            int halfWidth = width / 2;
            int halfHeight = height / 2;

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[4];

            vertices[0] = new VertexPositionColorTexture(new Vector3(-halfWidth, halfHeight, 0), Color.Red, UpperLeft);
            vertices[1] = new VertexPositionColorTexture(new Vector3(halfWidth, halfHeight, 0), Color.Red, UpperRight);
            vertices[2] = new VertexPositionColorTexture(new Vector3(-halfWidth, -halfHeight, 0), Color.Red, BottomLeft);
            vertices[3] = new VertexPositionColorTexture(new Vector3(halfWidth, -halfHeight, 0), Color.Red, BottomRight);
            

            return vertices;
        }

        public void Draw(Matrix viewMatrix, Matrix projectionMatrix, Matrix worldMatrix, float alpha, float scale)
        {
            effect.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            effect.World = worldMatrix *  Matrix.CreateRotationY(Game1.camera.leftrightRot) * Matrix.CreateScale(scale);
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;

            effect.Alpha = alpha;
            //effect.LightingEnabled = true;
            effect.VertexColorEnabled = true;
            

            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                effect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleStrip, 0, 2);
            }
        }

        public float Alpha
        {
            get { return effect.Alpha; }
            set { effect.Alpha = value; }
        }
    }
}

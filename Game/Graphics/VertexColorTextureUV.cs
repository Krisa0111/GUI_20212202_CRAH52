using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class VertexColorTextureUV : IVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Color;
        public Vector2 TextureUV;

        public int Size { get => 11; }
        public int Stride { get => Size * sizeof(float); }

        private static int PositionOffset { get => 0; }
        private static int NormalOffset { get => 3 * sizeof(float); }
        private static int ColorOffset { get => 6 * sizeof(float); }
        private static int TextureUVOffset { get => 9 * sizeof(float); }

        public VertexColorTextureUV(Vector3 position, Vector3 normal, Vector3 color, Vector2 textureUv)
        {
            Position = position;
            Normal = normal;
            Color = color;
            TextureUV = textureUv;
        }

        public float[] GetData()
        {
            return new[]
            {
            Position.X, Position.Y, Position.Z,
            Normal.X, Normal.Y, Normal.Z,
            Color.X, Color.Y, Color.Z,
            TextureUV.X, TextureUV.Y
            };
        }

        public void SetAttributes(VertexArrayObject vao)
        {
            vao.Bind();

            // position
            vao.LinkAttrib(0, 3, VertexAttribPointerType.Float, Stride, PositionOffset);
            // normal
            vao.LinkAttrib(1, 3, VertexAttribPointerType.Float, Stride, NormalOffset);
            // color
            vao.LinkAttrib(2, 3, VertexAttribPointerType.Float, Stride, ColorOffset);
            // texture uv
            vao.LinkAttrib(3, 2, VertexAttribPointerType.Float, Stride, TextureUVOffset);

            vao.Unbind();
        }
    }
}

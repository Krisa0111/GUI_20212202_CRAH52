using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class VertexTextureUV : IVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureUV;

        public int Size { get => 8; }
        public int Stride { get => Size * sizeof(float); }

        private static int PositionOffset { get => 0; }
        private static int NormalOffset { get => 3 * sizeof(float); }
        private static int TextureUVOffset { get => 6 * sizeof(float); }

        public VertexTextureUV(Vector3 position, Vector3 normal, Vector2 textureUv)
        {
            Position = position;
            Normal = normal;
            TextureUV = textureUv;
        }

        public float[] GetData()
        {
            return new[]
            {
            Position.X, Position.Y, Position.Z,
            Normal.X, Normal.Y, Normal.Z,
            TextureUV.X, TextureUV.Y
            };
        }

        public void SetAttributes(VertexArrayObject vao)
        {
            // position
            vao.LinkAttrib(0, 3, VertexAttribPointerType.Float, Stride, PositionOffset);
            // normal
            vao.LinkAttrib(1, 3, VertexAttribPointerType.Float, Stride, NormalOffset);
            // texture uv
            vao.LinkAttrib(3, 2, VertexAttribPointerType.Float, Stride, TextureUVOffset);
        }
    }
}

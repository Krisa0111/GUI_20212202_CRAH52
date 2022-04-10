using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class VertexT : IVertex
    {
        public Vector3 Position;
        public Vector2 TextureUV;

        public int Size { get => 5; }
        public int Stride { get => Size * sizeof(float); }

        private static int PositionOffset { get => 0; }
        private static int TextureUVOffset { get => 3 * sizeof(float); }

        public VertexT(Vector3 position, Vector2 textureUv)
        {
            Position = position;
            TextureUV = textureUv;
        }

        public float[] GetData()
        {
            return new[]
            {
            Position.X, Position.Y, Position.Z,
            TextureUV.X, TextureUV.Y
            };
        }

        public void SetAttributes(VertexArray vao)
        {
            // position
            vao.LinkAttrib(0, 3, VertexAttribPointerType.Float, Stride, PositionOffset);
            // texture uv
            vao.LinkAttrib(3, 2, VertexAttribPointerType.Float, Stride, TextureUVOffset);
        }
    }
}

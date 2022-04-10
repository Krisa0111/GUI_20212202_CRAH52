using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class VertexNC : IVertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Color;

        public int Size { get => 9; }
        public int Stride { get => Size * sizeof(float); }

        private static int PositionOffset { get => 0; }
        private static int NormalOffset { get => 3 * sizeof(float); }
        private static int ColorOffset { get => 6 * sizeof(float); }

        public VertexNC(Vector3 position, Vector3 normal, Vector3 color)
        {
            Position = position;
            Normal = normal;
            Color = color;
        }

        public float[] GetData()
        {
            return new[]
            {
            Position.X, Position.Y, Position.Z,
            Normal.X, Normal.Y, Normal.Z,
            Color.X, Color.Y, Color.Z
            };
        }

        public void SetAttributes(VertexArray vao)
        {
            // position
            vao.LinkAttrib(0, 3, VertexAttribPointerType.Float, Stride, PositionOffset);
            // normal
            vao.LinkAttrib(1, 3, VertexAttribPointerType.Float, Stride, NormalOffset);
            // color
            vao.LinkAttrib(2, 3, VertexAttribPointerType.Float, Stride, ColorOffset);
        }
    }
}

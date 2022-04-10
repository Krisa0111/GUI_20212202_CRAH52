﻿using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class VertexN : IVertex
    {
        public Vector3 Position;
        public Vector3 Normal;

        public int Size { get => 6; }
        public int Stride { get => Size * sizeof(float); }

        private static int PositionOffset { get => 0; }
        private static int NormalOffset { get => 3 * sizeof(float); }

        public VertexN(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }

        public float[] GetData()
        {
            return new[]
            {
            Position.X, Position.Y, Position.Z,
            Normal.X, Normal.Y, Normal.Z
            };
        }

        public void SetAttributes(VertexArray vao)
        {
            // position
            vao.LinkAttrib(0, 3, VertexAttribPointerType.Float, Stride, PositionOffset);
            // normal
            vao.LinkAttrib(1, 3, VertexAttribPointerType.Float, Stride, NormalOffset);
        }
    }
}

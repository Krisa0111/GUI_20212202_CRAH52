using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector3 Color;
        public Vector2 TextureUV;

        public Vertex(Vector3 position, Vector3 normal, Vector3 color, Vector2 textureUv)
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

        public static int Size { get => 11; }
        public static int Stride { get => Size * sizeof(float); }

    }
}

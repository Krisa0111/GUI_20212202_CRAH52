using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ResourceLoader
{
    public class Vertex
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoords;

        public Vertex(Vector3 position, Vector3 normal, Vector2 textureUv)
        {
            Position = position;
            Normal = normal;
            TextureCoords = textureUv;
        }

        public float[] GetData()
        {
            return new[]
            {
            Position.X, Position.Y, Position.Z,
            Normal.X, Normal.Y, Normal.Z,
            TextureCoords.X, TextureCoords.Y
            };
        }
    }
}

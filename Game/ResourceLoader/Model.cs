using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ResourceLoader
{
    public class Model
    {
        public string Name { get; }

        private Vertex[] vertices;
        private uint[] indices;
        public Material Material { get; }


        public float[] Vertices
        {
            get
            {
                float[] vertices = new float[this.vertices.Length * this.vertices[0].GetData().Length];
                int i = 0;
                foreach (Vertex vertex in this.vertices)
                {
                    float[] data = vertex.GetData();
                    foreach (float item in data)
                    {
                        vertices[i++] = item;
                    }
                }
                return vertices;
            }
        }

        public uint[] Indices => indices;

        public (Vector3 P1, Vector3 P2, Vector3 P3)[] Triangles { get; }

        public Model(string name, Vertex[] vertices, uint[] indices, Material material)
        {
            Name = name;
            this.vertices = vertices;
            this.indices = indices;
            Material = material;
            Triangles = new (Vector3, Vector3, Vector3)[indices.Length / 3];

            for (int i = 0; i < Triangles.Length; i++)
            {
                Triangles[i].P1 = vertices[indices[i * 3 + 0]].Position;
                Triangles[i].P2 = vertices[indices[i * 3 + 1]].Position;
                Triangles[i].P3 = vertices[indices[i * 3 + 2]].Position;
            }
            ;
        }

    }
}

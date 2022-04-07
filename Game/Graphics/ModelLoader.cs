using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    class Model<T> where T : IVertex
    {
        public IList<T> vertices;
        public IList<uint> indices;
        public Model()
        {
            vertices = new List<T>();
            indices = new List<uint>();
        }
    }
    
    internal class ModelLoader
    {
        private class Index
        {
            public Index(string rawData)
            {
                string[] data = rawData.Split('/');
                Vertex = int.Parse(data[0]);
                Texture = int.Parse(data[1]);
                Normal = int.Parse(data[2]);
            }

            public int Vertex { get; private set; }
            public int Texture { get; private set; }
            public int Normal { get; private set; }
        }

        private class Face
        {
            public Face(string pointA, string pointB, string pointC)
            {
                Points = new Index[3];
                Points[0] = new Index(pointA);
                Points[1] = new Index(pointB);
                Points[2] = new Index(pointC);
            }

            public Index[] Points { get; private set; }
        }

        public static Model<VertexTextureUV> loadModel(string file)
        {
            var model = new Model<VertexTextureUV>();

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<Face> faces = new List<Face>();

            StreamReader sr = new StreamReader(file);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] parts = line.Split(' ');
                if (parts.Length > 0)
                {
                    if (parts[0] == "v")
                    {
                        vertices.Add(
                            new Vector3(
                                float.Parse(parts[1]),
                                float.Parse(parts[2]),
                                float.Parse(parts[3])
                                )
                            );
                    }
                    else if (parts[0] == "vt")
                    {
                        uvs.Add(
                            new Vector2(
                                float.Parse(parts[1]),
                                float.Parse(parts[2])
                                )
                            );
                    }
                    else if (parts[0] == "vn")
                    {
                        normals.Add(
                            new Vector3(
                                float.Parse(parts[1]),
                                float.Parse(parts[2]),
                                float.Parse(parts[3])
                                )
                            );
                    } 
                    else if (parts[0] == "f")
                    {
                        faces.Add(new Face(parts[1], parts[2], parts[3]));
                    }
                }
            }
            sr.Close();


            foreach (var face in faces)
            {
                foreach (var item in face.Points)
                {
                    VertexTextureUV vertex = new VertexTextureUV(
                    vertices[item.Vertex-1],
                    normals[item.Normal-1],
                    uvs[item.Texture-1]);
                    model.vertices.Add(vertex);
                }
            }

            // TODO index vertices
            for (int i = 0; i < model.vertices.Count; i++)
            {
                int index = i;
                for (int j = 0; j < i; j++)
                {
                    if (model.vertices[i].GetData().SequenceEqual(model.vertices[j].GetData()))
                    {
                        model.vertices.RemoveAt(i);
                        index = j;
                        i--;
                        break;
                    }
                }
                model.indices.Add((uint)index);

            }
            return model;
        }
    }
}

using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
        public Material material;

        public Model()
        {
            vertices = new List<T>();
            indices = new List<uint>();
        }

        public StaticMesh<T> GetMesh()
        {
            return new StaticMesh<T>(vertices, indices);
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

        public static Model<VertexNT> LoadModel(string file)
        {
            var model = new Model<VertexNT>();

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
                                float.Parse(parts[1], CultureInfo.InvariantCulture),
                                float.Parse(parts[2], CultureInfo.InvariantCulture),
                                float.Parse(parts[3], CultureInfo.InvariantCulture)
                                )
                            );
                    }
                    else if (parts[0] == "vt")
                    {
                        uvs.Add(
                            new Vector2(
                                float.Parse(parts[1], CultureInfo.InvariantCulture),
                                float.Parse(parts[2], CultureInfo.InvariantCulture)
                                )
                            );
                    }
                    else if (parts[0] == "vn")
                    {
                        normals.Add(
                            new Vector3(
                                float.Parse(parts[1], CultureInfo.InvariantCulture),
                                float.Parse(parts[2], CultureInfo.InvariantCulture),
                                float.Parse(parts[3], CultureInfo.InvariantCulture)
                                )
                            );
                    }
                    else if (parts[0] == "f")
                    {
                        faces.Add(new Face(parts[1], parts[2], parts[3]));
                    }
                    else if (parts[0] == "mtllib")
                    {
                        string path = Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), file));
                        string mtlFile = Path.Combine(path, parts[1]);
                        model.material = LoadMaterial(mtlFile);
                    }
                }
            }
            sr.Close();


            foreach (var face in faces)
            {
                foreach (var item in face.Points)
                {
                    VertexNT vertex = new VertexNT(
                    vertices[item.Vertex-1],
                    normals[item.Normal-1],
                    uvs[item.Texture-1]);
                    model.vertices.Add(vertex);
                }
            }

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

            Debug.WriteLine($" {file} loaded with {model.vertices.Count} vertices and {model.indices.Count} indices");

            return model;
        }

        public static Material LoadMaterial(string file)
        {
            Material material = null;
            // TODO
            return material;
        }
    }
}

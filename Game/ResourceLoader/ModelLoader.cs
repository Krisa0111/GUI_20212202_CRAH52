using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ResourceLoader
{
    public class ModelLoader
    {
        private class Index
        {
            public int Vertex { get; private set; }
            public int Texture { get; private set; }
            public int Normal { get; private set; }

            public Index(string rawData)
            {
                string[] data = rawData.Split('/');
                Vertex = int.Parse(data[0]);
                Texture = int.Parse(data[1]);
                Normal = int.Parse(data[2]);
            }
        }

        private class Face
        {
            public Index[] Indices { get; private set; }

            public Face(string pointA, string pointB, string pointC)
            {
                Indices = new Index[3];
                Indices[0] = new Index(pointA);
                Indices[1] = new Index(pointB);
                Indices[2] = new Index(pointC);
            }
        }

        Dictionary<string, Model> models;
        private MaterialLoader materialLoader;
        private static ModelLoader modelLoader;

        private ModelLoader()
        {
            models = new Dictionary<string, Model>();
            materialLoader = new MaterialLoader();
        }

        public static ModelLoader GetInstance()
        {
            if (modelLoader == null) modelLoader = new ModelLoader();
            return modelLoader;
        }

        public void CacheModels(IList<string> files, Action<float> progressCallback)
        {
            float progress = 0.0f;

            foreach (var file in files)
            {
                GetModel(file);
                progress += 1.0f / files.Count;
                progressCallback(progress);
            }
        }

        public Model GetModel(string file)
        {
            if (!models.TryGetValue(file, out Model model))
            {
                model = LoadModel(file);
                models.Add(file, model);
            }
            return model;
        }

        /// <summary>
        /// Loads model from obj file
        /// Only supports triangulated faces
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private Model LoadModel(string file)
        {
            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<Face> faces = new List<Face>();
            Material material = new Material();

            StreamReader sr = new StreamReader(file);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] parts = line.Split(' ');
                if (parts.Length > 0)
                {
                    if (parts[0] == "v") // position
                    {
                        vertices.Add(
                            new Vector3(
                                float.Parse(parts[1], CultureInfo.InvariantCulture),
                                float.Parse(parts[2], CultureInfo.InvariantCulture),
                                float.Parse(parts[3], CultureInfo.InvariantCulture)
                                )
                            );
                    }
                    else if (parts[0] == "vt") // texture coords
                    {
                        uvs.Add(
                            new Vector2(
                                float.Parse(parts[1], CultureInfo.InvariantCulture),
                                float.Parse(parts[2], CultureInfo.InvariantCulture)
                                )
                            );
                    }
                    else if (parts[0] == "vn") // normal
                    {
                        normals.Add(
                            new Vector3(
                                float.Parse(parts[1], CultureInfo.InvariantCulture),
                                float.Parse(parts[2], CultureInfo.InvariantCulture),
                                float.Parse(parts[3], CultureInfo.InvariantCulture)
                                )
                            );
                    }
                    else if (parts[0] == "f") // 1 face
                    {
                        // triangulate, assuming ngons are convex and coplanar
                        for (int i = 2; i + 1 < parts.Length; i++)
                        {
                            faces.Add(new Face(parts[1], parts[i], parts[i + 1]));
                        }
                    }
                    else if (parts[0] == "mtllib")
                    {
                        string path = Path.GetDirectoryName(Path.Combine(Directory.GetCurrentDirectory(), file));
                        string mtlFile = Path.Combine(path, parts[1]);
                        material = materialLoader.LoadMaterial(mtlFile);
                    }
                }
            }
            sr.Close();

            List<Vertex> modelVertices = new List<Vertex>();
            List<uint> modelIndices = new List<uint>();

            foreach (var face in faces)
            {
                foreach (var item in face.Indices)
                {
                    Vertex vertex = new Vertex(
                    vertices[item.Vertex - 1],
                    normals[item.Normal - 1],
                    uvs[item.Texture - 1]);
                    modelVertices.Add(vertex);
                }
            }

            for (int i = 0; i < modelVertices.Count; i++)
            {
                int index = i;
                for (int j = 0; j < i; j++)
                {
                    if (modelVertices[i].GetData().SequenceEqual(modelVertices[j].GetData()))
                    {
                        modelVertices.RemoveAt(i);
                        index = j;
                        i--;
                        break;
                    }
                }
                modelIndices.Add((uint)index);

            }

            var model = new Model(
                Path.GetFileNameWithoutExtension(file),
                modelVertices.ToArray(),
                modelIndices.ToArray(),
                material
                );

            return model;
        }


    }
}

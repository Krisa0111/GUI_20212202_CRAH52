using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ResourceLoader
{
    internal class MaterialLoader
    {
        //private IDictionary<string, Material> materials = new Dictionary<string, Material>();

        public Material LoadMaterial(string file)
        {
            string materialName = Path.GetFileNameWithoutExtension(file);
            string currentDirectory = Path.GetFullPath(Path.GetDirectoryName(file));
            Material material;

            //if (materials.TryGetValue(materialName, out material))
            //    return material;

            Image diffuse = null;
            Image specular = null;
            Image normal = null;

            StreamReader sr = new StreamReader(file);
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] parts = line.Split(' ');

                string texFile = Path.GetFullPath(Path.Combine(currentDirectory, parts.Last()));

                if (parts[0] == "map_Ka") // ambient texture
                {
                    diffuse = new Image(texFile);
                }
                else if (parts[0] == "map_Kd") // diffuse texture
                {
                    diffuse = new Image(texFile);
                }
                else if (parts[0] == "map_Ks") // specular texture
                {
                    specular = new Image(texFile);
                }
                else if (parts[0] == "map_Ns") // specular highlight texture
                {
                    specular = new Image(texFile);
                }
                else if (parts[0] == "map_bump" ||
                         parts[0] == "map_Bump" ||
                         parts[0] == "bump") // normal texture
                {
                    normal = new Image(texFile);
                }
                else if (parts[0] == "map_refl" || parts[0] == "refl") // reflection
                {
                    specular = new Image(texFile);
                }
            }

            sr.Close();

            material = new Material(diffuse, specular, normal);
            //materials.Add(materialName, material);

            return material;
        }
    }
}

using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class Material
    {
        private Texture texture;
        private Texture specularMap;
        private Texture normalMap;
        private float shininess;

        public Material(Texture texture, Texture specularMap, float shininess = 32)
        {
            this.texture = texture;
            this.specularMap = specularMap;
            this.shininess = shininess;
        }

        public Material(string texture, string specularMap, float shininess = 32)
        {
            this.texture = Texture.LoadFromFile(texture);
            this.specularMap = Texture.LoadFromFile(specularMap);
            this.shininess = shininess;
        }

        public void Use()
        {
            texture?.Use(TextureUnit.Texture0);
            specularMap?.Use(TextureUnit.Texture1);
            normalMap?.Use(TextureUnit.Texture2);
        }
    }
}

using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics.OpenGL
{
    internal class Material
    {
        private Texture texture;
        private Texture specularMap;
        private Texture normalMap;
        private float shininess;

        public Material(Texture texture, Texture specularMap, Texture normalMap, float shininess = 32)
        {
            this.texture = texture;
            this.specularMap = specularMap;
            this.normalMap = normalMap;
            this.shininess = shininess;
        }

        public Material(string texture, string specularMap, string normalMap, float shininess = 32)
        {
            if (!string.IsNullOrEmpty(texture)) this.texture = Texture.LoadFromFile(texture);
            if (!string.IsNullOrEmpty(specularMap)) this.specularMap = Texture.LoadFromFile(specularMap);
            if (!string.IsNullOrEmpty(normalMap)) this.normalMap = Texture.LoadFromFile(normalMap);
            this.shininess = shininess;
        }

        public void Use()
        {

            if (texture != null)
                texture.Use(TextureUnit.Texture0);
            else
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            if (specularMap != null)
                specularMap.Use(TextureUnit.Texture1);
            else
            {
                GL.ActiveTexture(TextureUnit.Texture1);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            if (normalMap != null)
                normalMap.Use(TextureUnit.Texture2);
            else
            {
                GL.ActiveTexture(TextureUnit.Texture2);
                GL.BindTexture(TextureTarget.Texture2D, 0);
            }
        }
    }
}

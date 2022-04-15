using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ResourceLoader
{
    public class Material
    {
        public Image texture { get; }
        public Image specularMap { get; }
        public Image normalMap { get; }
        public float shininess { get; }

        public Material()
        {

        }

        public Material(Image texture, Image specularMap, Image normalMap, float shininess = 32)
        {
            this.texture = texture;
            this.specularMap = specularMap;
            this.normalMap = normalMap;
            this.shininess = shininess;
        }
    }
}

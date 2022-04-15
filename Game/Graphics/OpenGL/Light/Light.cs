using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal abstract class Light
    {
        public Vector3 AmbientIntensity { get; set; }
        public Vector3 DiffuseIntensity { get; set; }
        public Vector3 SpecularIntensity { get; set; }

        public Light(Vector3 ambientIntensity, Vector3 diffuseIntensity, Vector3 specularIntensity)
        {
            AmbientIntensity = ambientIntensity;
            DiffuseIntensity = diffuseIntensity;
            SpecularIntensity = specularIntensity;
        }
    }
}

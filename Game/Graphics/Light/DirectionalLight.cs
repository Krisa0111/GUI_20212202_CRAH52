using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class DirectionalLight : Light
    {
        public Vector3 Direction { get; set; }

        public DirectionalLight(Vector3 direction, Vector3 ambientIntensity, Vector3 diffuseIntensity, Vector3 specularIntensity) 
            : base(ambientIntensity, diffuseIntensity, specularIntensity)
        {
            Direction = direction;
        }

        public void SetUniforms(Shader shader)
        {
            shader.Use();
            shader.SetVector3("dirLight.direction", Vector3.Normalize(Direction));
            shader.SetVector3("dirLight.ambient", AmbientIntensity);
            shader.SetVector3("dirLight.diffuse", DiffuseIntensity);
            shader.SetVector3("dirLight.specular", SpecularIntensity);
        }
    }
}

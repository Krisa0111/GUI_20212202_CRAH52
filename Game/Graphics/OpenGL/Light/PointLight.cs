using Game.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class PointLight : Light, IPointLight
    {
        public Vector3 Position { get; set; }
        public float Constant { get; set; }
        public float Linear { get; set; }
        public float Quadratic { get; set; }

        public PointLight(Vector3 position, float constant, float linear, float quadratic, Vector3 ambientIntensity, Vector3 diffuseIntensity, Vector3 specularIntensity)
            : base(ambientIntensity, diffuseIntensity, specularIntensity)
        {
            Position = position;
            Constant = constant;
            Linear = linear;
            Quadratic = quadratic;
        }

        public void SetUniforms(Shader shader, int index)
        {
            shader.Use();
            shader.SetVector3("pointLights[" + index + "].position", Position);
            shader.SetVector3("pointLights[" + index + "].ambient", AmbientIntensity);
            shader.SetVector3("pointLights[" + index + "].diffuse", DiffuseIntensity);
            shader.SetVector3("pointLights[" + index + "].specular", SpecularIntensity);
            shader.SetFloat("pointLights[" + index + "].constant", Constant);
            shader.SetFloat("pointLights[" + index + "].linear", Linear);
            shader.SetFloat("pointLights[" + index + "].quadratic", Quadratic);
        }
    }
}

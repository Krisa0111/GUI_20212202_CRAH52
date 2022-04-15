using OpenTK.Mathematics;

namespace Game.Graphics
{
    public interface IPointLight
    {
        Vector3 Position { get; set; }
        Vector3 AmbientIntensity { get; set; }
        Vector3 DiffuseIntensity { get; set; }
        Vector3 SpecularIntensity { get; set; }
        float Constant { get; set; }
        float Linear { get; set; }
        float Quadratic { get; set; }
    }
}
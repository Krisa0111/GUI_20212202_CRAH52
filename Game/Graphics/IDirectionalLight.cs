using OpenTK.Mathematics;

namespace Game.Graphics
{
    public interface IDirectionalLight
    {
        Vector3 Direction { get; set; }
        Vector3 DiffuseIntensity { get; set; }
        Vector3 SpecularIntensity { get; set; }
        Vector3 AmbientIntensity { get; set; }
    }
}
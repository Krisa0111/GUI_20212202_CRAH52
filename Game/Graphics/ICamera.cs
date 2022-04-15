using Game.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace Game.Graphics
{
    public interface ICamera
    {
        float Fov { get; set; }
        float Pitch { get; set; }
        Vector3 Position { get; set; }
        float Yaw { get; set; }
    }
}
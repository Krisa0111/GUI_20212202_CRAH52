using Game.ResourceLoader;
using Game.ViewModel;
using OpenTK.Mathematics;
using System.Collections.Generic;

namespace Game.Graphics
{
    public interface IRenderer
    {
        ICamera Camera { get; }
        IDirectionalLight DirectionalLight { get; }
        IPointLight[] PointLights { get; }
        bool ShowNormals { get; set; }
        bool ShowWireframe { get; set; }

        void BeginFrame();
        void Dispose();
        void EndFrame();
        void Render(IList<Entity> entities);
        void RenderMultible(Model model, Vector3 startPosition, Vector3 offset, int instances);
        void Resize(int width, int height, int defaultFbo);
    }
}
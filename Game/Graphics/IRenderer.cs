using Game.ResourceLoader;
using Game.ViewModel;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;

namespace Game.Graphics
{
    public interface IRenderer : IDisposable
    {
        ICamera Camera { get; }
        IDirectionalLight DirectionalLight { get; }
        IPointLight[] PointLights { get; }
        bool ShowNormals { get; set; }
        bool ShowWireframe { get; set; }
        bool ShowColliders { get; set; }

        void BeginFrame();
        void EndFrame();
        void Render(IReadOnlyCollection<Entity> entities);
        void Render(Entity entity);
        void RenderMultible(Model model, Vector3 startPosition, Vector3 offset, int instances);
        void Resize(int width, int height, int defaultFbo);
    }
}
using Game.Graphics;
using Game.Logic;
using Game.ViewModel;
using Game.ViewModel.Entities;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using OVector3 = OpenTK.Mathematics.Vector3;

namespace Game
{
    public class MainMenuDisplay : IMainMenuDisplay
    {
        IMainMenuModel model = Ioc.Default.GetService<IMainMenuModel>();
        IRenderer renderer = Ioc.Default.GetService<IRenderer>();
        MainMenuLogic logic;

        public MainMenuDisplay()
        {
            renderer.Camera.Position = new OVector3(0.0f, 1.5f, -1.5f);
            renderer.Camera.Yaw = 90;
            renderer.Camera.Pitch = -15;
            logic = new MainMenuLogic();
        }

        public void Reset()
        {
            logic.Reset();
        }

        public void Render(double delta)
        {
            logic.Update(delta);

            int i = 0;
            foreach (var item in model.Entities)
            {
                if (item.Position.Z < model.Player.Position.Z - 20) continue;
                if (item is StreetLight)
                {
                    renderer.PointLights[i].Position = new OVector3(item.Position.X, item.Position.Y, item.Position.Z);

                    OVector3 color = new OVector3(1, .9f, .75f);
                    float intensity = 10;
                    renderer.PointLights[i].AmbientIntensity = color * 0.005f * intensity;
                    renderer.PointLights[i].DiffuseIntensity = color * 0.3f * intensity;
                    renderer.PointLights[i].SpecularIntensity = color * 0.8f * intensity;

                    i++;
                }

                if (i >= renderer.PointLights.Length) break;
            }

            for (; i < renderer.PointLights.Length; i++)
            {
                renderer.PointLights[i].AmbientIntensity = OVector3.Zero;
                renderer.PointLights[i].DiffuseIntensity = OVector3.Zero;
                renderer.PointLights[i].SpecularIntensity = OVector3.Zero;
            }

            float distance = model.Player.Position.Z - 1 - renderer.Camera.Position.Z;

            renderer.Camera.Position = new OVector3(model.Player.Position.X, 1.4f, renderer.Camera.Position.Z + (float)(distance * delta * 7));

            renderer.BeginFrame();

            renderer.Render(model.Player);

            renderer.Render(model.Entities);

            renderer.EndFrame();
        }

        public void Resize(int width, int height, int defaultFbo)
        {
            renderer.Resize(width, height, defaultFbo);
        }
    }
}

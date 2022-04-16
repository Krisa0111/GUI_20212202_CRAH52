using Game.Graphics;
using Game.Graphics.OpenGL;
using Game.Logic;
using Game.ViewModel;
using Game.ViewModel.Entities;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Game.Renderer
{
    internal class GameDisplay : IDisposable
    {
        Entity box;
        IGameModel gameModel = Ioc.Default.GetService<IGameModel>();
        IRenderer renderer = Ioc.Default.GetService<IRenderer>();
        IGameLogic logic = Ioc.Default.GetService<IGameLogic>();
        Map map;
        DispatcherTimer dt;
        Stopwatch timer;
        Player player;
        public GameDisplay(int width, int height)
        {
            // TODO: dependency injection (IOC)
           
            // TODO: move this to logic
            box = new Box(new Vector3(1, 0.5f, 5));
            player = gameModel.Player;

            gameModel.Entities.Add(box);
            map = new MapTunnel();
            
            renderer.Camera.Position = new OpenTK.Mathematics.Vector3(0.0f, 1.5f, -1.5f);
            renderer.Camera.Yaw = 90;
            renderer.Camera.Pitch = -15;

            timer = new Stopwatch();
            dt= new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(1000/120);
            dt.Tick += (sender, eventargs) =>
            {
                timer.Stop();
                TimeSpan elapsed = timer.Elapsed;
                timer.Start();
                logic.Update(elapsed);
                
            };
            dt.Start();

        }

        public void Resize(int width, int height, int defaultFbo)
        {
            renderer.Resize(width, height, defaultFbo);
        }

        public void Render()
        {
            //player.CurrentAnimatonStep += 1.0f;
            //player.Position += Vector3.UnitZ*0.07f;

            OpenTK.Mathematics.Vector3 startPos = new()
            {
                X = 0,
                Y = 0,
                Z = player.Position.Z - (player.Position.Z % map.ModelLength) 
            };

            for (int i = 0; i < renderer.PointLights.Length; i++)
            {
                renderer.PointLights[i].Position = startPos + new OpenTK.Mathematics.Vector3(0, 2, map.ModelLength * i - map.ModelLength);
            }

            renderer.Camera.Position = new OpenTK.Mathematics.Vector3(player.Position.X, player.Position.Y + 1.5f, player.Position.Z - 1.5f);

            renderer.BeginFrame();

            renderer.Render(gameModel.Entities);

            renderer.RenderMultible(map.Model, startPos, new OpenTK.Mathematics.Vector3(0, 0, map.ModelLength), 30);

            renderer.EndFrame();
        }

        public void Update(TimeSpan delta)
        {
            
        }

        public void Dispose()
        {
            renderer.Dispose();
        }
    }
}

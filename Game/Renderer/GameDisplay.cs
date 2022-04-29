using Game.Controller;
using Game.Graphics;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using OVector3 = OpenTK.Mathematics.Vector3;

namespace Game.Renderer
{
    internal class GameDisplay : IDisposable, IGameDisplay
    {
        Entity box;
        IGameModel gameModel = Ioc.Default.GetService<IGameModel>();
        IRenderer renderer = Ioc.Default.GetService<IRenderer>();
        IGameLogic logic = Ioc.Default.GetService<IGameLogic>();
        
        Player player;
        Thread updateThread;

        private bool Running { get; set; }

        public GameDisplay()
        {
            player = gameModel.Player;

            // TODO: move this to logic
            box = new Box(new Vector3(0, 0.5f, 5));
            gameModel.Entities.Add(box);

            renderer.Camera.Position = new OVector3(0.0f, 1.5f, -1.5f);
            renderer.Camera.Yaw = 90;
            renderer.Camera.Pitch = -15;

            updateThread = new Thread(UpdateLoop);
        }

        public void Start()
        {
            Running = true;
            updateThread.Start();
        }

        private void UpdateLoop()
        {
            const double updatePerSec = 120;
            const double updateStep = 1000 / updatePerSec; // milliseconds
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double accumulator = 0;
            while (Running)
            {
                stopwatch.Stop();
                double dt = stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Restart();

                accumulator += dt;

                while (accumulator > updateStep)
                {
                    logic.Update(updateStep / 1000); // millisec to sec
                    accumulator -= updateStep;
                }

                Thread.Sleep(1);
            }
        }

        public void Resize(int width, int height, int defaultFbo)
        {
            renderer.Resize(width, height, defaultFbo);
        }

        public void Render()
        {
            OVector3 startPos = new()
            {
                X = 0,
                Y = 0,
                Z = player.Position.Z - (player.Position.Z % 10)
            };

            for (int i = 0; i < renderer.PointLights.Length; i++)
            {
                renderer.PointLights[i].Position = startPos + new OVector3(0, 2, 10 * i - 10);
            }

            renderer.Camera.Position = new OVector3(player.Position.X, player.Position.Y + 0.75f, player.Position.Z - 1.5f);

            renderer.BeginFrame();

            renderer.Render(gameModel.Player);

            renderer.Render(gameModel.Entities);

            renderer.EndFrame();
        }

        public void Dispose()
        {
            Running = false;
            updateThread.Join();
            renderer.Dispose();
        }
    }
}

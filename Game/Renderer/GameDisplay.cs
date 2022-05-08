using Game.Controller;
using Game.Graphics;
using Game.Logic;
using Game.ResourceLoader;
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
        IGameModel gameModel = Ioc.Default.GetService<IGameModel>();
        IRenderer renderer = Ioc.Default.GetService<IRenderer>();
        IGameLogic logic = Ioc.Default.GetService<IGameLogic>();

        Player player;
        Thread updateThread;

        private bool Running { get; set; }

        public double TickRate { get; private set; }

        public GameDisplay()
        {
            player = gameModel.Player;

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
            const double MAX_TPS = 120;
            const double MIN_TPS = 30;
            double updatePerSec = MAX_TPS;
            double updateStep = 1000 / updatePerSec; // milliseconds
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double accumulator = 0;
            while (Running)
            {
                stopwatch.Stop();
                double dt = stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Restart();

                if (dt > 500) // prevent updating after breakpoint
                {
                    dt = updatePerSec;
                }

                accumulator += dt;

                if (accumulator > updateStep * 2)
                {
                    updatePerSec *= 0.75;
                    if (updatePerSec < MIN_TPS) updatePerSec = MIN_TPS;
                    updateStep = 1000 / updatePerSec;
                    //Debug.WriteLine($"Update is cycle {dt}ms behind, reducing tickrate to {updateStep}");
                    TickRate = updatePerSec;
                }
                else
                {
                    updatePerSec *= 1.1;
                    if (updatePerSec > MAX_TPS) updatePerSec = MAX_TPS;
                    //Debug.WriteLine($"Increasing tickrate to {updateStep}");
                    updateStep = 1000 / updatePerSec;
                    TickRate = updatePerSec;
                }

                while (accumulator > updateStep)
                {
                    logic.Update(updateStep / 1000); // millisec to sec
                    accumulator -= updateStep;
                }

                Thread.Sleep(0);
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

            renderer.Camera.Position = new OVector3(player.Position.X, 1.4f, player.Position.Z - 1.5f);

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

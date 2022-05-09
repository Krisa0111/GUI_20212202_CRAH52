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
using Random = System.Random;

namespace Game.Renderer
{
    internal class GameDisplay : IDisposable, IGameDisplay
    {
        IGameModel gameModel = Ioc.Default.GetService<IGameModel>();
        IRenderer renderer = Ioc.Default.GetService<IRenderer>();
        IGameLogic logic = Ioc.Default.GetService<IGameLogic>();

        private static Random rnd = new Random();

        Player player;
        Thread updateThread;

        private bool Running { get; set; }

        public double TickRate { get; private set; }

        public event Action<float> GameDisplayOver;

        public GameDisplay()
        {
            player = gameModel.Player;
            renderer.Camera.Position = new OVector3(0.0f, 1.5f, -1.5f);
            renderer.Camera.Yaw = 90;
            renderer.Camera.Pitch = -15;
            gameModel.EndOfGame += GameModel_EndOfGame;

            updateThread = new Thread(UpdateLoop);
        }

        private void GameModel_EndOfGame(float obj)
        {
            Running = false;
            GameDisplayOver.Invoke(obj);
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

        public void Render(double delta)
        {
            int i = 0;
            foreach (var item in gameModel.Entities)
            {
                if (item.Position.Z < player.Position.Z - 20) continue;
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

            float distance = player.Position.Z - 1 - renderer.Camera.Position.Z;

            renderer.Camera.Position = new OVector3(player.Position.X, 1.4f, renderer.Camera.Position.Z + (float)(distance * delta * 7));

            renderer.BeginFrame();

            renderer.Render(gameModel.Player);

            renderer.Render(gameModel.Entities);

            renderer.EndFrame();
        }

        public void Dispose()
        {
            if (Running)
            {
                Running = false;
                updateThread.Join();
            }

            renderer.Dispose();
        }
    }
}

using Game.Controller;
using Game.ResourceLoader;
using Game.ViewModel;
using Game.ViewModel.Entities;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game.Logic
{
    class MainMenuLogic
    {
        IMainMenuModel model = Ioc.Default.GetService<IMainMenuModel>();

        private const int CHUNK_SIZE = 40;
        private const int CHUNK_GEN_DISTANCE = 80;
        private int chunkPos;

        public MainMenuLogic()
        {
            Init();
        }

        private void Init()
        {
            chunkPos = -20;

            GenEntities();

            model.Player.Speed = 4;
        }

        public void Reset()
        {
            model.Reset();
            Init();
        }

        private void GenBuildings(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Building building1 = new Building(new Vector3(6, .1f, chunkPos + (CHUNK_SIZE / count) * i));
                Building building2 = new Building(new Vector3(-6, .1f, chunkPos + (CHUNK_SIZE / count) * i));
                building1.RotationY = -MathF.PI / 2;
                building2.RotationY = MathF.PI / 2;
                model.Entities.Add(building1);
                model.Entities.Add(building2);
            }
        }

        private void GenStreetlights(int count)
        {
            for (int i = 0; i < count; i++)
            {
                StreetLight streetLight1 = new StreetLight(new Vector3(1.6f, 4, chunkPos + (CHUNK_SIZE / count) * i));
                StreetLight streetLight2 = new StreetLight(new Vector3(-1.6f, 4, chunkPos + (CHUNK_SIZE / count) * i));
                streetLight2.RotationY = MathF.PI;
                model.Entities.Add(streetLight1);
                model.Entities.Add(streetLight2);
            }
        }

        private void GenEntities()
        {
            model.Entities.Add(new Road(new Vector3(0, 0, chunkPos)));
            GenBuildings(5);
            GenStreetlights(3);
            chunkPos += CHUNK_SIZE;
        }

        private void RemoveEntities()
        {
            int i = 0;
            while (i < model.Entities.Count)
            {
                Entity entity = model.Entities[i];
                if (entity.Position.Z + CHUNK_GEN_DISTANCE < model.Player.Position.Z) entity.MarkToDelete();

                if (entity.ShouldDelete)
                {
                    model.Entities.Remove(entity);
                }
                else
                {
                    i++;
                }
            }
        }

        public void Update(double delta)
        {
            // update entities
            model.Player.Position += model.Player.Velocity * (float)delta;
            model.Player.CurrentAnimatonStep += model.Player.Speed * (float)delta * 10f;

            if (model.Entities.Count > 0)
            {
                // add entities
                if (model.Entities.Last().Position.Z < model.Player.Position.Z + CHUNK_GEN_DISTANCE)
                {
                    GenEntities();
                }

                // delete entities
                RemoveEntities();
            }
        }
    }
}

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
    class GameLogic : IGameLogic
    {
        PlayerLogic playerLogic;
        IGameModel gameModel = Ioc.Default.GetService<IGameModel>();
        ChunkLoader chunkLoader;

        private const int CHUNK_SIZE = 40;
        private const int CHUNK_GEN_DISTANCE = 80;
        private int chunkPos = -60;
        
        public GameLogic()
        {
            playerLogic = new PlayerLogic();
            chunkLoader = new ChunkLoader("Maps");

            gameModel.Entities.Enqueue(new Road(new Vector3(0, 0, chunkPos + 20)));
            GenBuildings(5);
            GenStreetlights(3);
            chunkPos += CHUNK_SIZE;
            gameModel.Entities.Enqueue(new Road(new Vector3(0, 0, chunkPos + 20)));
            GenBuildings(5);
            GenStreetlights(3);
            chunkPos += CHUNK_SIZE;

            GenEntities();
        }

        private void GenBuildings(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Building building1 = new Building(new Vector3(6, .1f, chunkPos + (CHUNK_SIZE / count) * i));
                Building building2 = new Building(new Vector3(-6, .1f, chunkPos + (CHUNK_SIZE / count) * i));
                building1.RotationY = -MathF.PI / 2;
                building2.RotationY = MathF.PI / 2;
                gameModel.Entities.Enqueue(building1);
                gameModel.Entities.Enqueue(building2);
            }
        }

        private void GenStreetlights(int count)
        {
            for (int i = 0; i < count; i++)
            {
                StreetLight streetLight1 = new StreetLight(new Vector3(1.6f, 4, chunkPos + (CHUNK_SIZE / count) * i));
                StreetLight streetLight2 = new StreetLight(new Vector3(-1.6f, 4, chunkPos + (CHUNK_SIZE / count) * i));
                streetLight2.RotationY = MathF.PI;
                gameModel.Entities.Enqueue(streetLight1);
                gameModel.Entities.Enqueue(streetLight2);
            }
        }
        private void GenEntities()
        {
            var entities = chunkLoader.GetRandomChunk(new Vector3(0, 0, chunkPos));

            GenBuildings(5);
            GenStreetlights(3);

            for (int i = 0; i < entities.Count; i++)
            {
                gameModel.Entities.Enqueue(entities[i]);
            }

            chunkPos += CHUNK_SIZE;
        }

        private void RemoveEntities()
        {
            foreach (var entity in gameModel.Entities)
            {
                if (entity.Position.Z + CHUNK_GEN_DISTANCE < gameModel.Player.Position.Z) entity.MarkToDelete();
            }

            while (
                gameModel.Entities.TryPeek(out var entity) &&
                entity.ShouldDelete &&
                gameModel.Entities.TryDequeue(out var removedEntity)
                )
            {
                Debug.WriteLine(removedEntity.Position + " " + gameModel.Entities.Count);
            }
        }

        public void Move(Directions direction)
        {
            playerLogic.Move(direction);
        }

        public void Update(double delta)
        {
            // update entities
            playerLogic.Update(delta);
            foreach (var entity in gameModel.Entities)
            {
                switch (entity.Type)
                {
                    case EntityType.Decelerator:
                    case EntityType.Accelerator:
                    case EntityType.Skull:
                    case EntityType.PlusLife:
                    case EntityType.Random:
                        entity.RotationY += (float)delta;
                        break;
                }
            }

            // add entities
            if (gameModel.Entities.Last().Position.Z < gameModel.Player.Position.Z + CHUNK_GEN_DISTANCE)
            {
                GenEntities();
            }

            // delete entities
            RemoveEntities();
        }
    }
}

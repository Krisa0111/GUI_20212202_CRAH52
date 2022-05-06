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
        private const int CHUNK_GEN_DISTANCE = 100;
        private int lastChunkPos = 0;

        public GameLogic()
        {
            playerLogic = new PlayerLogic();
            chunkLoader = new ChunkLoader("maps");

            gameModel.Entities.Enqueue(new Road(new Vector3(0, 0, 20)));
            GenEntities();
        }

        private void GenEntities()
        {
            lastChunkPos += CHUNK_SIZE;
            var entities = chunkLoader.GetRandomChunk(new Vector3(0, 0, lastChunkPos));

            for (int i = 0; i < entities.Count; i++)
            {
                gameModel.Entities.Enqueue(entities[i]);
            }
        }

        private void RemoveEntities()
        {
            foreach (var entity in gameModel.Entities)
            {
                if (entity.Position.Z + CHUNK_SIZE < gameModel.Player.Position.Z) entity.MarkToDelete();
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
            playerLogic.Update(delta);
            foreach (var entity in gameModel.Entities)
            {
                if (entity.Type == EntityType.Decelerator) entity.RotationY += (float)delta;
            }

            // add entities
            while (gameModel.Entities.Last().Position.Z < gameModel.Player.Position.Z + CHUNK_GEN_DISTANCE)
            {
                GenEntities();
            }

            // delete entities
            RemoveEntities();
        }
    }
}

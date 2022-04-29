using Game.Controller;
using Game.ViewModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
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

        public GameLogic()
        {
            playerLogic = new PlayerLogic();
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
        }
    }
}

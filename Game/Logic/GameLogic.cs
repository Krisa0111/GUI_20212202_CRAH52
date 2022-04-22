using Game.Controller;
using Game.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game.Logic
{
    class GameLogic : IGameController, IGameLogic
    {
        IGameModel gameModel;
        PlayerLogic playerLogic;
        

        Size area;
        public void SetupSizes(Size area)
        {
            this.area = area;

        }
        public GameLogic()
        {
            playerLogic = new PlayerLogic();
        }

        public void Move(Directions direction)
        {
            throw new NotImplementedException();
        }
        public void Update(double delta)
        {
            playerLogic.Move(delta);
        }
    }
}

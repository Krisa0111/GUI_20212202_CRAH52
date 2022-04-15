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
    class GameLogic : IGameController
    {
        public int Life { get; set; }
        public int Score { get; set; }
        
        public GameLogic()
        {
            this.Life = 3;
            this.Score = 0;
        }
        Size area;
        public void SetupSizes(Size area)
        {
            this.area = area;

        }
        

        public void Move(Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}

﻿using Game.Controller;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    class GameLogic : IGameController, IGameModell
    {
        public int Life { get; set; }
        public int Score { get; set; }
        

        public void Move(Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}

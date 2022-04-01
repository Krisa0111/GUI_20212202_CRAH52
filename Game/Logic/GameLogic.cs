using Game.Controller;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    public class GameLogic : IGameController, IGameModell
    {
        public int Score { get; set; }
        public int Life { get; set; }
        public bool ContinueGame { get; set; }
        public GameLogic(int score, int life, bool continueGame)
        {
            this.Life = score;
            this.Score = life;
            this.ContinueGame = continueGame;
        }
        
    }
}

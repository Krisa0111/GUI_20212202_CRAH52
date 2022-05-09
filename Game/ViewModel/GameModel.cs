using Game.ViewModel.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel
{
    
    public class GameModel : IGameModel
    {
        public Player Player { get;set; }
        public event Action<float> EndOfGame;

        public ConcurrentQueue<Entity> Entities
        {
            get; set;
        }

        public Map mapTunnel { get; set; }
        

        public GameModel()
        {
            Entities = new ConcurrentQueue<Entity>();
            mapTunnel = new MapTunnel();
            Player = new Player(new Vector3(0,0.7f,0));
        }

        

        public void GameOver(float highscore)
        {
            EndOfGame.Invoke(highscore);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel
{
    public class GameModel : IGameModel
    {
        
        public IList<Entity> Entities
        {
            get; set;
        }

        public Map mapTunnel { get; set; }

        public GameModel()
        {
            Entities = new List<Entity>();
            mapTunnel = new MapTunnel();
        }
    }
}

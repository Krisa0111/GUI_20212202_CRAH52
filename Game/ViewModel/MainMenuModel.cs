using Game.ViewModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel
{
    internal class MainMenuModel : IMainMenuModel
    {
        public Player Player { get; set; }

        public List<Entity> Entities
        {
            get; set;
        }

        public MainMenuModel()
        {
            Entities = new List<Entity>();
            Player = new Player(new Vector3(0, 0.7f, 0));
        }

        public void Reset()
        {
            Entities.Clear();
            Player = new Player(new Vector3(0, 0.7f, 0));

        }
    }
}

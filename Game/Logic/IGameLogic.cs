using Game.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Logic
{
    interface IGameLogic
    {
        public void Update(double delta);
        public void Move(Directions direction);
        public void Reset();
    }
}

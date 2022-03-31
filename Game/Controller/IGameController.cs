using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Controller
{
    interface IGameController
    {
        void Move(Directions direction);
    }
}

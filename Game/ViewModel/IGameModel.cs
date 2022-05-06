using Game.ViewModel.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel
{
    public interface IGameModel
    {
        ConcurrentQueue<Entity> Entities { get; set; }
        Player Player { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel
{
    public interface IGameModel
    {
        IList<Entity> Entities { get; set; }
    }
}

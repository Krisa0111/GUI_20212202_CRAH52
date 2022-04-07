using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public interface IGameModell
    {
        event EventHandler Changed; // ide kene feliratkoztatni a renderert
        Player Player { get; set; }
        List<GameItem> GameItems { get; set; }

    }
}

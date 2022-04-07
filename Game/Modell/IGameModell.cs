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
        List<Decelerator> Decelerators { get; set; }
        List<Accelerator> Accelerators { get; set; }
        List<BluePortal> BluePortals { get; set; }
        List<RedPortal> RedPortals { get; set; }
        List<PlusLife> PlusLifes { get; set; }
        List<PremiumPortal> PremiumPortals { get; set; }
        List<RandomItem> RandomItems { get; set; }
        List<Skull> Skulls { get; set; }
        List<Box> Boxes { get; set; }

    }
}

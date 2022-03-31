using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Items
{
    abstract class GameItem
    {
        public double X { get; set; }
        public double Y { get; set; }
        public GameItemType Type { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Game.Model
{
    public abstract class GameItem
    {
        public abstract Geometry Area { get; }
        
        public GameItemType Type { get; set; }
        public Vector Speed { get; set; }
        public Vector Position { get; set; }
    }
}

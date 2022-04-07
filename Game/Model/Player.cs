using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game.Model
{
    public class Player : GameItem
    {
        public override Geometry Area => throw new NotImplementedException();
        public int Life { get; set; }
        public int MaxLife { get; set; }
    }
}

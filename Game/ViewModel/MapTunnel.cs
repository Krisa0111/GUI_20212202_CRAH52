using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel
{
    public class MapTunnel : Map
    {
        public MapTunnel() 
            : base(ModelLoader.GetModel("Models/tunnel.obj"), 6)
        { }
    }
}

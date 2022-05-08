using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class RedPortal : Entity
    {
        public RedPortal(Vector3 position)
           : base(EntityType.RedPortal, position, ModelLoader.GetModel("Models/red_portal.obj"))
        { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class BluePortal : Entity
    {
        public BluePortal(Vector3 position)
            : base(EntityType.BluePortal, position, ModelLoader.GetModel("Models/blue_portal.obj"))
        { }
    }
}

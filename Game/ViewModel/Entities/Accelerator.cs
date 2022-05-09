using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class Accelerator : Entity
    {
        public Accelerator(Vector3 position)
            : base(EntityType.PlusLife, position, ModelLoader.GetModel("Models/accelerator.obj"), ModelLoader.GetModel("Models/powerup_collider.obj"))
        {

        }
    }
}

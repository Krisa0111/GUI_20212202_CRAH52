using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class Skull : Entity
    {
        public Skull(Vector3 position)
            : base(EntityType.Skull, position, ModelLoader.GetModel("Models/skull.obj"), ModelLoader.GetModel("Models/powerup_collider.obj"))
        {

        }
    }
}

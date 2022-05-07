using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class Heart : Entity
    {
        public Heart(Vector3 position)
            : base(EntityType.PlusLife, position, ModelLoader.GetModel("Models/heart.obj"), ModelLoader.GetModel("Models/powerup_collider.obj"))
        {

        }
    }
}

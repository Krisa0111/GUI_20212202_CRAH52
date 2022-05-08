using Game.ResourceLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class Random : Entity
    {
        public Random(Vector3 position)
            : base(EntityType.Random, position, ModelLoader.GetModel("Models/random.obj"), ModelLoader.GetModel("Models/powerup_collider.obj"))
        {

        }
    }
}

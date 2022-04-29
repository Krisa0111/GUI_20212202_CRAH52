using Game.ResourceLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class Watch : Entity
    {
        public Watch(Vector3 position) 
            : base(EntityType.Decelerator, position, ModelLoader.GetModel("Models/stopwatch.obj"))
        {

        }

        public override Model ColliderModel => ModelLoader.GetModel("Models/powerup_collider.obj");
    }
}

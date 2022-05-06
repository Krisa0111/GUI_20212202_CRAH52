using Game.ResourceLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class Road : Entity
    {
        public Road(Vector3 position)
            : base(EntityType.Obstacle, position, ModelLoader.GetModel("Models/road.obj"))
        {

        }
    
    }
}

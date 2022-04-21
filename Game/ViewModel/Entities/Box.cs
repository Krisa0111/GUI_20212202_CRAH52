using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game.ViewModel.Entities
{
    internal class Box : Entity
    {
        public Box(Vector3 position)
            : base(EntityType.Obstacle, position, ModelLoader.GetModel("Models/untitled.obj"))
        { }
    }
}

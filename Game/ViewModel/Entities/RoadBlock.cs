using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class RoadBlock : Entity
    {
        public RoadBlock(Vector3 position) : base(EntityType.Obstacle, position)
        {
            var files = Directory.GetFiles("Models/RoadBlocks", "*.obj");
            Model = ModelLoader.GetModel(files[Rnd.Next(files.Length)]);
        }
    }
}

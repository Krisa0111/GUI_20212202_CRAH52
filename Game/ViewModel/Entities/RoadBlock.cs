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
        static string[] files = Directory.GetFiles("Models/RoadBlocks", "*.obj");

        public RoadBlock(Vector3 position) : base(EntityType.Obstacle, position, ModelLoader.GetModel(files[Rnd.Next(files.Length)]))
        {
        }
    }
}

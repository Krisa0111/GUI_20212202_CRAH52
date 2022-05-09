using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Game.ResourceLoader;

namespace Game.ViewModel.Entities
{
    internal class StreetLight : Entity
    {
        public Vector3 LightSourceOffset { get => new Vector3(0.4f, 4.4f, 0); }

        public StreetLight(Vector3 position) : base(EntityType.Obstacle, position, ModelLoader.GetModel("Models/streetlight.obj"), Model.Empty)
        {

        }
    }
}

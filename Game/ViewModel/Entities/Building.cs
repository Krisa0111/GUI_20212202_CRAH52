using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Game.ResourceLoader;

namespace Game.ViewModel.Entities
{
    internal class Building : Entity
    {
        private static Model[] models = new Model[]
            {
                ModelLoader.GetModel("Models/buildingA.obj"),
                ModelLoader.GetModel("Models/buildingB.obj"),
                ModelLoader.GetModel("Models/buildingC.obj"),
            };

        public Building(Vector3 position)
            : base(EntityType.Other, position, models[Rnd.Next(models.Length)], Model.Empty)
        {

        }
    }
}

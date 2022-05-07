using Game.ResourceLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class Truck : Entity
    {
        private static (Model model, Model collider)[] models = new (Model model, Model collider)[]
            {
                (ModelLoader.GetModel("Models/Vehicles/truck/truck_green.obj"), ModelLoader.GetModel("Models/Vehicles/truck/truck_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/truck/truck_red.obj"), ModelLoader.GetModel("Models/Vehicles/truck/truck_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/truck/truck_white.obj"), ModelLoader.GetModel("Models/Vehicles/truck/truck_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/truck/truck_yellow.obj"), ModelLoader.GetModel("Models/Vehicles/truck/truck_collider.obj")),
            };

        public Truck(Vector3 position) : base(EntityType.Obstacle, position, models[Rnd.Next(models.Length)])
        {

        }
    }
}

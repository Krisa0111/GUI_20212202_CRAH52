using Game.ResourceLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class BigCar : Entity
    {
        private static (Model model, Model collider)[] models = new (Model model, Model collider)[]
            {
                (ModelLoader.GetModel("Models/Vehicles/minivan/minivan_blue.obj"), ModelLoader.GetModel("Models/Vehicles/minivan/minivan_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/minivan/minivan_green.obj"), ModelLoader.GetModel("Models/Vehicles/minivan/minivan_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/minivan/minivan_red.obj"), ModelLoader.GetModel("Models/Vehicles/minivan/minivan_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/minivan/minivan_white.obj"), ModelLoader.GetModel("Models/Vehicles/minivan/minivan_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/minivan/minivan_yellow.obj"), ModelLoader.GetModel("Models/Vehicles/minivan/minivan_collider.obj")),

                (ModelLoader.GetModel("Models/Vehicles/suv/suv_blue.obj"), ModelLoader.GetModel("Models/Vehicles/suv/suv_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/suv/suv_green.obj"), ModelLoader.GetModel("Models/Vehicles/suv/suv_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/suv/suv_red.obj"), ModelLoader.GetModel("Models/Vehicles/suv/suv_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/suv/suv_white.obj"), ModelLoader.GetModel("Models/Vehicles/suv/suv_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/suv/suv_yellow.obj"), ModelLoader.GetModel("Models/Vehicles/suv/suv_collider.obj")),
            };

        public BigCar(Vector3 position) : base(EntityType.Obstacle, position, models[Rnd.Next(models.Length)])
        {

        }
    }
}

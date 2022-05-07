using Game.ResourceLoader;
using System.IO;
using System.Numerics;

namespace Game.ViewModel.Entities
{
    internal class Car : Entity
    {
        private static (Model model, Model collider)[] models = new (Model model, Model collider)[]
            {
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_01_blue.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_01_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_01_green.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_01_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_01_red.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_01_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_01_white.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_01_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_01_yellow.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_01_collider.obj")),

                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_02_blue.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_02_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_02_green.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_02_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_02_red.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_02_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_02_white.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_02_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_02_yellow.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_02_collider.obj")),

                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_03_blue.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_03_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_03_green.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_03_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_03_red.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_03_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_03_white.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_03_collider.obj")),
                (ModelLoader.GetModel("Models/Vehicles/sedan/sedan_03_yellow.obj"), ModelLoader.GetModel("Models/Vehicles/sedan/sedan_03_collider.obj")),

            };

        public Car(Vector3 position) : base(EntityType.Obstacle, position, models[Rnd.Next(models.Length)])
        {

        }
    }
}

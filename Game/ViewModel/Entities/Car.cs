using Game.ResourceLoader;
using System.IO;
using System.Numerics;

namespace Game.ViewModel.Entities
{
    internal class Car : Entity
    {
        public Car(Vector3 position) : base(EntityType.Obstacle, position)
        {
            var files = Directory.GetFiles("Models/Vehicles/Sedans", "*.obj");
            Model = ModelLoader.GetModel(files[Rnd.Next(files.Length)]);
        }
    }
}

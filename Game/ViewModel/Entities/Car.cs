using Game.ResourceLoader;
using System.IO;
using System.Numerics;

namespace Game.ViewModel.Entities
{
    internal class Car : Entity
    {
        private static readonly string[] files = Directory.GetFiles("Models/Vehicles/Sedans", "*.obj");

        public Car(Vector3 position) : base(EntityType.Obstacle, position, ModelLoader.GetModel(files[Rnd.Next(files.Length)]))
        {

        }
    }
}

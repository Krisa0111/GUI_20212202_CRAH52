using Game.Graphics;
using Game.Logic;
using Game.ResourceLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Game.ViewModel
{
    public abstract class Entity
    {
        public EntityType Type { get; }
        public Vector3 Position { get; set; }
        public float RotationY { get; set; }

        public virtual Model Model { get; protected set; }
        // low poly model for collison detection
        public virtual Model ColliderModel { get => Model; }

        public static ModelLoader ModelLoader { get; }

        protected static Random Rnd;

        static Entity()
        {
            ModelLoader = ModelLoader.GetInstance();
            Rnd = new Random();
        }

        protected Entity(EntityType type, Vector3 position, Model model)
        {
            Type = type;
            Position = position;
            Model = model;
        }

        protected Entity(EntityType type, Vector3 position)
        {
            Type = type;
            Position = position;
        }
    }
}

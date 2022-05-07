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

        public bool ShouldDelete { get; private set; } = false;

        public virtual Model Model { get; private set; }
        // low poly model for collison detection
        public Model ColliderModel { get; private set; }

        public static ModelLoader ModelLoader { get; }

        protected static Random Rnd;

        static Entity()
        {
            ModelLoader = ModelLoader.GetInstance();
            Rnd = new Random();
        }
        protected Entity(EntityType type, Vector3 position, (Model model, Model collider) tuple)
        {
            Type = type;
            Position = position;
            Model = tuple.model;
            ColliderModel = tuple.collider;
        }

        protected Entity(EntityType type, Vector3 position, Model model, Model collider = null)
        {
            Type = type;
            Position = position;
            Model = model;
            ColliderModel = collider ?? model;
        }

        protected Entity(EntityType type, Vector3 position)
        {
            Type = type;
            Position = position;
            Model = Model.Empty;
            ColliderModel = Model.Empty;
        }

        public void MarkToDelete()
        {
            ShouldDelete = true;
            Model = Model.Empty;
            ColliderModel = Model.Empty;
        }
    }
}

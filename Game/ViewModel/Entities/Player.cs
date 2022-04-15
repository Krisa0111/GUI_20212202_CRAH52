using Game.ResourceLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    internal class Player : Entity
    {
        public const int AnimationSteps = 30;
        private float currentAnimatonStep;
        public float CurrentAnimatonStep
        {
            get => currentAnimatonStep; set => currentAnimatonStep = value % AnimationSteps;
        }
        private readonly Model[] models;

        public override Model Model
        {
            get
            {
                return models[(int)CurrentAnimatonStep];
            }
        }

        public const int MaxLife = 3;
        public int Life { get; set; }

        public Player(Vector3 position) : base(EntityType.Player, position)
        {
            models = new Model[AnimationSteps];
            for (int i = 0; i < models.Length; i++)
            {
                models[i] = ModelLoader.GetModel("Models/human" + i + ".obj");
            }
        }

    }
}

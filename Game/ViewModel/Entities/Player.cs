﻿using Game.ResourceLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel.Entities
{
    public class Player : Entity
    {
        public const int AnimationSteps = 30;
        public Vector3 velocity;
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
        
        [Range(0,5)]
        public int Life { get; set; }

        public Player(Vector3 position) : base(EntityType.Player, position)
        {
            velocity = new Vector3(0, 0, 2);
            models = new Model[AnimationSteps];
            for (int i = 0; i < models.Length; i++)
            {
                models[i] = ModelLoader.GetModel("Models/human" + i + ".obj");
            }
        }

    }
}

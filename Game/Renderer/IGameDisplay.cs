﻿using System;

namespace Game.Renderer
{
    internal interface IGameDisplay
    {
        void Stop();
        void Render(double delta);
        void Resize(int width, int height, int defaultFbo);
        void Start();
        event Action<float> GameDisplayOver;
        public double TickRate { get; }
    }
}
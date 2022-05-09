namespace Game.Renderer
{
    internal interface IGameDisplay
    {
        void Dispose();
        void Render(double delta);
        void Resize(int width, int height, int defaultFbo);
        void Start();

        public double TickRate { get; }
    }
}
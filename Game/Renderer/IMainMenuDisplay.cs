namespace Game
{
    public interface IMainMenuDisplay
    {
        void Render(double delta);
        void Reset();
        void Resize(int width, int height, int defaultFbo);
    }
}
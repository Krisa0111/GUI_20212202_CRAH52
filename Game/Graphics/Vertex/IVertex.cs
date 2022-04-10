namespace Game.Graphics
{
    internal interface IVertex
    {
        int Size { get; }
        int Stride { get; }
        void SetAttributes(VertexArray vao);
        float[] GetData();
    }
}
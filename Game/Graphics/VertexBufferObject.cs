using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class VertexBufferObject
    {
        private int id;

        public VertexBufferObject(IReadOnlyList<Vertex> vertices)
        {
            id = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, id);

            float[] data = new float[vertices.Count * Vertex.Size];
            for (int i = 0; i < vertices.Count; i++)
            {
                float[] vertex = vertices[i].GetData();
                for (int j = 0; j < vertex.Length; j++)
                {
                    data[i * Vertex.Size + j] = vertex[j];
                }
            }

            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, id);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Delete()
        {
            GL.DeleteBuffer(id);
        }

    }
}

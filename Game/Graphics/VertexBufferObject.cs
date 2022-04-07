using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class VertexBufferObject<T> where T : IVertex
    {
        private readonly int id = 0;
        private int bufferSize;
        public int VertexSize { get; }

        public VertexBufferObject(IList<T> vertices, int vertexSize, BufferUsageHint bufferUsageHint)
        {
            id = GL.GenBuffer();
            this.VertexSize = vertexSize;
            BufferData(vertices, bufferUsageHint);
        }

        public void BufferData(IList<T> vertices, BufferUsageHint bufferUsageHint)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, id);

            float[] data = new float[vertices.Count * VertexSize];
            for (int i = 0; i < vertices.Count; i++)
            {
                float[] vertex = vertices[i].GetData();
                for (int j = 0; j < vertex.Length; j++)
                {
                    data[i * VertexSize + j] = vertex[j];
                }
            }

            if (data.Length * sizeof(float) > bufferSize)
            {
                GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, bufferUsageHint);
                bufferSize = data.Length * sizeof(float);
            }
            else
            {
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, data.Length * sizeof(float), data);
            }
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

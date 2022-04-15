using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics.OpenGL
{
    internal class VertexBuffer : IDisposable
    {
        private readonly int id = 0;
        private int bufferSize;

        public VertexBuffer(IList<float> data, BufferUsageHint bufferUsageHint)
        {
            id = GL.GenBuffer();
            BufferData(data, bufferUsageHint);
        }

        public void BufferData(IList<float> data, BufferUsageHint bufferUsageHint)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, id);

            float[] buffer = data.ToArray();
            int newSize = buffer.Length * sizeof(float);

            if (newSize > bufferSize)
            {
                GL.BufferData(BufferTarget.ArrayBuffer, newSize, buffer, bufferUsageHint);
                bufferSize = newSize;
            }
            else
            {
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, newSize, buffer);
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

        public void Dispose()
        {
            GL.DeleteBuffer(id);
        }
    }
}

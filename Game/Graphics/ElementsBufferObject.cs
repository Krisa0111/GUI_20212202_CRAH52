using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class ElementsBufferObject
    {
        private readonly int id = 0;
        private int bufferSize;

        public ElementsBufferObject(IList<uint> indices, BufferUsageHint bufferUsageHint)
        {
            id = GL.GenBuffer();
            BufferData(indices, bufferUsageHint);
        }

        public void BufferData(IList<uint> indices, BufferUsageHint bufferUsageHint)
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);

            uint[] data = indices.ToArray();

            if (data.Length * sizeof(float) > bufferSize)
            {
                GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(uint), data, bufferUsageHint);
                bufferSize = data.Length * sizeof(uint);
            }
            else
            {
                GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, data.Length * sizeof(uint), data);
            }
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Delete()
        {
            GL.DeleteBuffer(id);
        }
    }
}

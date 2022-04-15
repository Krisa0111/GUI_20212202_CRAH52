using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics.OpenGL
{
    internal class VertexArray : IDisposable
    {
        private int id;
        public int VertexSize { get; private set; }

        public VertexArray()
        {
            id = GL.GenVertexArray();
        }

        public void LinkBuffer(VertexBuffer vertexBuffer, VertexBufferLayout layout)
        {
            Bind();
            vertexBuffer.Bind();
            VertexSize = layout.Elements.Count;

            int offset = 0;
            for (int i = 0; i < layout.Elements.Count; i++)
            {
                var element = layout.Elements[i];

                GL.VertexAttribPointer(
                    element.Layout,
                    element.Count,
                    element.Type,
                    element.Normalized,
                    layout.Size,
                    offset
                );
                GL.EnableVertexAttribArray(element.Layout);

                offset += element.Count * element.TypeSize;
            }

        }

        public void Bind()
        {
            GL.BindVertexArray(id);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            GL.DeleteVertexArray(id);
        }
    }
}

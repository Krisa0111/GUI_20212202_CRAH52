using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class VertexArray
    {
        private int id;

        public VertexArray()
        {
            id = GL.GenVertexArray();
            ;
        }

        public void LinkAttrib(int layout, int numComponents, VertexAttribPointerType type, int stride, int offset)
        {
            GL.VertexAttribPointer(layout, numComponents, type, false, stride, offset);
            GL.EnableVertexAttribArray(layout);
        }

        public void Bind()
        {
            GL.BindVertexArray(id);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Delete()
        {
            GL.DeleteVertexArray(id);
        }
    }
}

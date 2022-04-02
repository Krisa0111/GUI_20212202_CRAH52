using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class Mesh
    {
        private VertexBufferObject vbo;
        private VertexArrayObject vao;
        private int vertexCount;
        public Mesh(IReadOnlyList<Vertex> vertices)
        {
            vertexCount = vertices.Count;

            vbo = new VertexBufferObject(vertices);

            vao = new VertexArrayObject();

            vao.Bind();

            // position
            vao.LinkAttrib(vbo, 0, 3, VertexAttribPointerType.Float, Vertex.Stride, 0);
            // normal
            vao.LinkAttrib(vbo, 1, 3, VertexAttribPointerType.Float, Vertex.Stride, 3);
            // color
            vao.LinkAttrib(vbo, 2, 3, VertexAttribPointerType.Float, Vertex.Stride, 6);
            // texture uv
            vao.LinkAttrib(vbo, 3, 2, VertexAttribPointerType.Float, Vertex.Stride, 9);

            vao.Unbind();

        }

        public void Draw()
        {
            vao.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
        }

    }
}

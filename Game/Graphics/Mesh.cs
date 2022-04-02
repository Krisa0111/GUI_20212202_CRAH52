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
            vbo.Bind();

            // position
            vao.LinkAttrib(0, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.PositionOffset);
            // normal
            vao.LinkAttrib(1, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.NormalOffset);
            // color
            vao.LinkAttrib(2, 3, VertexAttribPointerType.Float, Vertex.Stride, Vertex.ColorOffset);
            // texture uv
            vao.LinkAttrib(3, 2, VertexAttribPointerType.Float, Vertex.Stride, Vertex.TextureUVOffset);

            vbo.Unbind();
            vao.Unbind();

        }

        public void Draw()
        {
            vao.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
        }

    }
}

using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{

    internal class StaticMesh<T> where T : IVertex
    {
        protected VertexBufferObject<T> vbo;
        protected VertexArrayObject vao;
        protected int vertexCount;

        public StaticMesh(IList<T> vertices)
        {
            vertexCount = vertices.Count;
            
            vbo = new VertexBufferObject<T>(vertices, vertices[0].Size, BufferUsageHint.StaticDraw);

            vao = new VertexArrayObject();

            vao.Bind();
            vbo.Bind();
            vertices[0].SetAttributes(vao);
            vbo.Unbind();

        }

        public void Draw()
        {
            vao.Bind();
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
        }

    }
}

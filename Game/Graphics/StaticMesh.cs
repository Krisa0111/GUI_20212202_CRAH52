using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
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
        protected ElementsBufferObject ebo;
        protected VertexArrayObject vao;
        protected int vertexCount;
        protected int indexCount;

        private Texture texture;

        public StaticMesh(IList<T> vertices)
        {
            vertexCount = vertices.Count;
            
            vbo = new VertexBufferObject<T>(vertices, vertices[0].Size, BufferUsageHint.StaticDraw);

            vao = new VertexArrayObject();

            vao.Bind();
            vbo.Bind();

            vertices[0].SetAttributes(vao);

            vbo.Unbind();
            vao.Unbind();
        }

        public StaticMesh(IList<T> vertices, IList<uint> indices) : this(vertices)
        {
            vertexCount = vertices.Count;
            indexCount = indices.Count;

            vbo = new VertexBufferObject<T>(vertices, vertices[0].Size, BufferUsageHint.StaticDraw);

            vao = new VertexArrayObject();

            vao.Bind();
            vbo.Bind();

            vertices[0].SetAttributes(vao);

            ebo = new ElementsBufferObject(indices, BufferUsageHint.StaticDraw);
            ebo.Bind();

            vbo.Unbind();
            vao.Unbind();
        }

        public void AttachTexture(Texture texture)
        {
            this.texture = texture;
        }

        public void Draw(Shader shader, Matrix4 model)
        {
            shader.Use();
            shader.SetMatrix4("model", model);

            texture?.Use(TextureUnit.Texture0);

            vao.Bind();

            if (ebo is null)
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
            else
                GL.DrawElements(BeginMode.Triangles, indexCount, DrawElementsType.UnsignedInt, 0);
        }

    }
}

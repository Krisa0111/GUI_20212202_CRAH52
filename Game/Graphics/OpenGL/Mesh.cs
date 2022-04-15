using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics.OpenGL
{

    internal class Mesh : IDisposable
    {
        protected VertexBuffer vbo;
        protected ElementsBuffer ebo;
        protected VertexArray vao;
        protected int vertexCount;
        protected int indexCount;

        public Material Material { get; set; }

        public Mesh(IList<float> vertices, VertexBufferLayout vertexBufferLayout)
        {
            vertexCount = vertices.Count / vertexBufferLayout.Count;

            vbo = new VertexBuffer(vertices, BufferUsageHint.StaticDraw);

            vao = new VertexArray();

            vao.LinkBuffer(vbo, vertexBufferLayout);

            vbo.Unbind();
            vao.Unbind();
        }

        public Mesh(IList<float> vertices, IList<uint> indices, VertexBufferLayout vertexBufferLayout)
        {
            vertexCount = vertices.Count / vertexBufferLayout.Count;
            indexCount = indices.Count;

            vbo = new VertexBuffer(vertices, BufferUsageHint.StaticDraw);

            vao = new VertexArray();

            vao.LinkBuffer(vbo, vertexBufferLayout);

            vao.Bind();
            ebo = new ElementsBuffer(indices, BufferUsageHint.StaticDraw);
            ebo.Bind();

            vbo.Unbind();
            vao.Unbind();
        }

        public Mesh(IList<float> vertices, IList<uint> indices, VertexBufferLayout vertexBufferLayout, Material material)
            : this(vertices, indices, vertexBufferLayout)
        {
            Material = material;
        }

        public Mesh(IList<float> vertices, VertexBufferLayout vertexBufferLayout, Material material)
            : this(vertices, vertexBufferLayout)
        {
            Material = material;
        }

        public void Draw(Shader shader, Matrix4 model)
        {
            shader.Use();
            shader.SetMatrix4("model", model);

            Material?.Use();

            vao.Bind();

            if (ebo is null)
                GL.DrawArrays(PrimitiveType.Triangles, 0, vertexCount);
            else
                GL.DrawElements(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        public void DrawMultible(Shader shader, Matrix4 model, int instances, Vector3 instanceOffset)
        {
            shader.Use();
            shader.SetMatrix4("model", model);
            shader.SetVector3("instanceOffset", instanceOffset);

            Material?.Use();

            vao.Bind();

            if (ebo is null)
                GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, vertexCount, instances);
            else
                GL.DrawElementsInstanced(PrimitiveType.Triangles, indexCount, DrawElementsType.UnsignedInt, IntPtr.Zero, instances);
        }

        public void Dispose()
        {
            ebo?.Dispose();
            vbo.Dispose();
            vao.Dispose();
        }
    }
}

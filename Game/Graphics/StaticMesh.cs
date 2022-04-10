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
        protected VertexBuffer<T> vbo;
        protected ElementsBuffer ebo;
        protected VertexArray vao;
        protected int vertexCount;
        protected int indexCount;

        public Material Material { get; set; }

        public StaticMesh(IList<T> vertices)
        {
            vertexCount = vertices.Count;
            
            vbo = new VertexBuffer<T>(vertices, vertices[0].Size, BufferUsageHint.StaticDraw);

            vao = new VertexArray();

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

            vbo = new VertexBuffer<T>(vertices, vertices[0].Size, BufferUsageHint.StaticDraw);

            vao = new VertexArray();

            vao.Bind();
            vbo.Bind();

            vertices[0].SetAttributes(vao);

            ebo = new ElementsBuffer(indices, BufferUsageHint.StaticDraw);
            ebo.Bind();

            vbo.Unbind();
            vao.Unbind();
        }
        
        public StaticMesh(IList<T> vertices, IList<uint> indices, Material material) : this(vertices, indices)
        {
            this.Material = material;
        }

        public StaticMesh(IList<T> vertices, Material material) : this(vertices)
        {
            this.Material = material;
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

        public void Draw(Shader shader, Matrix4 model, int instances, Vector3 instanceOffset)
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

    }
}

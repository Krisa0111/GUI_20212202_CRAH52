using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics.OpenGL
{
    internal class DynamicMesh : Mesh
    {
        IList<float> Vertices { get; set; }

        public DynamicMesh(IList<float> vertices, VertexBufferLayout vertexBufferLayout)
            : base(vertices, vertexBufferLayout)
        {
            Vertices = vertices;
        }

        public void UpdateMesh()
        {
            vbo.BufferData(Vertices, BufferUsageHint.DynamicDraw);
            vertexCount = Vertices.Count;
        }
    }
}

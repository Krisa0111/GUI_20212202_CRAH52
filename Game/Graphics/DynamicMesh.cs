using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class DynamicMesh<T> : StaticMesh<T> where T : IVertex
    {
        IList<T> Vertices { get; set; }

        public DynamicMesh(IList<T> vertices) : base(vertices)
        {
            this.Vertices = vertices;
        }

        public void UpdateMesh()
        {
            vbo.BufferData(Vertices, BufferUsageHint.DynamicDraw);
            vertexCount = Vertices.Count;
        }
    }
}

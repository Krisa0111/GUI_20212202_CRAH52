using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics.OpenGL
{
    public enum VertexType
    {
        Byte = VertexAttribPointerType.Byte,
        UnsignedByte = VertexAttribPointerType.UnsignedByte,
        Short = VertexAttribPointerType.Short,
        UnsignedShort = VertexAttribPointerType.UnsignedShort,
        Int = VertexAttribPointerType.Int,
        UnsignedInt = VertexAttribPointerType.UnsignedInt,
        Float = VertexAttribPointerType.Float,
        Double = VertexAttribPointerType.Double,
        HalfFloat = VertexAttribPointerType.HalfFloat,
        Fixed = VertexAttribPointerType.Fixed
    }

    public enum VertexElement
    {
        Position = 0,
        Normal = 1,
        Color = 2,
        TexCoord = 3
    }

    internal class VertexBufferLayout
    {
        public class VertexBufferElement
        {
            public int Layout { get; }
            public int Count { get; }
            public VertexAttribPointerType Type { get; }
            public bool Normalized { get; }
            public int TypeSize { get => GetTypeSize((VertexType)Type); }

            public VertexBufferElement(int layout, int count, VertexAttribPointerType type, bool normalized)
            {
                Layout = layout;
                Count = count;
                Type = type;
                Normalized = normalized;
            }
        }

        private static int GetTypeSize(VertexType type)
        {
            return type switch
            {
                VertexType.Byte => 1,
                VertexType.UnsignedByte => 1,
                VertexType.Short => 2,
                VertexType.UnsignedShort => 2,
                VertexType.Int => 4,
                VertexType.UnsignedInt => 4,
                VertexType.Float => 4,
                VertexType.Double => 8,
                VertexType.HalfFloat => 2,
                VertexType.Fixed => 4,
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }

        public List<VertexBufferElement> Elements { get; private set; }
        public int Size { get; private set; }
        public int Count { get; private set; }


        public VertexBufferLayout()
        {
            Elements = new List<VertexBufferElement>();
            Size = 0;
        }

        public void Push(VertexElement element, VertexType type, int count, bool normalized​ = false)
        {
            if (Elements.Exists(x => x.Layout == (int)element))
                throw new InvalidOperationException();

            Size += count * GetTypeSize(type);
            Count += count;
            Elements.Add(
                new VertexBufferElement(
                    (int)element,
                    count,
                    (VertexAttribPointerType)type,
                    normalized
                ));
        }

    }
}

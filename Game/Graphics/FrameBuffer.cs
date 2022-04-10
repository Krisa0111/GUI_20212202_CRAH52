using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class FrameBuffer
    {
        private int id = 0;
        private int texture;
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Samples { get; private set; }
        private static int DefaultFbo { get; set; }

        private static VertexArray vao;
        private static VertexBuffer<VertexT> vbo;
        private RenderBuffer rbo;

        public FrameBuffer(int width, int height, int samples = 0)
        {
            Width = width;
            Height = height;
            Samples = samples;
            generateFrameBuffer();
        }

        ~FrameBuffer()
        {
            GL.DeleteFramebuffer(id);
            GL.DeleteTexture(texture);
        }

        static FrameBuffer()
        {
            List<VertexT> vertices = new List<VertexT>();
            vertices.Add(new VertexT(new Vector3(-1.0f, 1.0f, 0.0f), new Vector2(0, 1)));
            vertices.Add(new VertexT(new Vector3(-1.0f, -1.0f, 0.0f), new Vector2(0, 0)));
            vertices.Add(new VertexT(new Vector3(1.0f, -1.0f, 0.0f), new Vector2(1, 0)));
            vertices.Add(new VertexT(new Vector3(-1.0f, 1.0f, 0.0f), new Vector2(0, 1)));
            vertices.Add(new VertexT(new Vector3(1.0f, -1.0f, 0.0f), new Vector2(1, 0)));
            vertices.Add(new VertexT(new Vector3(1.0f, 1.0f, 0.0f), new Vector2(1, 1)));

            vbo = new VertexBuffer<VertexT>(vertices, vertices[0].Size, BufferUsageHint.StaticDraw);
            vao = new VertexArray();

            vao.Bind();
            vbo.Bind();

            vertices[0].SetAttributes(vao);

            vbo.Unbind();
            vao.Unbind();
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, id);
        }

        public void Bind(FramebufferTarget framebufferTarget)
        {
            GL.BindFramebuffer(framebufferTarget, id);
        }

        public void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, DefaultFbo);
        }

        public void Draw(Shader shader)
        {
            GL.Disable(EnableCap.DepthTest);

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            shader.Use();
            //shader.SetInt("screenTexture", 0);
            vao.Bind();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }

        public void Resize(int width, int height, int defaultFbo)
        {
            Width = width;
            Height = height;
            DefaultFbo = defaultFbo;
            generateFrameBuffer();
        }

        public void BlitTo(FrameBuffer other)
        {
            if (this.Width == 0 || this.Height == 0) return;
            if (other.Width == 0 || other.Height == 0) return;

            this.Bind(FramebufferTarget.ReadFramebuffer);
            other.Bind(FramebufferTarget.DrawFramebuffer);
            GL.BlitFramebuffer(
                0, 0, this.Width, this.Height,
                0, 0, other.Width, other.Height,
                ClearBufferMask.ColorBufferBit,
                BlitFramebufferFilter.Nearest
                );
        }

        private void generateFrameBuffer()
        {
            if (Width == 0 || Height == 0) return;

            if (id != 0) GL.DeleteFramebuffer(id);
            if (texture != 0) GL.DeleteTexture(texture);

            id = GL.GenFramebuffer();
            Bind();


            texture = GL.GenTexture();
            if (Samples > 1)
            {
                GL.BindTexture(TextureTarget.Texture2DMultisample, texture);
                GL.TexImage2DMultisample(
                    TextureTargetMultisample.Texture2DMultisample,
                    Samples,
                    PixelInternalFormat.Rgb,
                    Width,
                    Height,
                    true
                );
                GL.BindTexture(TextureTarget.Texture2DMultisample, 0);

                GL.FramebufferTexture2D(
                   FramebufferTarget.Framebuffer,
                   FramebufferAttachment.ColorAttachment0,
                   TextureTarget.Texture2DMultisample,
                   texture,
                   0
               );
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, texture);
                GL.TexImage2D(
                    TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgb,
                    Width,
                    Height,
                    0,
                    PixelFormat.Rgb,
                    PixelType.UnsignedByte,
                    IntPtr.Zero
                );

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

                GL.BindTexture(TextureTarget.Texture2D, 0);

                GL.FramebufferTexture2D(
                    FramebufferTarget.Framebuffer,
                    FramebufferAttachment.ColorAttachment0,
                    TextureTarget.Texture2D,
                    texture,
                    0
                );
            }


            if (rbo == null)
                rbo = new RenderBuffer();
            rbo.SetupStorage(Width, Height, RenderbufferStorage.Depth24Stencil8, Samples);
            rbo.Attach(this, FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment);


            var fboStatus = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (fboStatus != FramebufferErrorCode.FramebufferComplete)
                Debug.WriteLine("Framebuffer error: {0}", fboStatus);

        }


    }
}

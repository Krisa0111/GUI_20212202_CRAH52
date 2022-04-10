using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Graphics
{
    internal class RenderBuffer
    {
        private int id = 0;

        public RenderBuffer()
        {
            id = GL.GenRenderbuffer();
        }

        ~RenderBuffer()
        {
            GL.DeleteRenderbuffer(id);
        }

        public void SetupStorage(int width, int height, RenderbufferStorage renderbufferStorage, int samples = 0)
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, id);
            if (samples > 1)
                GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer,
                    samples,
                    renderbufferStorage,
                    width,
                    height
                    );
            else
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer,
                    renderbufferStorage,
                    width,
                    height
                    );
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

        }

        public void Attach(FrameBuffer fbo, FramebufferTarget frameBufferTarget, FramebufferAttachment framebufferAttachment)
        {
            fbo.Bind(frameBufferTarget);
            GL.FramebufferRenderbuffer(frameBufferTarget, framebufferAttachment, RenderbufferTarget.Renderbuffer, id);
        }
    }
}

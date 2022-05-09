using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace Game.Graphics.OpenGL
{
    internal class Skybox
    {
        private float[] skyboxVertices = new float[]
        {
			//   Coordinates
			-1.0f, -1.0f,  1.0f,//        7--------6
			 1.0f, -1.0f,  1.0f,//       /|       /|
			 1.0f, -1.0f, -1.0f,//      4--------5 |
			-1.0f, -1.0f, -1.0f,//      | |      | |
			-1.0f,  1.0f,  1.0f,//      | 3------|-2
			 1.0f,  1.0f,  1.0f,//      |/       |/
			 1.0f,  1.0f, -1.0f,//      0--------1
			-1.0f,  1.0f, -1.0f
        };

        private uint[] skyboxIndices = new uint[]
        {
			// Right
			1, 2, 6,
            6, 5, 1,
			// Left
			0, 4, 7,
            7, 3, 0,
			// Top
			4, 5, 6,
            6, 7, 4,
			// Bottom
			0, 3, 2,
            2, 1, 0,
			// Back
			0, 1, 5,
            5, 4, 0,
			// Front
			3, 7, 6,
            6, 2, 3
        };

        private string[] files = new string[]
        {
            /*"Images/skybox/Daylight Box_Right.bmp",
            "Images/skybox/Daylight Box_Left.bmp",
            "Images/skybox/Daylight Box_Top.bmp",
            "Images/skybox/Daylight Box_Bottom.bmp",
            "Images/skybox/Daylight Box_Front.bmp",
            "Images/skybox/Daylight Box_Back.bmp",*/

            "Images/skybox/nightsky_left.jpg",
            "Images/skybox/nightsky_right.jpg",
            "Images/skybox/nightsky_bottom.jpg",
            "Images/skybox/nightsky_top.jpg",
            "Images/skybox/nightsky_front.jpg",
            "Images/skybox/nightsky_back.jpg",
        };

        private static VertexArray vao;
        private static ElementsBuffer ebo;
        private static VertexBuffer vbo;

        private Shader shader;

        private int cubemap;

        public Skybox()
        {
            vbo = new VertexBuffer(skyboxVertices, BufferUsageHint.StaticDraw);
            vao = new VertexArray();

            VertexBufferLayout vertexBufferLayout = new VertexBufferLayout();
            vertexBufferLayout.Push(VertexElement.Position, VertexType.Float, 3);

            vao.LinkBuffer(vbo, vertexBufferLayout);

            vao.Bind();
            ebo = new ElementsBuffer(skyboxIndices, BufferUsageHint.StaticDraw);
            ebo.Bind();

            vbo.Unbind();
            vao.Unbind();

            shader = new Shader("Shaders/skybox_vert.glsl", "Shaders/skybox_frag.glsl");

            cubemap = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, cubemap);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);

            for (int i = 0; i < 6; i++)
            {
                var image = new Bitmap(files[i]);
                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb
                    );

                GL.TexImage2D(
                    TextureTarget.TextureCubeMapPositiveX + i,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0
                    );
            }

        }

        public void Render(Camera camera)
        {
            GL.DepthFunc(DepthFunction.Lequal);
            GL.Disable(EnableCap.CullFace);

            shader.Use();

            // We make the mat4 into a mat3 and then a mat4 again in order to get rid of the last row and column
            // The last row and column affect the translation of the skybox (which we don't want to affect)
            Matrix4 view = new Matrix4(new Matrix3(camera.GetViewMatrix()));
            Matrix4 projection = camera.GetProjectionMatrix();
            shader.SetMatrix4("view", view);
            shader.SetMatrix4("projection", projection);

            vao.Bind();

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, cubemap);

            GL.DrawElements(PrimitiveType.Triangles, skyboxIndices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);


        }


    }
}

using Game.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;

namespace Game
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    /// 
    public partial class GameWindow : Window
    {
        private Shader shader;
        private Shader playerShader;

        private Camera camera;

        private StaticMesh<VertexColorTextureUV> mesh1;
        private StaticMesh<VertexTextureUV> playerMesh;

        private Texture texture1;
        private Texture playerTexture;

        public GameWindow()
        {
            InitializeComponent();

            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 3
            };
            OpenTkControl.Start(settings);

            GL.Enable(EnableCap.DepthTest);
            
            shader = new Shader("Shaders/simplev.glsl", "Shaders/colortexturef.glsl");
            playerShader = new Shader("Shaders/playerv.glsl", "Shaders/texturef.glsl");
            texture1 = Texture.LoadFromFile("Images/metalbox.png");
            playerTexture = Texture.LoadFromFile("Images/running.png");

            camera = new Camera(Vector3.UnitZ * 1, (float)(ActualWidth / ActualHeight));

            List<VertexColorTextureUV> vertices1 = new List<VertexColorTextureUV>();

            vertices1.Add(new VertexColorTextureUV(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(0,0,1), new Vector3(0,0,1), new Vector2(0,0)));
            vertices1.Add(new VertexColorTextureUV(new Vector3(0.5f, -0.5f, 0.0f), new Vector3(0,0,1), new Vector3(0,1,0), new Vector2(1,0)));
            vertices1.Add(new VertexColorTextureUV(new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0,0,1), new Vector3(1,0,0), new Vector2(.5f,1)));

            mesh1 = new StaticMesh<VertexColorTextureUV>(vertices1);

            List<VertexTextureUV> vertices2 = new List<VertexTextureUV>();

            vertices2.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(0, 0)));
            vertices2.Add(new VertexTextureUV(new Vector3(-0.5f, 0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(0, 1)));
            vertices2.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(1, 1)));
            vertices2.Add(new VertexTextureUV(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(0, 0)));
            vertices2.Add(new VertexTextureUV(new Vector3(0.5f, 0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(1, 1)));
            vertices2.Add(new VertexTextureUV(new Vector3(0.5f, -0.5f, 0.0f), new Vector3(0, 0, 1), new Vector2(1, 0)));

            playerMesh = new StaticMesh<VertexTextureUV>(vertices2);

        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            camera.AspectRatio = (float)(ActualWidth / ActualHeight);
        }

        int ang = 0;
        int x = 1;
        int y = 1;
        private void OpenTkControl_OnRender(TimeSpan delta)
        {
            GL.ClearColor(Color4.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            ang++;

            shader.Use();
            var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(ang)) * Matrix4.CreateTranslation(Vector3.UnitX);
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", camera.GetViewMatrix());
            shader.SetMatrix4("projection", camera.GetProjectionMatrix());

            texture1.Use(TextureUnit.Texture0);

            mesh1.Draw();


            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusConstantAlpha);

            playerShader.Use();
            playerShader.SetMatrix4("model", Matrix4.Identity);
            playerShader.SetMatrix4("view", camera.GetViewMatrix());
            playerShader.SetMatrix4("projection", camera.GetProjectionMatrix());
            playerShader.SetVector2("textureID", new Vector2(x, y));
            x++;
            if (x > 16)
            {
                x = 1;
                y++;
                if (y > 8) y = 1;
            }

            playerTexture.Use(TextureUnit.Texture0);

            playerMesh.Draw();

            GL.Disable(EnableCap.Blend);


            var code = GL.GetError();
            while (code != ErrorCode.NoError)
            {
                Debug.WriteLine(code);
                code = GL.GetError();
            }

        }

    }
}

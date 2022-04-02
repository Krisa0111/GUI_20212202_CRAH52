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

        private Camera camera;

        private Mesh mesh;

        public GameWindow()
        {
            InitializeComponent();

            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 3
            };
            OpenTkControl.Start(settings);

            shader = new Shader("Shaders/simplev.glsl", "Shaders/simplef.glsl");

            camera = new Camera(Vector3.UnitZ * 3, (float)(ActualWidth / ActualHeight));

            List<Vertex> vertices = new List<Vertex>();

            vertices.Add(new Vertex(new Vector3(-0.5f, -0.5f, 0.0f), new Vector3(0), new Vector3(0), new Vector2(0)));
            vertices.Add(new Vertex(new Vector3(0.5f, -0.5f, 0.0f), new Vector3(0), new Vector3(0), new Vector2(0)));
            vertices.Add(new Vertex(new Vector3(0.0f, 0.5f, 0.0f), new Vector3(0), new Vector3(0), new Vector2(0)));

            mesh = new Mesh(vertices);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            camera.AspectRatio = (float)(ActualWidth / ActualHeight);
        }

        int ang = 0;
        private void OpenTkControl_OnRender(TimeSpan delta)
        {
            GL.ClearColor(Color4.Blue);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use();
            var model = Matrix4.Identity * Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(ang++));
            shader.SetMatrix4("model", model);
            shader.SetMatrix4("view", camera.GetViewMatrix()); // camera.GetViewMatrix()
            shader.SetMatrix4("projection", camera.GetProjectionMatrix()); // camera.GetProjectionMatrix()

            mesh.Draw();

            var code = GL.GetError();
            while (code != ErrorCode.NoError)
            {
                Debug.WriteLine(code);
                code = GL.GetError();
            }

        }


    }
}

using Game.Graphics;
using Game.Renderer;
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
        GameDisplay gameDisplay;

        double fps;

        public GameWindow()
        {
            InitializeComponent();

            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 3
            };
            OpenTkControl.Start(settings);

            gameDisplay = new GameDisplay(OpenTkControl.FrameBufferWidth, OpenTkControl.FrameBufferHeight);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            gameDisplay.Resize(OpenTkControl.FrameBufferWidth, OpenTkControl.FrameBufferHeight, OpenTkControl.Framebuffer);
        }
        
        private void OpenTkControl_OnRender(TimeSpan delta)
        {
            gameDisplay.Render();

            fps = 1.0 / delta.TotalSeconds;

            lb_fps.Content = Math.Round(fps);

            var code = GL.GetError();
            while (code != ErrorCode.NoError)
            {
                Debug.WriteLine(code);
                code = GL.GetError();
            }

        }

    }
}

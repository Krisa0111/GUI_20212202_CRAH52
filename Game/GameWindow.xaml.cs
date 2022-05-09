using Game.Controller;
using Game.Renderer;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
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
        private IGameDisplay gameDisplay;
        private IGameController controller;
        private bool IsRunning = true;
        private double fps;

        public GameWindow()
        {
            InitializeComponent();

            var settings = new GLWpfControlSettings
            {
                MajorVersion = 3,
                MinorVersion = 3
            };
            OpenTkControl.Start(settings);

            gameDisplay = Ioc.Default.GetService<IGameDisplay>();
            controller = Ioc.Default.GetService<IGameController>();
            gameDisplay.GameDisplayOver += GameDisplay_GameDisplayOver;
        }

        private void GameDisplay_GameDisplayOver(float obj)
        {
            MessageBox.Show("A játék véget ért. Brendon nem ért be a munkahelyére. A pontszámod: " + obj);
            IsRunning = false;

        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            gameDisplay.Resize(OpenTkControl.FrameBufferWidth, OpenTkControl.FrameBufferHeight, OpenTkControl.Framebuffer);
        }
        
        private void OpenTkControl_OnRender(TimeSpan delta)
        {
            if (IsRunning == false)
            {
                this.Close();
                
            }
            gameDisplay.Render(delta.TotalSeconds);

            fps = 1.0 / delta.TotalSeconds;

            lb_info.Content = $"FPS: {Math.Round(fps)} TickRate: {Math.Round(gameDisplay.TickRate)}";

            var code = GL.GetError();
            while (code != ErrorCode.NoError)
            {
                Debug.WriteLine(code);
                code = GL.GetError();
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            gameDisplay.Dispose();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gameDisplay.Start();
        }

        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            controller.KeyPressed(e.Key);
        }
    }
}

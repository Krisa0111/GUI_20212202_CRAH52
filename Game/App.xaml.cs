using Game.Graphics;
using Game.Logic;
using Game.ViewModel;
using Game.Controller;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Game.Renderer;

namespace Game
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddSingleton<IRenderer, OpenGLRenderer>()
                .AddSingleton<IGameDisplay, GameDisplay>()
                .AddSingleton<IGameController, GameController>()
                .AddSingleton<IGameModel, GameModel>()
                .AddSingleton<IGameLogic, GameLogic>()
                .AddSingleton<IMainMenuModel, MainMenuModel>()
                .AddSingleton<IMainMenuDisplay, MainMenuDisplay>()
                .BuildServiceProvider()
                );
        }
    }
}

﻿using Game.Controller;
using Game.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameController gameController;
        public MainWindow()
        {
            InitializeComponent();
            GameLogic logic = new GameLogic();
            display
        }
        public MainMenuDisplay mainMenuDisplay;

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            mainmenudisplay.InvalidateVisual();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}

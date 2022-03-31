using Game.Controller;
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
        
        HighScoreWindow highScoreWindow = new HighScoreWindow();
        public MainWindow()
        {
            InitializeComponent();
            GameLogic logic = new GameLogic();
            
        }
        public MainMenuDisplay mainMenuDisplay;

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            mainmenudisplay.InvalidateVisual();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void New_Game_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            GameWindow gameWindow = new GameWindow();
            gameWindow.Show();
        }
        private void Exit_Game_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HighScoreTable_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            highScoreWindow.Show();
        }
    }
}

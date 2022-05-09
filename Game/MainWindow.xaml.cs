using Game.Controller;
using Game.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

        private SoundPlayer theme = new SoundPlayer(@"SoundEffects\theme.wav");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            mainmenudisplay.InvalidateVisual();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            theme.PlayLooping();
        }

        private void Exit_Game_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HighScore_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            HighScoreWindow highScoreWindow = new HighScoreWindow();
            highScoreWindow.Show();
            this.Show();
        }

        private void New_Game_ButtonClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            GameWindow gameWindow = new GameWindow();
            gameWindow.ShowDialog();
            this.Show();
        }
    }
}

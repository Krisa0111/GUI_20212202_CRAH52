using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Game
{
    public class HighScoreTableDisplay : FrameworkElement
    {
        
        Size size;
        public string[] Scores {get; set;}
        public void Resize(Size size)
        {
            this.size = size;
        }
        public HighScoreTableDisplay()
        {
            this.Scores = WriteOutHighScore(ScoreArray());
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            
            if (ActualWidth > 0 && ActualHeight > 0)
            {
                drawingContext.DrawRectangle(BackGroundBrush, null, new Rect(0, 0, ActualWidth, ActualHeight));
            }

        }
        public Brush BackGroundBrush
        {
            get
            {
                return new ImageBrush(new BitmapImage(new Uri(Path.Combine(Directory.GetCurrentDirectory(), ("Images"), "background.jpg"))));
            }
        }
        private string[] ScoreArray()
        {
            string[] scoreArray = new string[10];
            StreamReader sr = new StreamReader(@"..\..\..\..\Game\HighScores\HighScoreTxt.txt");
            for (int i = 0; i < scoreArray.Length; i++)
            {
                scoreArray[i] = sr.ReadLine();
            }
            sr.Close();
            return scoreArray;
        }
        private string[] WriteOutHighScore(string[] scoreArray)
        {
            string[] writeoutScores = new string[5];
            writeoutScores[0] = "Champion:  " + scoreArray[0].Split('#')[0] + " --> " + scoreArray[0].Split('#')[1];
            writeoutScores[1] = "2nd place: " + scoreArray[1].Split('#')[0] + " --> " + scoreArray[1].Split('#')[1];
            writeoutScores[2] = "3rd place: " + scoreArray[2].Split('#')[0] + " --> " + scoreArray[2].Split('#')[1];
            writeoutScores[3] = "4th place: " + scoreArray[3].Split('#')[0] + " --> " + scoreArray[3].Split('#')[1];
            writeoutScores[4] = "5th place: " + scoreArray[4].Split('#')[0] + " --> " + scoreArray[4].Split('#')[1];
            return writeoutScores;
        }
    }
}

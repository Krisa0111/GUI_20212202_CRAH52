using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Game.HighScores
{
    public static class HighScoreManager
    {
        public static void EndOfTheGame(float finalScore)
        {
            string[] lines = new string[10];
            //lines = File.ReadAllLines("HighScoreTxt.txt");
            string[] plusLines = new string[11];
            StreamReader sr = new StreamReader("HighScoreTxt.txt");
            string[] lines1 = new string[11];
            for (int i = 0; i < 10; i++)
            {
                lines1[i] = sr.ReadLine();
            }
            sr.Close();
            for (int i = 0; i < lines1.Length; i++)
            {
                plusLines[i] = lines1[i];
            }
            plusLines[plusLines.Length - 1] = finalScore.ToString();
            float[] newArray = new float[plusLines.Length];
            for (int i = 0; i < newArray.Length; i++)
            {
                newArray[i] = float.Parse(plusLines[i]);
            }
            ;


            Array.Reverse(newArray);

            float[] finalArray = new float[newArray.Length-1];
            for (int i = 0; i < finalArray.Length; i++)
            {
                finalArray[i] = newArray[i];
            }

            StreamWriter sw = new StreamWriter("HighScoreTxt.txt");
            for (int i = 0; i < finalArray.Length; i++)
            {
            sw.WriteLine(finalArray[i]);

            }
            sw.Close();
        }
    }
}

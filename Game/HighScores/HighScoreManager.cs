using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace Game.HighScores
{
    public static class HighScoreManager
    {
        public static void EndOfTheGame(float finalScore, string name)
        {
            //lines = File.ReadAllLines("HighScoreTxt.txt");
            string[] plusLines = new string[11];
            StreamReader sr = new StreamReader(@"..\..\..\..\Game\HighScores\HighScoreTxt.txt");
            string[] lines1 = new string[10];
            string[] lines2 = new string[10];
            for (int i = 0; i < 10; i++)
            {
                string line = sr.ReadLine();
                lines1[i] = line.Split('#')[0];
                lines2[i] = line.Split('#')[1];
            }
            sr.Close();
            for (int i = 0; i < lines1.Length; i++)
            {
                plusLines[i] = lines1[i] + '#' + lines2[i];
            }
            plusLines[plusLines.Length - 1] = finalScore.ToString() + '#' + name;

            for (int i = plusLines.Length-1; i>=0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (float.Parse(plusLines[j].Split('#')[0], CultureInfo.InvariantCulture ) < float.Parse(plusLines[j+1].Split('#')[0],CultureInfo.InvariantCulture))
                    {
                        string tmp = plusLines[j];
                        plusLines[j] = plusLines[j+1];
                        plusLines[j+1] = tmp;
                    }
                }
            }
            ;


            StreamWriter sw = new StreamWriter(@"..\..\..\..\Game\HighScores\HighScoreTxt.txt");
            for (int i = 0; i < plusLines.Length-1; i++)
            {
                sw.WriteLine(plusLines[i]);
            }
            sw.Close();
        }
    }
}

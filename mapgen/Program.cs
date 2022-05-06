using System;
using System.IO;

namespace txtgenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();
            string[] entities = new string[] { "RoadBlock", "Car", "BluePortal", "RedPortal", "Watch", "Accelerator", "PlusLife", "Skull", "Random" };
            bool end = false;
            int maxz = 3;
            int i = 0;
            int ix = 2;
            StreamWriter sw = new StreamWriter("map.txt");
            sw.WriteLine("Road 0 0 20");
            while (!end)
            {
                string entitie = new string("");
                int entitierandom = rnd.Next(0, 104);
                if (entitierandom >= 0 && entitierandom <= 40)
                {
                    entitie = entities[0];
                }
                else if (entitierandom >= 41 && entitierandom <= 80)
                {
                    entitie = entities[1];
                }
                else if (entitierandom >= 81 && entitierandom <= 84)
                {
                    entitie = entities[2];
                }
                else if (entitierandom >= 85 && entitierandom <= 88)
                {
                    entitie = entities[3];
                }
                else if (entitierandom >= 89 && entitierandom <= 92)
                {
                    entitie = entities[4];
                }
                else if (entitierandom >= 93 && entitierandom <= 96)
                {
                    entitie = entities[5];
                }
                else if (entitierandom >= 97 && entitierandom <= 100)
                {
                    entitie = entities[6];
                }
                else if (entitierandom >= 100 && entitierandom <= 103)
                {
                    entitie = entities[7];
                }
                else
                {
                    entitie = entities[8];
                }
                int x = rnd.Next(-1, 2);
                int y = 0;
                int z = rnd.Next(ix, maxz);
                if (i % 2 == 0)
                {
                    ix++;
                }
                i++;
                maxz++;
                string line = entitie + " " + x + " " + y + " " + z;
                sw.WriteLine(line);
                if (i == 40)
                {
                    end = true;
                }
            }
            sw.Close();
        }
    }
}

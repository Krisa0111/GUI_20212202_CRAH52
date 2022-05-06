using System;
using System.Globalization;
using System.IO;

namespace txtgenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string path2 =  @"..\..\..\..\..\Game\Game\Resources\Maps\map2.txt";
            string path1 =  @"..\..\..\..\..\Game\Game\Resources\Maps\map1.txt";
            string path3 =  @"..\..\..\..\..\Game\Game\Resources\Maps\map3.txt";
            string path4 =  @"..\..\..\..\..\Game\Game\Resources\Maps\map4.txt";
            string path5 =  @"..\..\..\..\..\Game\Game\Resources\Maps\map5.txt";
            string path6 =  @"..\..\..\..\..\Game\Game\Resources\Maps\map6.txt";
            string path7 =  @"..\..\..\..\..\Game\Game\Resources\Maps\map7.txt";
            string path8 =  @"..\..\..\..\..\Game\Game\Resources\Maps\map8.txt";
            string path9 =  @"..\..\..\..\..\Game\Game\Resources\Maps\map9.txt";
            string path10 = @"..\..\..\..\..\Game\Game\Resources\Maps\map10.txt";
            string path11 = @"..\..\..\..\..\Game\Game\Resources\Maps\map11.txt";
            string path12 = @"..\..\..\..\..\Game\Game\Resources\Maps\map12.txt";
            string path13 = @"..\..\..\..\..\Game\Game\Resources\Maps\map13.txt";
            string path14 = @"..\..\..\..\..\Game\Game\Resources\Maps\map14.txt";
            string path15 = @"..\..\..\..\..\Game\Game\Resources\Maps\map15.txt";
            string path16 = @"..\..\..\..\..\Game\Game\Resources\Maps\map16.txt";
            string path17 = @"..\..\..\..\..\Game\Game\Resources\Maps\map17.txt";
            string path18 = @"..\..\..\..\..\Game\Game\Resources\Maps\map18.txt";
            string path19 = @"..\..\..\..\..\Game\Game\Resources\Maps\map19.txt";
            string path20 = @"..\..\..\..\..\Game\Game\Resources\Maps\map20.txt";
            Gen(path1);
            Gen(path2);
            Gen(path3);
            Gen(path4);
            Gen(path5);
            Gen(path6);
            Gen(path7);
            Gen(path8);
            Gen(path9);
            Gen(path10);
            Gen(path11);
            Gen(path12);
            Gen(path13);
            Gen(path14);
            Gen(path15);
            Gen(path16);
            Gen(path17);
            Gen(path18);
            Gen(path19);
            Gen(path20);
            
        }
        public static void Gen(string path)
        {
            string[] entities = new string[] { "RoadBlock", "Car", "BluePortal", "RedPortal", "Watch", "Accelerator", "PlusLife", "Skull", "Random" };
            Random rnd = new Random();
            bool end = false;
            int maxz = 5;
            int i = 0;
            int ix = 2;
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("Road 0 0 20");
            while (!end)
            {
                string entitie = new string("");
                double x = rnd.Next(-1, 2);
                double y = 0;
                double z = rnd.Next(ix, maxz);
                double entitierandom = rnd.Next(0, 104);
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
                    y = 0.7;
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
                string _x = x.ToString(CultureInfo.InvariantCulture);
                string _y = y.ToString(CultureInfo.InvariantCulture);
                string _z = z.ToString(CultureInfo.InvariantCulture);
                
                i+=3;
                maxz+=3;
                string line = entitie + " " + _x + " " + _y + " " + _z;
                sw.WriteLine(line);
                if (i > 40)
                {
                    end = true;
                }
            }
            sw.Close();
            
        }
    }
}

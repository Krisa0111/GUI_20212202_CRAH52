using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Game.Logic
{
    class MapGeneratorLogic
    {
        static void Generate(List<GameItem> gameItems)
        {
            // 0 = SEMMI
            // A = Accelerator
            // B = Blue portal
            // X = Box
            // D = Decelerator
            // L = PlusLife
            // P = PremiumPortal
            // R = RanddomItem
            // E = RedPortal
            // S = Skull
            StreamReader sr = new StreamReader("map.txt");
            while (!sr.EndOfStream)
            {
                string row = sr.ReadLine();
                if (row[0] == 0)
                {

                }
                else if (row[0] == 'A')
                {

                }
            }





            sr.Close();

        }
        
    }
}

using System;
using System.Windows;
using System.Windows.Media;

namespace Game.Model
{
    public class Accelerator:GameItem
    {
        public System.Drawing.Point Center { get; set; }
        static Random r = new Random();

        public override Geometry Area { get; }

        private int Randomizer(int min, int max)
        {
            int rnd = 0;
            do
            {
                rnd = r.Next(min, max + 1);
            } while (rnd == 0);
            return rnd;
        }
        public Accelerator(int xPos, int yPos)
        {
            Center = new System.Drawing.Point(xPos, yPos);
        }
        
        
    }
}
using System;
using System.Windows;

namespace Game.Model
{
    public class Accelerator
    {
        public System.Drawing.Point Center { get; set; }
        static Random r = new Random();
        public Vector Speed { get; set; }
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
            xPos = r.Next(-5, 5);
            yPos = r.Next(-5, 5);
            Center = new System.Drawing.Point(xPos, yPos);

        }
    }
}
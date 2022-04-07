using System;
using System.Windows;

namespace Game.Model
{
    public class PremiumPortal: IPortal
    {
        static Random r = new Random();

        public Vector ToWhere { get; set; }

        private int Randomizer(int min, int max)
        {
            int rnd = 0;
            do
            {
                rnd = r.Next(min, max + 1);
            } while (rnd == 0);
            return rnd;
        }
        public PremiumPortal()
        {

        }
    }
}
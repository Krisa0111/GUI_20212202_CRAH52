using System;

namespace Game.Model
{
    public class Skull
    {
        static Random r = new Random();
        private int Randomizer(int min, int max)
        {
            int rnd = 0;
            do
            {
                rnd = r.Next(min, max + 1);
            } while (rnd == 0);
            return rnd;
        }
    }
}
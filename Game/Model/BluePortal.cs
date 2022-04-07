using System;
using System.Windows;

namespace Game.Model
{
    public class BluePortal:IPortal
    {
        static Random r = new Random();

        public Vector ToWhere { get; set; }

        public BluePortal(Vector position)
        {
            
        }
        
    }
}
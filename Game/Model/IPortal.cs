using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game.Model
{
    interface IPortal
    {
        public Vector ToWhere { get; set; }
    }
}

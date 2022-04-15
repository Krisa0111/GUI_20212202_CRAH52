using Game.ResourceLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ViewModel
{
    public abstract class Map
    {
        public Model Model { get; }
        public float ModelLength { get; }

        protected static ModelLoader ModelLoader { get; set; }

        static Map()
        {
            ModelLoader = ModelLoader.GetInstance();
        }

        protected Map(Model model, float modelLength)
        {
            Model = model;
            ModelLength = modelLength;
        }
    }
}

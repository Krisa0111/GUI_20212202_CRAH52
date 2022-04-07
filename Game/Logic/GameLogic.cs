using Game.Controller;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Game.Logic
{
    class GameLogic : IGameController, IGameModell
    {
        public int Life { get; set; }
        public int Score { get; set; }
        public List<Decelerator> Decelerators { get; set; }
        public List<Accelerator> Accelerators { get; set; }
        public List<BluePortal> BluePortals { get; set; }
        public List<RedPortal> RedPortals { get; set; }
        public List<PlusLife> PlusLifes { get; set; }
        public List<PremiumPortal> PremiumPortals { get; set; }
        public List<RandomItem> RandomItems { get; set; }
        public List<Skull> Skulls { get; set; }
        public List<Box> Boxes { get; set; }
        public event EventHandler Changed;
        public event EventHandler GameOver;
        public GameLogic()
        {
            this.Life = 3;
            this.Score = 0;
        }
        Size area;
        public void SetupSizes(Size area)
        {
            this.area = area;
            Decelerators = new List<Decelerator>();
            Accelerators = new List<Accelerator>();
            BluePortals = new List<BluePortal>();
            RedPortals = new List<RedPortal>();
            PlusLifes = new List<PlusLife>();
            PremiumPortals = new List<PremiumPortal>();
            RandomItems = new List<RandomItem>();
            Skulls = new List<Skull>();

        }
        

        public void Move(Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}

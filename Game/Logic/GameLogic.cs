using Game.Controller;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Game.Logic
{
    class GameLogic : IGameController, IGameModell
    {
        public int Score { get; set; }
        public List<GameItem> GameItems { get; set; }
        public Player Player { get; set; }
        public event EventHandler Changed;
        public event EventHandler GameOver;
        public GameLogic()
        {
            this.Score = 0;
        }
        Size area;
        public void SetupSizes(Size area)
        {
            this.area = area;
            GameItems = new List<GameItem>();
            Player = new Player();

        }
        public bool IsCollision(GameItem other)
        {
            return Geometry.Combine(
                Player.Area,
                other.Area,
                GeometryCombineMode.Intersect,
                null
                ).GetArea() > 0;
        }
        public void Update()
        {
            foreach (var item in GameItems)
            {
                if (IsCollision(item))
                {
                    if (item is Accelerator)
                    {
                        Player.Speed += new Vector(0, 10); // CHNAGE THIS ACCORDING TO TESTS
                        Score -= 5;
                    }
                    else if (item is Decelerator)
                    {
                        Player.Speed -= new Vector(0, 10); // CHNAGE THIS ACCORDING TO TESTS
                        Score += 5;
                    }
                    else if (item is BluePortal )
                    {
                        Player.Position += new Vector(0, 20);
                        Score += 20;
                    }
                    else if (item is RedPortal )
                    {
                        Player.Position -= new Vector(0, 20);
                        Score -= 20;
                    }
                    else if (item is PlusLife)
                    {
                        if (Player.MaxLife >Player.Life)
                        {
                            Player.Life++;
                        }
                    }
                    else if (item is PremiumPortal)
                    {
                        //TO DO
                        Player.Position += new Vector();
                    }
                    else if (item is RandomItem)
                    {

                    }
                    else if (item is Skull)
                    {
                        // Game over
                    }
                }
                
            }
        }
        public void Move(Directions direction)
        {
            throw new NotImplementedException();
        }
    }
}

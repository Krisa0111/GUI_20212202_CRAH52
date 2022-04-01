using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Game.Enums;
namespace Game.Logic
{
    
    public class CharacterLogic : GameLogic
    {
        public int ZCoordinate { get; set; }
        public int PrevScore { get; set; }
        public int PrevLife { get; set; }
        public CharacterLogic(int score, int life, bool continueGame) : base(score, life, continueGame)
        {
            if (continueGame)
            {
                score = PrevScore;
                life = PrevLife;
            }
            else
            {
                score = 0;
                life = 3;
            }
        }

        public int Life { get; set; }
        public int Score { get; set; }
        GameLogic gameLogic { get; set; }
        public Position currentPosition { get; set; }
        
        public void ButtonPressed(Key pressedKey)
        {
            switch (pressedKey)
            {
                case Key.Up or Key.W:
                    
                    break;
                case Key.Down or Key.S:

                    break;
                case Key.Right or Key.D:
                    if (currentPosition != Position.Right)
                    {
                        Move(pressedKey);
                    }
                    break;
                case Key.Left or Key.A:
                    if (currentPosition != Position.Left)
                    {
                        Move(pressedKey);
                    }
                    break;
                default:
                    break;
            }
        }

        public void Move(Key pressedKey)
        {
            if ((pressedKey == Key.Right || pressedKey == Key.D) && currentPosition == Position.Left)
            {
                currentPosition = Position.Mid;
            }
            else if ((pressedKey == Key.Right || pressedKey == Key.D) && currentPosition == Position.Mid)
            {
                currentPosition = Position.Right;
            }
            else if ((pressedKey == Key.Left || pressedKey == Key.A) && currentPosition == Position.Mid)
            {
                currentPosition = Position.Left;
            }
            else if((pressedKey == Key.Left || pressedKey == Key.A) && currentPosition == Position.Right)
            {
                currentPosition = Position.Mid;
            }
            else
            {
                throw new Exception();
            }
        }
        public void Change_Speed()
        {

        }
        public void Change_Score()
        {

        }


    }
}

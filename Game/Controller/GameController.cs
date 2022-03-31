using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Game.Controller
{
    class GameController
    {
        IGameController gameController;
        public GameController(IGameController gameController)
        {
            this.gameController = gameController;
        }
        public void KeyPressed(Key key)
        {
            switch (key)
            {
                case Key.Up:
                    gameController.Move(Directions.Up);
                    break;
                case Key.Down:
                    gameController.Move(Directions.Down);
                    break;
                case Key.Right:
                    gameController.Move(Directions.Right);
                    break;
                case Key.Left:
                    gameController.Move(Directions.Left);
                    break;
                case Key.W:
                    gameController.Move(Directions.Up);
                    break;
                case Key.S:
                    gameController.Move(Directions.Down);
                    break;
                case Key.D:
                    gameController.Move(Directions.Right);
                    break;
                case Key.A:
                    gameController.Move(Directions.Left);
                    break;
                case Key.Escape:
                    ;
                    break;
                default:
                    break;
            }
        }
    }
}

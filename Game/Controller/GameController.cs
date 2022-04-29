using Game.Graphics;
using Game.Logic;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Game.Controller
{
    class GameController : IGameController
    {
        IGameLogic logic = Ioc.Default.GetService<IGameLogic>();
        IRenderer renderer = Ioc.Default.GetService<IRenderer>();


        public void KeyPressed(Key key)
        {
            switch (key)
            {
                case Key.Up:
                case Key.W:
                    logic.Move(Directions.Up);
                    break;
                case Key.Down:
                case Key.S:
                    logic.Move(Directions.Down);
                    break;
                case Key.Right:
                case Key.D:
                    logic.Move(Directions.Right);
                    break;
                case Key.Left:
                case Key.A:
                    logic.Move(Directions.Left);
                    break;
                case Key.D1:
                    renderer.ShowNormals = !renderer.ShowNormals;
                    break;
                case Key.D2:
                    renderer.ShowWireframe = !renderer.ShowWireframe;
                    break;
                case Key.D3:
                    renderer.ShowColliders = !renderer.ShowColliders;
                    break;
                default:
                    break;
            }
        }
    }
}

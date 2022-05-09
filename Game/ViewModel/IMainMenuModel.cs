using Game.ViewModel.Entities;
using System.Collections.Generic;

namespace Game.ViewModel
{
    internal interface IMainMenuModel
    {
        List<Entity> Entities { get; set; }
        Player Player { get; set; }

        void Reset();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public enum ScreenStates
    {
        Menu,
        Title,
        ChoosePlayOrMakeMap,
        MakeMap,
        Game,
        Pause,
        Setting,
        Rules
    }

    public enum ConnectionTypes 
    { 
        Bottom,
        Top,
        Left,
        Right
    }
}

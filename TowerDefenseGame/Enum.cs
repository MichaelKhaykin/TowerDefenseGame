using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGame
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
        Rules,
        LoseScreen
    }

    public enum ConnectionTypes 
    { 
        Bottom,
        Top,
        Left,
        Right,
        None
    }

    public enum TroopMovingStates
    {
        CrossingTile,
        AtEndOfTile
    }



}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseGameWillFinishThisOne
{
    public class TileInfo
    {
        public Vector2 Position { get; private set; }
        public ConnectionTypes[] Connections { get; private set; }
        public string TileName { get; private set; }

        public TileInfo(string tileName, Vector2 position, params ConnectionTypes[] connections)
        {
            TileName = tileName;
            Connections = connections;
            Position = position;
        }
    }
}

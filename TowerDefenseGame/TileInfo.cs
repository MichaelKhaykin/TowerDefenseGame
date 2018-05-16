using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseGame
{
    public struct TileInfo
    {
        public Vector2 Position { get; private set; }
        public Vector2 GridPosition { get; set; }
        public ConnectionTypes[] Connections { get; private set; }
        public string TileName { get; private set; }

        public ConnectionTypes TileApproachedFrom { get; set; }

        public List<Vector2> PathPositions { get; set; }
        public bool IsPathPositionListConfigured { get; set; } 
        public bool IsPathPositionListReversed { get; set; }

        public TileInfo(string tileName, Vector2 position, params ConnectionTypes[] connections)
        {
            TileApproachedFrom = ConnectionTypes.None;
            IsPathPositionListConfigured = false;
            IsPathPositionListReversed = false;
            PathPositions = new List<Vector2>();
            GridPosition = new Vector2();
            TileName = tileName;
            Connections = connections;
            Position = position;
        }

        public override bool Equals(object obj)
        {
            return this == (TileInfo)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(TileInfo lhs, TileInfo rhs)
        {
            if (lhs.GridPosition == rhs.GridPosition)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(TileInfo lhs, TileInfo rhs)
        {
            return !(lhs == rhs);
        }

        public bool ReversePathPositions()
        {
            if(IsPathPositionListReversed)
            {
                return false;
            }

            IsPathPositionListReversed = true;
            PathPositions.Reverse();

            return true;
        }
    }
}

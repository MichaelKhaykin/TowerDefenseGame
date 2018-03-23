using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Tile : Sprite
    {
        public ConnectionTypes[] Connections { get; set; }

        public Tile(TileCreateInfo tileInfo, Vector2 position, Vector2 scale) 
            : base(tileInfo.Texture, position, Color.White, scale)
        {
            Connections = tileInfo.Connections;
        }


        public Tile(TileInfo tileInfo, ContentManager content)
            : base(content.Load<Texture2D>(tileInfo.TileName), tileInfo.Position, Color.White, new Vector2(Main.SpriteScales[tileInfo.TileName] * Main.ScreenScale))
        {
            Connections = tileInfo.Connections;
        }


        public TileInfo GetInfo()
        {
            return new TileInfo(Texture.Name, Position, Connections);
        }

        public override bool Equals(object obj)
        {
            return (Tile)obj == this;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Tile lhs, Tile rhs)
        {
            if (lhs is null || rhs is null)
            {
                return false;
            }
            if ((int)lhs.Position.X == (int)rhs.Position.X && (int)lhs.Position.Y == (int)rhs.Position.Y && lhs.Connections == rhs.Connections)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Tile lhs, Tile rhs)
        {
            return !(lhs == rhs);
        }

    }
}

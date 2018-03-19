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

    }
}

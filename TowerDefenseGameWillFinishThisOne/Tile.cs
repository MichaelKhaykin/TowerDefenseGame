using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Tile : Sprite
    {
        public bool IsStartingTile { get; set; }
        public bool IsEndingTile { get; set; }
        
        public ConnectionTypes[] Connections { get; set; }

        public List<Vector2> PathPositions = new List<Vector2>();

        public string Name { get; set; }

        public Vector2 GridPosition { get; set; }

        [JsonIgnore]
        public override Texture2D Texture { get => base.Texture; set => base.Texture = value; }

        public Tile(TileCreateInfo tileInfo, Vector2 position, Vector2 scale, string name)
            : base(tileInfo.Texture, position, Color.White, scale)
        {
            PathPositions = tileInfo.PathPositions;
            Connections = tileInfo.Connections;
            Name = name;
        }


        public Tile(TileInfo tileInfo, ContentManager content, string name)
            : base(content.Load<Texture2D>(tileInfo.TileName), tileInfo.Position, Color.White, new Vector2(Main.SpriteScales[tileInfo.TileName] * Main.ScreenScale))
        {
            Connections = tileInfo.Connections;
            Name = name;
        }


        [JsonConstructor]
        private Tile() 
            : base(null, Vector2.Zero, Color.White, Vector2.One)
        {

        }

        public TileInfo GetInfo()
        {
            return new TileInfo(Texture.Name, Position, Connections);
        }

        public override bool Equals(object obj)
        {
            var tile = obj as Tile;
            return tile == this;
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
            if ((int)lhs.Position.X == (int)rhs.Position.X && (int)lhs.Position.Y == (int)rhs.Position.Y && lhs.Connections == rhs.Connections && lhs.IsStartingTile == rhs.IsStartingTile && lhs.IsEndingTile == rhs.IsEndingTile)
            {
                return true;
            }
            return false;
        }

        public static bool operator !=(Tile lhs, Tile rhs)
        {
            return !(lhs == rhs);
        }

        public bool IsClicked(MouseState mouse)
        {
            if (mouse.LeftButton == ButtonState.Pressed && HitBox.Contains(mouse.X, mouse.Y))
            {
                return true;
            }
            return false;
        }
    }
}

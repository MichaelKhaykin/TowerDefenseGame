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
    public class Enemy : AnimationSprite
    {
        public int Health { get; set; }
        public int Speed { get; set; }
        public Vector2 OldPos { get; set; }

        public Stack<Vertex<Tile, ConnectionTypes>> Path = null;

        public Vertex<Tile, ConnectionTypes> CurrentTile { get; set; }
        public Vertex<Tile, ConnectionTypes> PreviousTile { get; set; }

        public Vector2 CurrentStartPoint { get; set; }
        public Vector2 CurrentEndPoint { get; set; }

        public float TravelPercentage { get; set; } = 0f;
        public int CurrentPointIndex { get; set; } = 0;

        public Enemy(Texture2D texture, Vector2 position, List<(TimeSpan timeSpan, Rectangle rect)> frames, int health, int speed, bool isVisible, Color color, Vector2 scale, Texture2D pixel = null) 
            : base(texture, position, color, scale, frames, pixel)
        {
            Health = health;
            Speed = speed;
            IsVisible = isVisible;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}

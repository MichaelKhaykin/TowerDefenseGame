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
    public class Enemy : Sprite
    {
        public int Health { get; set; }
        public int Speed { get; set; }

        public Enemy(Texture2D texture, Vector2 position, int health, int speed, bool isVisible, Color color, Vector2 scale, Texture2D pixel = null) : base(texture, position, color, scale, pixel)
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

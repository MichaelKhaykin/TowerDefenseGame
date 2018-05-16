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
    public class Tower : Sprite
    {
        public int Range { get; set; }
        public int Damage { get; set; }
        public int Cost { get; set; }

        public Enemy EnemyToHit { get; set; }

        public Arrow Arrow { get; set; }

        public Tower(Texture2D texture, Vector2 position, Color color, Vector2 scale, int range, int damage, int cost, ContentManager content, Texture2D pixel = null)
            : base(texture, position, color, scale, pixel)
        {
            Arrow = new Arrow(content.Load<Texture2D>("Arrow"), new Vector2(position.X, position.Y), Color.White, new Vector2((Main.ScreenScale * Main.SpriteScales["Arrow"]) * 3/4));

            Range = range;
            Damage = damage;
            Cost = cost;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Arrow.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}

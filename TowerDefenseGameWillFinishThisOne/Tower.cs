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
    public class Tower : Sprite
    {
        public int Range { get; set; }
        public int Damage { get; set; }
        public int Cost { get; set; }
        public Enemy EnemyToHit { get; set; }
        public TimeSpan FireRate { get; set; }
        public TimeSpan ElapsedFireRate { get; set; }

        public Tower(Texture2D texture, Vector2 position, Color color, Vector2 scale, int range, int damage, int cost, TimeSpan fireRate, Texture2D pixel = null)
            : base(texture, position, color, scale, pixel)
        {
            FireRate = fireRate;
            Range = range;
            Damage = damage;
            Cost = cost;
        }
    }
}

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
    public class Arrow : Sprite
    {
        public Enemy Target;

        public Arrow(Texture2D texture, Vector2 position, Color color, Vector2 scale, Texture2D pixel = null) : base(texture, position, color, scale, pixel)
        {

        }

        public bool HitTarget()
        {    
            if (Target == null)
            {
                return false;
            }

            if (!Target.IsVisible)
            {
                Target = null;
                return false;
            }

            Vector2 diff = Target.Position - Position;
            diff.Normalize();
            Position += diff * 2;

            Rotation = (float)(Math.Atan2(diff.Y, diff.X) + Math.PI / 2);

            if (Vector2.Distance(Position, Target.Position) <= 2f)
            {
                return true;
            }
            return false;
        }
    }
}


using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public static class Extensions
    {

        public static bool VEquals(this Vector2 a, Vector2 b)
        {
            if (a.X.FEquals(b.X) && a.Y.FEquals(b.Y))
            {
                return true;
            }
            return false;
        }

        public static bool FEquals(this float a, float b)
        {
            float deadzone = 0.05f;
            if(Math.Abs(a - b) < deadzone)
            {
                return true;
            }
            return false;
        }

    }
}

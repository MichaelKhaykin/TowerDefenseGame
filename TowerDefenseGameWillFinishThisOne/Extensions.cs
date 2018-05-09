using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public static class Extensions
    {
        public static Vector2 TranslateToOrigin(this Vector2 vectorToTranslate, Vector2 origin)
         => vectorToTranslate - origin;

        public static Vector2 TranslateFromOrigin(this Vector2 vectorToTranslate, Vector2 origin)
         => vectorToTranslate + origin;

        public static float ToAngle(this Vector2 vector)
         => (float)Math.Atan2(-vector.Y, vector.X);

        public static Vector2 ToVector(this float floatToBeVector)
           => new Vector2((float)Math.Cos(floatToBeVector), (float)-Math.Sin(floatToBeVector));

        public static Vector2 ToVector(this float floatToBeVector, float radius)
            => floatToBeVector.ToVector() * radius;

        public static Vector2 ToVector(this Angle angle)
         => new Vector2((float)Math.Cos(angle.value), -(float)Math.Sin(angle.value));

        public static Vector2 ToVector(this Angle angle, float radius)
         => angle.ToVector() * radius;

        public static void DrawLine(this SpriteBatch spriteBatch, Line line, Texture2D pixel, float width, Color color, bool drawEndPoints = false)
        {
            var scale = new Vector2(line.Length, width);
            var position = new Vector2(line.StartX, line.StartY);

            spriteBatch.Draw(pixel, position, null, color, line.Angle, Vector2.Zero, scale, SpriteEffects.None, 0);

            if (drawEndPoints)
            {
                spriteBatch.Draw(pixel, new Rectangle((int)line.StartX, (int)line.StartY, 3, 3), Color.Yellow);
                spriteBatch.Draw(pixel, new Rectangle((int)line.EndX, (int)line.EndY, 3, 3), Color.Yellow);
            }
        }

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

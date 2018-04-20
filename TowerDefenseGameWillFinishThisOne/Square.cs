using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Square
    {
        Line[] Line = new Line[4];
        Texture2D Pixel;
        public Rectangle Rectangle { get; set; }
        public Vector2 Center => new Vector2(Rectangle.X + Rectangle.Width / 2, Rectangle.Y + Rectangle.Height / 2);

        public Square(Line[] line, Texture2D pixel)
        {
            Line = line;
            Pixel = pixel;

            //Line[0] is the top line, Line[1] is the bottom line, Line[2] is the left line, Left[3] is the right line

            int width = (int)(Line[0].EndX - Line[0].StartX);
            int height = (int)(Line[2].EndY - Line[2].StartY);
            int x = (int)(Line[0].StartX + width);
            int y = (int)(Line[0].StartY + height);

            //Origin for rectangle is top left corner
            Rectangle = new Rectangle(x, y, width, height); 
        }

        public Square(Rectangle rect, Texture2D pixel)
        {
            Pixel = pixel;
            Rectangle = rect;

            Line[0] = new Line(new Vector2(rect.X, rect.Y), new Vector2(rect.X + rect.Width, rect.Y));
            Line[1] = new Line(new Vector2(rect.X, rect.Y + rect.Height), new Vector2(rect.X + rect.Width, rect.Y + rect.Height));
            Line[2] = new Line(new Vector2(rect.X, rect.Y), new Vector2(rect.X, rect.Y + rect.Height));
            Line[3] = new Line(new Vector2(rect.X + rect.Width, rect.Y), new Vector2(rect.X + rect.Width, rect.Y + rect.Height));
        }

        public bool Contains(Vector2 position)
        {
            if (Rectangle.Contains(position))
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Line.Length; i++)
            {
                Extensions.DrawLine(spriteBatch, Line[i], Pixel, 1, Color.Red);
            }
        }
    }
}

using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGame
{
    public class Grid
    {
        public Square[] Squares { get; set; }
        public Rectangle Bounds { get; set; }

        public Grid(int amountOfSquares, int modByNumber, int width, int height, Texture2D pixel)
        {
            if (amountOfSquares % modByNumber != 0)
            {
                throw new ArgumentException();
            }
            Squares = new Square[amountOfSquares];
            
            int x = width * 3 + width / 2;
            int y = height * 3 + height / 2;
            for (int i = 0; i < Squares.Length; i++)
            {
                if (i % 15 == 0 && i != 0)
                {
                    x = width * 3 + width / 2;
                    y += height;
                }
                Squares[i] = new Square(new Rectangle(x, y, width, height), pixel);
                x += width;
            }

            Bounds = new Rectangle(Squares[0].Rectangle.X, Squares[0].Rectangle.Y, Squares[0].Rectangle.Width * modByNumber, Squares[0].Rectangle.Height * amountOfSquares / modByNumber);
        }

        public void DrawGrid(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Squares.Length; i++)
            {
                Squares[i].Draw(spriteBatch);
            }
        }
    }
}
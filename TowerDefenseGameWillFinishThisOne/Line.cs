using MichaelLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseGameWillFinishThisOne
{
    public struct Line
    {
        public float StartX { get; private set; }
        public float StartY { get; private set; }

        public float EndX { get; private set; }
        public float EndY { get; private set; }

        public float Length { get; private set; }
        public float Angle { get; private set; }

        public Line(float startX, float startY, float endX, float endY)
        {
            //In RobotC this ctor turns into: 
            //CreateLine(float startX, float startY, float endX, float endY, Line& line);

            //Length of the line: a*a + b*b = c*c
            var a = endY - startY;
            var b = endX - startX;
            var cSquared = a * a + b * b;
            var c = (float)Math.Sqrt(cSquared);

            //Angle (aka theta) of the line;
            var theta = (float)Math.Atan2(a, b);

            //Save data to public properties
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            Angle = theta;
            Length = c;
        }

        public Line(Vector2 startingPosition, Vector2 endingPosition)
            : this(startingPosition.X, startingPosition.Y, endingPosition.X, endingPosition.Y)
        {
            //Pass-through constructor
        }
    }
}

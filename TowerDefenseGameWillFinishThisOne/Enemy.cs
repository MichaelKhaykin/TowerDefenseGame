using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Enemy : AnimationSprite
    {
        public int ID { get; set; }

        public int Health { get; set; }
        public Texture2D HealthBox { get; set; }
        public int Speed { get; set; }
        public Vector2 OldPos { get; set; }

        //private HealthBarWidth healthBarWidth = new HealthBarWidth();
        public int HealthBarWidth = 0;

        public Stack<TileInfo> Path = null;

        private TileInfo currentTile;
        public ref TileInfo CurrentTile => ref currentTile;

        public TileInfo PreviousTile { get; set; }
        public TileInfo NextTile { get; set; }

        public Vector2 CurrentStartPoint { get; set; }
        public Vector2 CurrentEndPoint { get; set; }

        public float TravelPercentage { get; set; } = 0f;
        public int CurrentPointIndex { get; set; } = 0;

        public Enemy(Texture2D texture, Vector2 position, List<(TimeSpan timeSpan, Rectangle rect)> frames, int health, int speed, bool isVisible, Color color, Vector2 scale, GraphicsDevice graphics, Texture2D pixel = null)
            : base(texture, position, color, scale, frames, pixel)
        {
            //healthBarWidth.HealthBoxWidth = HitBox.Width;
            HealthBarWidth = HitBox.Width;

            HealthBox = new Texture2D(graphics, 1, 1);
            HealthBox.SetData(new Color[] { Color.Green });

            Health = health;
            Speed = speed;
            IsVisible = isVisible;
        }

        ConnectionTypes GetOppisite(ConnectionTypes type)
        {
            if (type == ConnectionTypes.Right)
            {
                return ConnectionTypes.Left;
            }
            else if (type == ConnectionTypes.Left)
            {
                return ConnectionTypes.Right;
            }
            else if (type == ConnectionTypes.Top)
            {
                return ConnectionTypes.Bottom;
            }
            else if (type == ConnectionTypes.Bottom)
            {
                return ConnectionTypes.Top;
            }
            return default;
        }

        public void MoveAcrossTile()
        {
            //MAKE THIS A FUINCTIOn

            if (!CurrentTile.IsPathPositionListConfigured)
            {
                CurrentTile.IsPathPositionListConfigured = true;

                switch (CurrentTile.TileApproachedFrom)
                {
                    case ConnectionTypes.Bottom:
                        if (CurrentTile.PathPositions.Last().Y > CurrentTile.PathPositions.First().Y)
                        {
                            CurrentTile.ReversePathPositions();
                        }
                        break;

                    case ConnectionTypes.Top:
                        if (CurrentTile.PathPositions.Last().Y < CurrentTile.PathPositions.First().Y)
                        {
                            CurrentTile.ReversePathPositions();
                        }
                        break;

                    case ConnectionTypes.Left:
                        if (CurrentTile.PathPositions.Last().X < CurrentTile.PathPositions.First().X)
                        {
                            CurrentTile.ReversePathPositions();
                        }
                        break;

                    case ConnectionTypes.Right:
                        if (CurrentTile.PathPositions.Last().X > CurrentTile.PathPositions.First().X)
                        {
                            CurrentTile.ReversePathPositions();
                        }
                        break;
                }
                IsVisible = true;
            }


            CurrentStartPoint = CurrentTile.PathPositions[CurrentPointIndex - 1] + CurrentTile.Position;
            CurrentEndPoint = CurrentTile.PathPositions[CurrentPointIndex] + CurrentTile.Position;

            TravelPercentage += 0.01f * CurrentTile.PathPositions.Count;
            Position = Vector2.Lerp(CurrentStartPoint, CurrentEndPoint, TravelPercentage);

            if (TravelPercentage >= 1)
            {
                TravelPercentage = 0;
                CurrentPointIndex++;

                if (CurrentPointIndex == CurrentTile.PathPositions.Count)
                {
                    if (Path.Count > 0)
                    {
                        PreviousTile = Path.Peek();
                        Path.Pop();
                    }
                    if (Path.Count != 0)
                    {
                        CurrentTile = Path.Peek();

                        //Have to check if path.count is greater than 0 again because we just popped
                        var temp = Path.Pop();
                        if (Path.Count > 0)
                        {
                            NextTile = Path.Peek();
                            Path.Push(temp);
                        }
                        if (CurrentTile.TileName == "RoadPieces/4SideCrossPiece")
                        {
                            var DirectionFrom = CurrentTile.TileApproachedFrom;
                            var DirectionTo = GetOppisite(NextTile.TileApproachedFrom);

                            var CorrectPieceName = "Paths/RoadPieces/" + FindPeiceName(DirectionTo, DirectionFrom) + ".json";
                            
                            CurrentTile.PathPositions = JsonConvert.DeserializeObject<List<Vector2>>(System.IO.File.ReadAllText(CorrectPieceName));
                        }

                        if (!CurrentTile.IsPathPositionListConfigured)
                        {
                            CurrentTile.IsPathPositionListConfigured = true;

                            switch (CurrentTile.TileApproachedFrom)
                            {
                                case ConnectionTypes.Bottom:
                                    if (CurrentTile.PathPositions.Last().Y > CurrentTile.PathPositions.First().Y)
                                    {
                                        CurrentTile.ReversePathPositions();
                                    }
                                    break;

                                case ConnectionTypes.Top:
                                    if (CurrentTile.PathPositions.Last().Y < CurrentTile.PathPositions.First().Y)
                                    {
                                        CurrentTile.ReversePathPositions();
                                    }
                                    break;

                                case ConnectionTypes.Left:
                                    if (CurrentTile.PathPositions.Last().X < CurrentTile.PathPositions.First().X)
                                    {
                                        CurrentTile.ReversePathPositions();
                                    }
                                    break;

                                case ConnectionTypes.Right:
                                    if (CurrentTile.PathPositions.Last().X > CurrentTile.PathPositions.First().X)
                                    {
                                        CurrentTile.ReversePathPositions();
                                    }
                                    break;
                            }
                            IsVisible = true;
                        }


                        CurrentPointIndex = 1;
                    }
                    else
                    {
                        IsVisible = false;

                        GameScreen.troopCrossedCounter++;
                        if (GameScreen.troopCrossedCounter > 10)
                        {
                            Main.CurrentScreen = ScreenStates.LoseScreen;
                            Main.ScreenCameFrom = ScreenStates.Game;
                        }
                    }
                }

                if (Path.Count > 0)
                {
                    CurrentStartPoint = CurrentTile.PathPositions[CurrentPointIndex - 1] + CurrentTile.Position;
                    CurrentEndPoint = CurrentTile.PathPositions[CurrentPointIndex] + CurrentTile.Position;
                }
            }

        }

        string FindPeiceName(ConnectionTypes directionTo, ConnectionTypes directionFrom)
        {
            //Direction came from bottom
            if ((directionTo == ConnectionTypes.Right && directionFrom == ConnectionTypes.Bottom)
                || (directionTo == ConnectionTypes.Bottom && directionFrom == ConnectionTypes.Right))
            {
                return "RightUpArcPiece";
            }
            else if ((directionTo == ConnectionTypes.Left && directionFrom == ConnectionTypes.Bottom)
                || (directionTo == ConnectionTypes.Bottom && directionFrom == ConnectionTypes.Left))
            {
                return "LeftUpArcPiece";
            }
            else if ((directionTo == ConnectionTypes.Top && directionFrom == ConnectionTypes.Bottom)
                || directionTo == ConnectionTypes.Bottom && directionFrom == ConnectionTypes.Top)
            {
                return "StraightVerticalPiece";
            }

            else if ((directionTo == ConnectionTypes.Right && directionFrom == ConnectionTypes.Top)
                || (directionTo == ConnectionTypes.Top && directionFrom == ConnectionTypes.Right))
            {
                return "RightDownArcPiece";
            }

            else if ((directionTo == ConnectionTypes.Left && directionFrom == ConnectionTypes.Top)
                || (directionTo == ConnectionTypes.Top && directionFrom == ConnectionTypes.Left))
            {
                return "LeftDownArcPiece";
            }

            else if ((directionTo == ConnectionTypes.Right && directionFrom == ConnectionTypes.Left)
                || directionTo == ConnectionTypes.Left && directionFrom == ConnectionTypes.Right)
            {
                return "StraightHorizontalPiece";
            }

            return default;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
           spriteBatch.Draw(HealthBox, new Rectangle((int)(Position.X - HitBox.Width / 2), (int)(Position.Y - HitBox.Height - 4), HealthBarWidth, 5), Color.White);


            if (IsVisible)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}

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
    public class Enemy : AnimationSprite
    {
        public int ID { get; set; }

        public int Health { get; set; }
        public int Speed { get; set; }
        public Vector2 OldPos { get; set; }

        public Stack<TileInfo> Path = null;

        private TileInfo currentTile;
        public ref TileInfo CurrentTile => ref currentTile;
        
        public TileInfo PreviousTile { get; set; }

        public Vector2 CurrentStartPoint { get; set; }
        public Vector2 CurrentEndPoint { get; set; }

        public float TravelPercentage { get; set; } = 0f;
        public int CurrentPointIndex { get; set; } = 0;

        public Enemy(Texture2D texture, Vector2 position, List<(TimeSpan timeSpan, Rectangle rect)> frames, int health, int speed, bool isVisible, Color color, Vector2 scale, Texture2D pixel = null) 
            : base(texture, position, color, scale, frames, pixel)
        {
            Health = health;
            Speed = speed;
            IsVisible = isVisible;
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
                        if (ID == 1)
                        {
                            ;
                        }

                        if (ID == 2)
                        {
                            ;
                        }

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

                    case ConnectionTypes.None:

                        break;
                }
            }

            TravelPercentage += 0.01f * CurrentTile.PathPositions.Count;
            Position = Vector2.Lerp(CurrentStartPoint, CurrentEndPoint, TravelPercentage);

            if (TravelPercentage >= 1)
            {
                TravelPercentage = 0;
                CurrentPointIndex++;
            
                if (CurrentPointIndex == CurrentTile.PathPositions.Count)
                {
                    PreviousTile = Path.Peek();
                    Path.Pop();

                    if (Path.Count != 0)
                    {
                        CurrentTile = Path.Peek();
                        if (!CurrentTile.IsPathPositionListConfigured)
                        {
                            CurrentTile.IsPathPositionListConfigured = true;

                            switch (CurrentTile.TileApproachedFrom)
                            {
                                case ConnectionTypes.Bottom:
                                    if (ID == 1)
                                    {
                                        ;
                                    }

                                    if (ID == 2)
                                    {
                                        ;
                                    }

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

                                case ConnectionTypes.None:

                                    break;
                            }
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}

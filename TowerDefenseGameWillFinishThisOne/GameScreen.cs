using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;

namespace TowerDefenseGameWillFinishThisOne
{
    public class GameScreen : Screen
    {
        Rectangle temprect;
        Rectangle temprect1;

        bool shouldDrawGridLines = true;

        Vertex<Tile, ConnectionTypes> startingVertex;
        Vertex<Tile, ConnectionTypes> endingVertex;

        Texture2D texture;
        Rectangle RectAngleToDrawAt;

        TimeSpan timeToCross = new TimeSpan(0, 0, 0, 0, 1000);
        TimeSpan elpasedTimeToCross = new TimeSpan();

        //List<Enemy> Troops = new List<Enemy>();
        Enemy troop;

        Vector2 initTroopPosition;
        Vector2 positionToMoveTo;
        float travelPercentage;

        
        Grid grid;

        int counter = 0;

        Vertex<Tile, ConnectionTypes> current;

        List<(TimeSpan timeSpan, Rectangle rect)> frames = new List<(TimeSpan timeSpan, Rectangle rect)>();

        Tower temp;

        TroopMovingStates troopMovingStates = TroopMovingStates.AtEndOfTile;

        Stack<Vertex<Tile, ConnectionTypes>> path = null;//= new Stack<Vertex<Tile, ConnectionTypes>>();

        Texture2D pixel1;

        Tile[,] TilesArray;

        Tile tempTile;

        KeyboardState oldkeyboard;

        Button LoadButton;

        ContentManager contentManager;

        Texture2D pixel;

        GraphicsDevice graphics;

        bool hasFinishedPlacingTower = true;
        List<Tower> Towers = new List<Tower>();
        List<Button> TowerButtons = new List<Button>();

        public GameScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            contentManager = content;
            this.graphics = graphics;

            SetUpFrames();

            pixel1 = new Texture2D(graphics, 1, 1);
            pixel1.SetData(new Color[] { Color.White });

            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            tempTile = new Tile(new TileInfo("RoadPieces/RightUpArcPiece", new Vector2(-10, -10), null), content, "RoadPieces/RightUpArcPiece");

            Texture2D backgroundBoxTexture = Content.Load<Texture2D>("BackgroundBox");

            var tower1 = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("Towers/Tower1"), new Vector2(graphics.Viewport.Width / 2 + 5445, graphics.Viewport.Height / 2 + 400) * Main.ScreenScale * Main.SpriteScales["Towers/Tower1"]);
            var tower2 = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("Towers/Tower2"), new Vector2(tower1.Position.X, tower1.Y - tower1.ScaledHeight));
            var tower3 = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("Towers/Tower3"), new Vector2(tower2.Position.X, tower2.Y - tower2.ScaledHeight));

            tower1.Tag = new Tower(Content.Load<Texture2D>("Towers/Tower1"), new Vector2(-100, -100), Color.White, new Vector2(0.3f), 2, 100, 5);
            tower2.Tag = new Tower(Content.Load<Texture2D>("Towers/Tower2"), new Vector2(-100, -100), Color.White, new Vector2(0.3f), 10, 300, 15);
            tower3.Tag = new Tower(Content.Load<Texture2D>("Towers/Tower3"), new Vector2(-100, -100), Color.White, new Vector2(0.3f), 6, 200, 10);

            TowerButtons.Add(tower1);
            TowerButtons.Add(tower2);
            TowerButtons.Add(tower3);

            LoadButton = new Button(Content.Load<Texture2D>("LoadButton"), new Vector2(graphics.Viewport.Width / 2 * Main.SpriteScales["LoadButton"] * Main.ScreenScale, graphics.Viewport.Height / 2 * Main.SpriteScales["LoadButton"] * Main.ScreenScale), Color.White, new Vector2(Main.SpriteScales["LoadButton"] * Main.ScreenScale), null);

            int gridWidth = 15;
            grid = new Grid(75, gridWidth, tempTile.HitBox.Width, tempTile.HitBox.Height, pixel);

            TilesArray = new Tile[gridWidth, 75 / gridWidth];

            Sprites.Add(LoadButton);
            Sprites.Add(tower1);
            Sprites.Add(tower2);
            Sprites.Add(tower3);


            Sprites.Add((BaseSprite)tower1.Tag);
            Sprites.Add((BaseSprite)tower2.Tag);
            Sprites.Add((BaseSprite)tower3.Tag);
        }

        private void SetUpFrames()
        {
            var t = new TimeSpan(0, 0, 0, 0, 175);
            frames.Add((t, new Rectangle(0, 0, 522, 652)));
            frames.Add((t, new Rectangle(520, -3, 491, 650)));
            frames.Add((t, new Rectangle(0, 649, 521, 652)));
            frames.Add((t, new Rectangle(522, 652, 488, 647)));
            frames.Add((t, new Rectangle(1014, 650, 465, 645)));
        }

        private void DrawGridLines(SpriteBatch spriteBatch, int TileWidth, int TileHeight)
        {
            Texture2D pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            for (int i = TileWidth / 2; i <= TilesArray.GetLength(0) * TileWidth; i += TileWidth)
            {
                Line line = new Line(new Vector2(i, 0), new Vector2(i, graphics.Viewport.Height));
                Extensions.DrawLine(spriteBatch, line, pixel, 1, Color.Red, false);
            }
            for (int i = TileHeight / 2; i <= TilesArray.GetLength(1) * TileHeight; i += TileHeight)
            {
                Line line = new Line(new Vector2(0, i), new Vector2(graphics.Viewport.Width, i));
                Extensions.DrawLine(spriteBatch, line, pixel, 1, Color.Red, false);
            }

        }

        private void PlaceTower(Tower tower)
        {
            tower.IsVisible = false;
            for (int i = 0; i < grid.Squares.Length; i++)
            {
                if (grid.Squares[i].Contains(new Vector2(Main.mouse.X, Main.mouse.Y)))
                {
                    tower.IsVisible = true;
                    tower.Position = grid.Squares[i].Center;
                }
            }
            if (Main.mouse.LeftButton == ButtonState.Released && !(Main.oldMouse.LeftButton == ButtonState.Pressed))
            {
                hasFinishedPlacingTower = true;
            }
        }
        
        public override void Update(GameTime gameTime)
        {
            elpasedTimeToCross += gameTime.ElapsedGameTime;

            switch (troopMovingStates)
            {
                case TroopMovingStates.CrossingTile:
                    //  troop.Position = new Vector2((float)current.Point.X, (float)current.Point.Y + 50);

                    //float count = (positionToMoveTo.X - initTroopPosition) 

                    travelPercentage += 0.01f;

                    bool isStraightPiece = (initTroopPosition.X == positionToMoveTo.X) || (initTroopPosition.Y == positionToMoveTo.Y);
                    if (isStraightPiece)
                    {
                        troop.Position = new Vector2(MathHelper.Lerp(initTroopPosition.X, positionToMoveTo.X, travelPercentage), MathHelper.Lerp(initTroopPosition.Y, positionToMoveTo.Y, travelPercentage));
                    }
                    else
                    {
                        if (positionToMoveTo.X > initTroopPosition.X && positionToMoveTo.Y < initTroopPosition.Y)
                        {
                            //Going to the right up
                            float angle = MathHelper.Lerp(MathHelper.Pi, 3 * MathHelper.PiOver2, travelPercentage);
                            troop.Position = new Vector2((float)(Math.Cos(angle) * tempTile.ScaledWidth / 2 + positionToMoveTo.X), (float)(Math.Sin(angle) * tempTile.ScaledHeight / 2 + initTroopPosition.Y));

                        }
                        else if (positionToMoveTo.X < initTroopPosition.X && positionToMoveTo.Y < initTroopPosition.Y)
                        {
                            //Going to the right down
                            float angle = MathHelper.Lerp(MathHelper.Pi, MathHelper.PiOver2, travelPercentage);
                            troop.Position = new Vector2((float)(Math.Cos(angle) * tempTile.ScaledWidth / 2 + positionToMoveTo.X), (float)(Math.Sin(angle) * tempTile.ScaledHeight / 2 + initTroopPosition.Y));
                        }
                        else if (positionToMoveTo.X < initTroopPosition.X && positionToMoveTo.Y < initTroopPosition.Y)
                        {
                            //Going to the left up
                            //float angle = MathHelper.Lerp(MathHelper.Pi, 3 * MathHelper.PiOver2, travelPercentage);
                            //troop.Position = new Vector2((float)(Math.Cos(angle) * tempTile.ScaledWidth / 2 + positionToMoveTo.X), (float)(Math.Sin(angle) * tempTile.ScaledHeight / 2 + initTroopPosition.Y));

                        }
                        else if (positionToMoveTo.X < initTroopPosition.X && positionToMoveTo.Y > initTroopPosition.Y)
                        {
                            //float angle = MathHelper.Lerp(MathHelper.Pi, 3 * MathHelper.PiOver2, travelPercentage);
                            //troop.Position = new Vector2((float)(Math.Cos(angle) * tempTile.ScaledWidth / 2 + positionToMoveTo.X), (float)(Math.Sin(angle) * tempTile.ScaledHeight / 2 + initTroopPosition.Y));

                            //Going to the left down

                        }
                    }

                    if (travelPercentage >= 1f)
                    {
                        troopMovingStates = TroopMovingStates.AtEndOfTile;
                        troop.Position = positionToMoveTo;
                    }

                 break;

                case TroopMovingStates.AtEndOfTile:
                    if (path != null && path.Count != 0)
                    {
                        current = path.Pop();
                        initTroopPosition = troop.Position;

                        temprect = new Rectangle((int)initTroopPosition.X, (int)initTroopPosition.Y, 10, 10);

                        travelPercentage = 0;

                        //float amountToMove = current.Value.X + current.Value.ScaledWidth - current.Value.X;
                        positionToMoveTo = current.Value.Position;

                        temprect1 = new Rectangle((int)positionToMoveTo.X, (int)positionToMoveTo.Y, 10, 10);

                        troopMovingStates = TroopMovingStates.CrossingTile;
                    }
                    break;
            }

            KeyboardState keyboard = Keyboard.GetState();

            counter = 0;
            for (int i = 0; i < grid.Squares.Length; i++)
            {
                if (grid.Squares[i].Contains(new Vector2(Main.mouse.X, Main.mouse.Y)))
                {
                    counter++;

                    texture = new Texture2D(graphics, grid.Squares[i].Rectangle.Width - 1, grid.Squares[i].Rectangle.Height + 1);
                    RectAngleToDrawAt = grid.Squares[i].Rectangle;

                    Color[] data = new Color[texture.Width * texture.Height];

                    texture.GetData(data);

                    for (int j = 0; j < data.Length; j++)
                    {
                        data[j] = Color.LightSteelBlue;
                    }

                    texture.SetData(data);
                }
            }

            if (counter == 0)
            {
                texture = null;
                RectAngleToDrawAt = new Rectangle(-10, -10, 0, 0);
            }

            if (keyboard.IsKeyDown(Keys.G) && oldkeyboard.IsKeyUp(Keys.G))
            {
                shouldDrawGridLines = !shouldDrawGridLines;
            }

            if (LoadButton.IsClicked(Main.mouse) && !LoadButton.IsClicked(Main.oldMouse))
            {
                Sprites.Remove(LoadButton);

                var serializedInfo = System.IO.File.ReadAllText("NamesAndPositions.json");
                MakeMapScreen.TilesGraph = JsonConvert.DeserializeObject<Graph<Tile, ConnectionTypes>>(serializedInfo);

                foreach (var vertex in MakeMapScreen.TilesGraph.Vertices)
                {
                    if (vertex.Value.IsStartingTile)
                    {
                        startingVertex = vertex;
                    }
                    else if (vertex.Value.IsEndingTile)
                    {
                        endingVertex = vertex;
                    }

                    vertex.Value.Texture = contentManager.Load<Texture2D>(vertex.Value.Name);
                    (int x, int y) = ((int)vertex.Value.GridPosition.X, (int)vertex.Value.GridPosition.Y);
                    TilesArray[x, y] = vertex.Value;
                    vertex.Value.Scale = new Vector2(Main.ScreenScale * Main.SpriteScales[vertex.Value.Name]);
                    vertex.Point = new Pointy { X = vertex.Value.Position.X, Y = vertex.Value.Position.Y };
                    //Position calculation needs to be re-done

                    // z -> x,y
                    // z % width = x
                    // z / width = y

                    int index = y * TilesArray.GetLength(0) + x;
                    vertex.Value.Position = grid.Squares[index].Center;

                    Sprites.Add(vertex.Value);
                }

                if (startingVertex != null && endingVertex != null)
                {
                    troop = new Enemy(Content.Load<Texture2D>("sprites"), new Vector2(startingVertex.Value.Position.X - startingVertex.Value.ScaledWidth, startingVertex.Value.Position.Y), frames, 100, 1, true, Color.White, new Vector2(0.05f));
                    Sprites.Add(troop);

                    path = MakeMapScreen.TilesGraph.AStar(startingVertex, endingVertex);
                }
            }

            foreach (var towerbutton in TowerButtons)
            {
                if (towerbutton.IsClicked(Main.mouse) && !towerbutton.IsClicked(Main.oldMouse))
                {
                    var tag = (Tower)towerbutton.Tag;
                    temp = new Tower(tag.Texture, tag.Position, Color.White, tag.Scale, tag.Range, tag.Damage, tag.Cost);
                    Sprites.Add(temp);
                    Towers.Add(temp);
                    hasFinishedPlacingTower = false;
                }
            }

            if (hasFinishedPlacingTower == false)
            {
                PlaceTower(temp);
            }

            oldkeyboard = keyboard;

            base.Update(gameTime);
        }

        private (int x, int y) Index(Tile tile)
        {
            return (Map((int)tile.Position.X, 0, graphics.Viewport.Width, 0, TilesArray.GetLength(0)), Map((int)tile.Position.Y, 0, graphics.Viewport.Height, 0, TilesArray.GetLength(1)));
            //return ((int)(graphics.Viewport.Width / tile.Position.X), (int)(graphics.Viewport.Height / tile.Position.Y));
        }

        private int Map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (shouldDrawGridLines)
            {
                if (texture != null && RectAngleToDrawAt != null)
                {
                    spriteBatch.Draw(texture, RectAngleToDrawAt, Color.White);
                }
                grid.DrawGrid(spriteBatch);
                //DrawGridLines(spriteBatch, tempTile.HitBox.Width, tempTile.HitBox.Height);
            }
            base.Draw(spriteBatch);
            if (temprect != null)
            {
                spriteBatch.Draw(pixel1, temprect, Color.Yellow);
                spriteBatch.Draw(pixel1, temprect1, Color.Yellow);
            }


        }
    }
}

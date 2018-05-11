using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        bool shouldDrawGridLines = true;

        Vertex<Tile, ConnectionTypes> startingVertex;
        Vertex<Tile, ConnectionTypes> endingVertex;

        Texture2D texture;
        Rectangle RectAngleToDrawAt;

        TimeSpan timeToCross = new TimeSpan(0, 0, 0, 0, 1000);
        TimeSpan elpasedTimeToCross = new TimeSpan();

        //List<Enemy> Troops = new List<Enemy>();
        List<Enemy> troops = new List<Enemy>();

        Grid grid;

        int counter = 0;

        List<(TimeSpan timeSpan, Rectangle rect)> frames = new List<(TimeSpan timeSpan, Rectangle rect)>();

        Stopwatch timeForSpawningNewEnemy = new Stopwatch();

        public static int troopCrossedCounter = 0;

        Tower temp;

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

        private Vector2 Turn(Vector2 origin, Vector2 start, Vector2 end, float radius, float travelPercent)
        {
            Angle startAngle = start.TranslateToOrigin(origin).ToAngle();
            Angle endAngle = end.TranslateToOrigin(origin).ToAngle();

            //Calculate fastest path
            var angle = endAngle - startAngle;
            //if angle is greater than 180 there's obviously a faster way
            if (angle > Math.PI)
            {
                //Get different angle for faster path
                angle = startAngle - endAngle;
                //Get angle to be a non negative number that represents the same thing (as in it will be the same getting the cos or sin of it)
                angle += (float)Math.PI;
            }

            angle *= travelPercent;
            angle += startAngle;

            return angle.ToVector(radius).TranslateFromOrigin(origin);
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

        private void AddTroop(Enemy enemy)
        {
            enemy.Path = MakeMapScreen.TilesGraph.AStar(startingVertex, endingVertex);
            enemy.PreviousTile = enemy.Path.Peek();
            enemy.CurrentTile = enemy.Path.Peek();
            enemy.CurrentPointIndex = 1;
            enemy.CurrentStartPoint = enemy.CurrentTile.PathPositions[0] + enemy.Path.Peek().Position;
            enemy.CurrentEndPoint = enemy.CurrentTile.PathPositions[1] + enemy.Path.Peek().Position;

            troops.Add(enemy);
            enemy.ID = troops.Count;

            Sprites.Add(enemy);
        }

        public override void Update(GameTime gameTime)
        {
            elpasedTimeToCross += gameTime.ElapsedGameTime;
            for (int i = 0; i < troops.Count; i++)
            {
                if (troops[i] != null)
                {
                    troops[i].OldPos = troops[i].Position;
                }

                if (troops[i] != null)
                {
                    troops[i].MoveAcrossTile();
                }
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
                LoadButton.IsVisible = false;

                var serializedInfo = System.IO.File.ReadAllText("NamesAndPositions.json");
                MakeMapScreen.TilesGraph = JsonConvert.DeserializeObject<Graph<Tile, ConnectionTypes>>(serializedInfo);

                // TODO: Vertex.Edges does not properly keep firstVertex or secondVertex references

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

                    string jPath = "Paths/" + vertex.Value.Name + ".json";
                    vertex.Value.PathPositions = JsonConvert.DeserializeObject<List<Vector2>>(System.IO.File.ReadAllText(jPath));

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
                    var enemy = new Enemy(Content.Load<Texture2D>("sprites"), new Vector2(startingVertex.Value.Position.X - startingVertex.Value.ScaledWidth / 2, startingVertex.Value.Position.Y), frames, 100, 1, true, Color.White, new Vector2(0.05f));
                    AddTroop(enemy);

                    timeForSpawningNewEnemy.Start();
                }
            }

            if (timeForSpawningNewEnemy.ElapsedMilliseconds > 3000)
            {
                var enemy = new Enemy(Content.Load<Texture2D>("sprites"), new Vector2(startingVertex.Value.Position.X - startingVertex.Value.ScaledWidth / 2, startingVertex.Value.Position.Y), frames, 100, 1, true, Color.White, new Vector2(0.05f));
                AddTroop(enemy);

                timeForSpawningNewEnemy.Restart();
            }

            if (troops.Count > 0)
            {
                for (int i = 0; i < troops.Count; i++)
                {
                    troops[i].SpriteEffects = troops[i].OldPos.X <= troops[i].Position.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
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

        private (int row, int col) GridIndex(Tile tile)
        {
            for (int i = 0; i < grid.Squares.Length; i++)
            {
                if (grid.Squares[i].Contains(tile.Position))
                {
                    return (i % TilesArray.GetLength(0), i / TilesArray.GetLength(0));
                }
            }
            return (-10, -10);
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
        }
    }
}

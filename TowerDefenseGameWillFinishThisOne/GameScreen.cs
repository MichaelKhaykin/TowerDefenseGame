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
        bool shouldDrawGridLines = true;

        Grid grid;

        Tower temp;

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
            if (Main.mouse.LeftButton == ButtonState.Released && !(Main.oldMouse.LeftButton == ButtonState.Pressed))
            {
                hasFinishedPlacingTower = true;
            }
            tower.Position = new Vector2((int)(Main.mouse.X / ((tower.ScaledWidth))) * ((tower.ScaledWidth)), (int)(Main.mouse.Y / ((tower.ScaledHeight))) * ((tower.ScaledHeight)));
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

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
                    vertex.Value.Texture = contentManager.Load<Texture2D>(vertex.Value.Name);
                    (int x, int y) = ((int)vertex.Value.GridPosition.X, (int)vertex.Value.GridPosition.Y);
                    TilesArray[x, y] = vertex.Value;
                    vertex.Value.Scale = new Vector2(Main.ScreenScale * Main.SpriteScales[vertex.Value.Name]);
                    //Position calculation needs to be re-done

                    // z -> x,y
                    // z % width = x
                    // z / width = y

                    int index = y * TilesArray.GetLength(0) + x;
                    vertex.Value.Position = grid.Squares[index].Center;

                    Sprites.Add(vertex.Value);
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
                grid.DrawGrid(spriteBatch);
                //DrawGridLines(spriteBatch, tempTile.HitBox.Width, tempTile.HitBox.Height);
            }
             base.Draw(spriteBatch);
        }
    }
}

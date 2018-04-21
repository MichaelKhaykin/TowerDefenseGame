using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MichaelLibrary;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using System;

namespace TowerDefenseGameWillFinishThisOne
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Sprite Background;
        public static Sprite Tint;

        public static MouseState oldMouse;
        public static MouseState mouse;

        public static ScreenStates CurrentScreen = ScreenStates.Title;
        public static ScreenStates ScreenCameFrom = ScreenStates.Title;

        public static Song song;

        Dictionary<ScreenStates, Screen> screens = new Dictionary<ScreenStates, Screen>();

        public static int widthOfCurrentScreen = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int heightOfCurrentScreen = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        public static float ScreenScale;

        public static Dictionary<string, float> SpriteScales = new Dictionary<string, float>();

        Texture2D pixel;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = widthOfCurrentScreen;
            graphics.PreferredBackBufferHeight = heightOfCurrentScreen;
            graphics.ApplyChanges();
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            InitDictionary();

            //900 is the width of my screen and 1080 is the width of the texture (which is the backGround)
            
            Texture2D backgroundTexture = Content.Load<Texture2D>("background");

            //All game assets are scaled to 1920x1080 (16:9 Full HD) screen
            ScreenScale = (float)GraphicsDevice.Viewport.Height / (float)backgroundTexture.Height;

            Background = new Sprite(backgroundTexture, new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White, new Vector2(ScreenScale), null);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            Tint = new Sprite(pixel, Vector2.Zero, Color.Black * 0.45f, new Vector2(Background.Texture.Width, Background.Texture.Height));

       
            screens.Add(ScreenStates.Title, new TitleScreen(GraphicsDevice, Content));
            screens.Add(ScreenStates.Game, new GameScreen(GraphicsDevice, Content));
            screens.Add(ScreenStates.Setting, new SettingsScreen(GraphicsDevice, Content));
            screens.Add(ScreenStates.Rules, new RulesScreen(GraphicsDevice, Content));
            screens.Add(ScreenStates.ChoosePlayOrMakeMap, new ChoosePlayOrMakeMap(GraphicsDevice, Content));
            screens.Add(ScreenStates.MakeMap, new MakeMapScreen(GraphicsDevice, Content));

            song = Content.Load<Song>("MusicForTowerDefense");
            
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            // TODO: use this.Content to load your game content here
        }

        private void InitDictionary()
        {
            SpriteScales.Add("PlayButton", 1f);
            SpriteScales.Add("BackgroundBox", 0.7f);
            SpriteScales.Add("SettingsButton", 1f);
            SpriteScales.Add("RulesButton", 0.5f);
            SpriteScales.Add("Character", 1f);
            SpriteScales.Add("Arrow", 0.4f);
            SpriteScales.Add("MusicPlayButton", 1f);
            SpriteScales.Add("BackButton", 0.5f);
            SpriteScales.Add("ChoicesSprite", 0.96f);
            SpriteScales.Add("MakeMapButton", 2f);
            SpriteScales.Add("BattleButton", 1f);
            SpriteScales.Add("RoadPieces/RightUpArcPiece", 0.25f);
            SpriteScales.Add("RoadPieces/RightDownArcPiece", 0.25f);
            SpriteScales.Add("RoadPieces/LeftUpArcPiece", 0.25f);
            SpriteScales.Add("RoadPieces/LeftDownArcPiece", 0.25f);
            SpriteScales.Add("RoadPieces/StraightHorizontalPiece", 0.25f);
            SpriteScales.Add("RoadPieces/StraightVerticalPiece", 0.25f);
            SpriteScales.Add("RoadPieces/4SideCrossPiece", 0.25f);
            SpriteScales.Add("SaveButton", 1f);
            SpriteScales.Add("LoadButton", 2f);
            SpriteScales.Add("MarkStartTile", 0.5f);
            SpriteScales.Add("MarkEndTile", 0.5f);
            SpriteScales.Add("Towers/Tower1", 0.25f);
            SpriteScales.Add("Towers/Tower2", 0.25f);
            SpriteScales.Add("Towers/Tower3", 0.25f);
            SpriteScales.Add("Eraser", 0.3f);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            mouse = Mouse.GetState();
            
            screens[CurrentScreen].Update(gameTime);
            
            oldMouse = mouse;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        public static Button CreateButton(GraphicsDevice graphicsDevice, Texture2D box, Texture2D image, Vector2 position)
        {
            Vector2 boxSize = new Vector2(box.Width, box.Height);
            float boxScale = Main.SpriteScales[box.Name];
            Vector2 scaledBoxSize = boxSize * boxScale;

            Vector2 imageSize = new Vector2(image.Width, image.Height);
            float imageScale = Main.SpriteScales[image.Name];
            Vector2 scaledImageSize = imageSize * imageScale;

            RenderTarget2D renderTarget = new RenderTarget2D(graphicsDevice, (int)scaledBoxSize.X, (int)scaledBoxSize.Y);
            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);


            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();

            spriteBatch.Draw(box, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, boxScale, SpriteEffects.None, 0f);
            spriteBatch.Draw(image, (scaledBoxSize - scaledImageSize) / 2f, null, Color.White, 0f, Vector2.Zero, imageScale, SpriteEffects.None, 0f);

            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);

            return new Button(renderTarget, position, Color.White, new Vector2(Main.ScreenScale), null);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            spriteBatch.Begin();
      
            screens[CurrentScreen].Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

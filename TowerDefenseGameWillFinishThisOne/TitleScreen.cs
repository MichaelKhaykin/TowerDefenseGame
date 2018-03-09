using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TowerDefenseGameWillFinishThisOne
{
    public class TitleScreen : Screen
    {
        Button playButton;
        Button settingsButton;
        Button rulesButton;

        Sprite banner;
        int bannerYPositionToGetTo;

        SpriteFont font;
        
        AnimationSprite character;

        TimeSpan arrowFlyTime = TimeSpan.FromSeconds(2);
        TimeSpan elapsedArrowFlyTime = TimeSpan.Zero;
        TimeSpan timeToBeInvisible = TimeSpan.FromMilliseconds(500);
        TimeSpan elapsedTimeToBeInvisible = TimeSpan.Zero;

        Sprite Arrow;

        List<(TimeSpan timeSpan, Rectangle frame)> frames = new List<(TimeSpan, Rectangle)>();

        private Texture2D CreateGameTitle(GraphicsDevice graphicsDevice, Texture2D titleBanner, string text, SpriteFont font, Color topColor, Color bottomColor)
        {
            RenderTarget2D result = new RenderTarget2D(graphicsDevice, titleBanner.Width, titleBanner.Height);
            SpriteBatch spriteBatch = new SpriteBatch(graphicsDevice);
            graphicsDevice.SetRenderTarget(result);
            graphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();

            spriteBatch.Draw(titleBanner, Vector2.Zero, Color.White);

            Vector2 sizeOfText = font.MeasureString(text);
            Vector2 sizeOfBanner = new Vector2(titleBanner.Width, titleBanner.Height);
            Vector2 positionOfText = (sizeOfBanner - sizeOfText) / 2f;

            spriteBatch.DrawString(font, text, positionOfText, bottomColor);

            spriteBatch.End();  
            graphicsDevice.SetRenderTarget(null);

            var pixels = new Color[result.Width * result.Height];
            result.GetData(pixels);
            
            //Color the top part of the pixels with the topColor passed in
            for (int i = 0; i < pixels.Length / 2 - 1; i++)
            {
                if (pixels[i] == bottomColor)
                {
                    pixels[i] = topColor;
                }
            }

            result.SetData(pixels);
            return result;
        }
        
        public TitleScreen(GraphicsDevice graphics, ContentManager content)
            : base(graphics, content)
        {
            MakeFrames();

            font = Content.Load<SpriteFont>("TitleFont");

            Texture2D bannerTexture = CreateGameTitle(graphics, Content.Load<Texture2D>("Title"), "MICHAEL'S TOWER DEFENSE", font, Color.Red, Color.Black);

            Vector2 offScreenStartingPoint = new Vector2(graphics.Viewport.Width / 2, -10);
            banner = new Sprite(bannerTexture, offScreenStartingPoint, Color.White, new Vector2(Main.ScreenScale));
            bannerYPositionToGetTo = graphics.Viewport.Height / 8;

            Texture2D backgroundBoxTexture = Content.Load<Texture2D>("BackgroundBox");

            playButton = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("PlayButton"), new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2));

            settingsButton = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("SettingsButton"), new Vector2(graphics.Viewport.Width / 2, playButton.Position.Y + playButton.Texture.Height * playButton.Scale.Y));

            rulesButton = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("RulesButton"), new Vector2(graphics.Viewport.Width / 2, settingsButton.Position.Y + settingsButton.Texture.Height * settingsButton.Scale.Y));

            character = new AnimationSprite(Content.Load<Texture2D>("Spritesheet"), new Vector2(300, 100) * Main.ScreenScale, Color.White, new Vector2(Main.SpriteScales["Character"] * Main.ScreenScale), frames, null);

            Arrow = new Sprite(Content.Load<Texture2D>("Arrow"), new Vector2(character.Position.X, character.Position.Y) + new Vector2(20, -25) * Main.ScreenScale, Color.White, new Vector2(Main.SpriteScales["Arrow"] * Main.ScreenScale));
            Arrow.Rotation += MathHelper.ToRadians(90);
            
            Sprites.Add(Main.Background);
            Sprites.Add(character);
            Sprites.Add(Arrow);
            Sprites.Add(Main.Tint);
            Sprites.Add(playButton);
            Sprites.Add(settingsButton);
            Sprites.Add(rulesButton);
            Sprites.Add(banner);
        }

        private void MakeFrames()
        {
            frames.Add((TimeSpan.FromSeconds(1), new Rectangle(153, 1222, 43, 51)));
            frames.Add((TimeSpan.FromMilliseconds(500), new Rectangle(429, 1222, 52, 51)));
            frames.Add((TimeSpan.FromMilliseconds(500), new Rectangle(495, 1222, 44, 51)));
        }

        public override void Update(GameTime gameTime)
        {
            elapsedArrowFlyTime += gameTime.ElapsedGameTime;
            elapsedTimeToBeInvisible += gameTime.ElapsedGameTime;

            if (playButton.IsClicked(Main.mouse) && !playButton.IsClicked(Main.oldMouse) && Main.CurrentScreen == ScreenStates.Title)
            {
                Main.CurrentScreen = ScreenStates.ChoosePlayOrMakeMap;
                Main.ScreenCameFrom = ScreenStates.Title;
            }
            if(settingsButton.IsClicked(Main.mouse) && !settingsButton.IsClicked(Main.oldMouse) && Main.CurrentScreen == ScreenStates.Title)
            {
                Main.CurrentScreen = ScreenStates.Setting;
                Main.ScreenCameFrom = ScreenStates.Title;
            }
            if (rulesButton.IsClicked(Main.mouse) && !settingsButton.IsClicked(Main.oldMouse) && Main.CurrentScreen == ScreenStates.Title)
            {
                Main.CurrentScreen = ScreenStates.Rules;
                Main.ScreenCameFrom = ScreenStates.Title;
            }

            if (character.currentFrameIndex == 0)
            {
                Arrow.Position = new Vector2(character.Position.X, character.Position.Y) + new Vector2(20, -25) * Main.ScreenScale;
            }

            if (elapsedTimeToBeInvisible > timeToBeInvisible)
            {
                Arrow.IsVisible = true;
                elapsedTimeToBeInvisible = TimeSpan.Zero;
            }
            
            if (Arrow.IsVisible)
            {
                Arrow.Position = new Vector2(Arrow.Position.X + 2f * Main.ScreenScale, Arrow.Position.Y);
            }
            if (elapsedArrowFlyTime > arrowFlyTime)
            {
                elapsedArrowFlyTime = TimeSpan.Zero;
                Arrow.IsVisible = false;
            }

            if (banner.Position.Y <= bannerYPositionToGetTo)
            {
                banner.Position = new Vector2(banner.Position.X, banner.Position.Y + 1 * Main.ScreenScale);
            }

            base.Update(gameTime);
        }
    }
}

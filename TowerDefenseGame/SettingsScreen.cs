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
using Microsoft.Xna.Framework.Media;

namespace TowerDefenseGame
{
    public class SettingsScreen : Screen
    {
        Button backButton;
        Button musicPlayButton;
        TimeSpan pausedTimeSpan = TimeSpan.Zero;

        SpriteFont font;
        TextLabel musicLabel;
        
        public SettingsScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            font = Content.Load<SpriteFont>("TextFont");

            Texture2D backgroundBoxTexture = Content.Load<Texture2D>("BackgroundBox");

            musicPlayButton = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("MusicPlayButton"), new Vector2(graphics.Viewport.Width / 2, graphics.Viewport.Height / 2));
            
            Texture2D backButtonTexture = Content.Load<Texture2D>("BackButton");
            backButton = new Button(backButtonTexture, new Vector2(backButtonTexture.Width / 2 * Main.SpriteScales["BackButton"] * Main.ScreenScale, graphics.Viewport.Height - backButtonTexture.Height / 2 * Main.SpriteScales["BackButton"] * Main.ScreenScale), Color.White, new Vector2(Main.SpriteScales["BackButton"] * Main.ScreenScale), null);

            musicLabel = new TextLabel(new Vector2(musicPlayButton.Position.X, musicPlayButton.Position.Y) + new Vector2(70, 1) * Main.ScreenScale, Color.Yellow, " ", font);

            Sprites.Add(Main.Background);
            Sprites.Add(backButton);
            Sprites.Add(musicLabel);
            Sprites.Add(Main.Tint);
            Sprites.Add(musicPlayButton);
        }

        public override void Update(GameTime gameTime)
        {
            musicLabel.Text = MediaPlayer.State.ToString();

            if (musicPlayButton.IsClicked(Main.mouse) && !musicPlayButton.IsClicked(Main.oldMouse) && Main.CurrentScreen == ScreenStates.Setting)
            {
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Pause();
                }
                else if (MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Resume();
                }
            }
            if (backButton.IsClicked(Main.mouse) && !musicPlayButton.IsClicked(Main.oldMouse) && Main.CurrentScreen == ScreenStates.Setting)
            {
                var temp = Main.CurrentScreen;
                Main.CurrentScreen = Main.ScreenCameFrom;
                Main.ScreenCameFrom = temp;
            }

            base.Update(gameTime);
        }

    }
}

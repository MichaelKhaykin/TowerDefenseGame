using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TowerDefenseGame
{
    public class RulesScreen : Screen
    {
        SpriteFont font;
        TextLabel label;

        Button backButton;

        public RulesScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            font = Content.Load<SpriteFont>("TextFont");
            label = new TextLabel(new Vector2(-10, -10), Color.White, "", font);
            
            label = new TextLabel(new Vector2(graphics.Viewport.Width / 2 - graphics.Viewport.Width / 4, graphics.Viewport.Height / 2), new Color(255, 255, 50), "Hello there, here are the rules of the game: You will place towers down on to a field, and will only be able to be placed in certain spots. Once you press the ready button,\n enemies will follow a path and try to get to your castle. Goal: Kill enemies. You will need to generate energy (using farms) and using that energy you can buy a tower\n and then place it down. Have Fun!", font);

            Texture2D backButtonTexture = Content.Load<Texture2D>("BackButton");
            backButton = new Button(backButtonTexture, new Vector2(backButtonTexture.Width / 2 * Main.SpriteScales["BackButton"] * Main.ScreenScale, graphics.Viewport.Height - backButtonTexture.Height / 2 * Main.SpriteScales["BackButton"] * Main.ScreenScale), Color.White, new Vector2(Main.SpriteScales["BackButton"] * Main.ScreenScale), null);

            Sprites.Add(Main.Background);
            Sprites.Add(backButton);
            Sprites.Add(Main.Tint);
            Sprites.Add(label);
        }

        public override void Update(GameTime gameTime)
        {
            if (backButton.IsClicked(Main.mouse) && !backButton.IsClicked(Main.oldMouse))
            {
                var temp = Main.CurrentScreen;
                Main.CurrentScreen = Main.ScreenCameFrom;
                Main.ScreenCameFrom = temp;
            }
            base.Update(gameTime);
        }
    }

}

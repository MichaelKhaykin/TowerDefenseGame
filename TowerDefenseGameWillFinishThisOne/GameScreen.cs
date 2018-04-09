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
        Tower temp;

        Button LoadButton;

        ContentManager contentManager;

        bool hasFinishedPlacingTower = true;

        List<Tower> Towers = new List<Tower>();
        List<Button> TowerButtons = new List<Button>();

        public GameScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            contentManager = content;

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

            Sprites.Add(LoadButton);
            Sprites.Add(tower1);
            Sprites.Add(tower2);
            Sprites.Add(tower3);

            Sprites.Add((BaseSprite)tower1.Tag);
            Sprites.Add((BaseSprite)tower2.Tag);
            Sprites.Add((BaseSprite)tower3.Tag);
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
            if (LoadButton.IsClicked(Main.mouse) && !LoadButton.IsClicked(Main.oldMouse))
            {
                Sprites.Remove(LoadButton);

                var serializedInfo = System.IO.File.ReadAllText("NamesAndPositions.json");
                MakeMapScreen.TilesGraph = JsonConvert.DeserializeObject<Graph<Tile, ConnectionTypes>>(serializedInfo);

                foreach (var vertex in MakeMapScreen.TilesGraph.Vertices)
                {
                    vertex.Value.Texture = contentManager.Load<Texture2D>(vertex.Value.Name);
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

            base.Update(gameTime);
        }
    }
}

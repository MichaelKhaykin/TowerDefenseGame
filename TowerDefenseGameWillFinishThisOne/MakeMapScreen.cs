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
    public class MakeMapScreen : Screen
    {

        List<Button> TileButtons = new List<Button>();

        Button saveButton;
        
        GraphicsDevice graphics;

        List<Tile> newTilesToBeCreated = new List<Tile>();

        bool hasFinishedPlacing = true;

        Button LoadButton;

        ContentManager content;

        public MakeMapScreen(GraphicsDevice graphics, ContentManager content)
            : base(graphics, content)
        {
            this.graphics = graphics;
            this.content = content;

            Texture2D backgroundBox = Content.Load<Texture2D>("BackgroundBox");

            string[] roadPieces = { "RightUp", "LeftUp", "RightDown", "LeftDown" };
            float buttonScale = backgroundBox.Width / 2 * Main.SpriteScales["BackgroundBox"];
            var position = new Vector2(buttonScale, 200) * Main.ScreenScale;

            foreach (var roadPiece in roadPieces)
            {

            }

            var saveButtonTexture = Content.Load<Texture2D>("SaveButton");
            saveButton = new Button(saveButtonTexture, new Vector2(1200, (saveButtonTexture.Width * Main.SpriteScales["SaveButton"]) / 2), Color.White, new Vector2(Main.SpriteScales["SaveButton"] * Main.ScreenScale), null);

            //Don't need these could just add these directly into the TileButtons list

            var topRightRoadPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/RightUpArcPiece"), new Vector2(0 + backgroundBox.Width / 2 * Main.SpriteScales["BackgroundBox"], 200) * Main.ScreenScale);
            var topLeftRoadPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/LeftUpArcPiece"), new Vector2(topRightRoadPiece.Position.X, topRightRoadPiece.Position.Y + topRightRoadPiece.Texture.Height * topRightRoadPiece.Scale.Y));
            var bottomRightRoadPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/RightDownArcPiece"), new Vector2(topLeftRoadPiece.Position.X, topLeftRoadPiece.Position.Y + topLeftRoadPiece.Texture.Height * topRightRoadPiece.Scale.Y));
            var bottomLeftRoadPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/LeftDownArcPiece"), new Vector2(bottomRightRoadPiece.Position.X, bottomRightRoadPiece.Position.Y + bottomRightRoadPiece.Texture.Height * bottomRightRoadPiece.Scale.Y));
            var straightHorizontalPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/StraightHorizontalPiece"), new Vector2(bottomLeftRoadPiece.Position.X, bottomLeftRoadPiece.Position.Y + bottomLeftRoadPiece.ScaledHeight));
            var straightVerticalPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/StraightVerticalPiece"), new Vector2(straightHorizontalPiece.Position.X, straightHorizontalPiece.Position.Y + straightHorizontalPiece.ScaledHeight));

            topRightRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/RightUpArcPiece"), ConnectionTypes.Right, ConnectionTypes.Bottom);
            topLeftRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/LeftUpArcPiece"), ConnectionTypes.Left, ConnectionTypes.Bottom);
            bottomRightRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/RightDownArcPiece"), ConnectionTypes.Right, ConnectionTypes.Top);
            bottomLeftRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/LeftDownArcPiece"), ConnectionTypes.Left, ConnectionTypes.Top);
            straightHorizontalPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/StraightHorizontalPiece"), ConnectionTypes.Right, ConnectionTypes.Left);
            straightVerticalPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/StraightVerticalPiece"), ConnectionTypes.Bottom, ConnectionTypes.Top);

            TileButtons.Add(topRightRoadPiece);
            TileButtons.Add(topLeftRoadPiece);
            TileButtons.Add(bottomRightRoadPiece);
            TileButtons.Add(bottomLeftRoadPiece);
            TileButtons.Add(straightHorizontalPiece);
            TileButtons.Add(straightVerticalPiece);
            //Debug
            LoadButton = new Button(Content.Load<Texture2D>("LoadButton"), new Vector2(100, 800), Color.White, new Vector2(1), null);

            Sprites.AddRange(TileButtons);
            Sprites.Add(saveButton);
            Sprites.Add(LoadButton);
        }

        private void PlaceTile(List<Tile> tiles)
        {
            Tile tile = tiles[tiles.Count - 1];

            tile.IsVisible = true;

            //cuz int math kewl
            tile.Position = new Vector2((int)(Main.mouse.X / ((tile.ScaledWidth))) * ((tile.ScaledWidth)), (int)(Main.mouse.Y / ((tile.ScaledHeight))) * ((tile.ScaledHeight)));
            if (Main.mouse.LeftButton == ButtonState.Released && Main.oldMouse.LeftButton == ButtonState.Pressed)
            {
                hasFinishedPlacing = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (saveButton.IsClicked(Main.mouse) && !saveButton.IsClicked(Main.oldMouse))
            {
                var saveInfo = newTilesToBeCreated.Select(t => t.GetInfo());
                var serializedInfo = JsonConvert.SerializeObject(saveInfo);
                System.IO.File.WriteAllText("NamesAndPositions.json", serializedInfo);
            }

            if (LoadButton.IsClicked(Main.mouse) && !LoadButton.IsClicked(Main.oldMouse))
            {
                Sprites.Clear();

                string serializedInfo = System.IO.File.ReadAllText("NamesAndPositions.json");

                var savedInfo = JsonConvert.DeserializeObject<List<TileInfo>>(serializedInfo);

                for (int i = 0; i < savedInfo.Count; i++)
                {
                    var tile = new Tile(savedInfo[i], content);
                    Sprites.Add(tile);
                }
            }

            for (int i = 0; i < TileButtons.Count; i++)
            {
                if (TileButtons[i].IsClicked(Main.mouse) && !TileButtons[i].IsClicked(Main.oldMouse) && hasFinishedPlacing)
                {
                    var tileInfo = (TileCreateInfo)(TileButtons[i].Tag);
                    Tile tileToCreate = new Tile(tileInfo, new Vector2(-10, -10), new Vector2(Main.SpriteScales[tileInfo.Texture.Name] * Main.ScreenScale))
                    {
                        Connections = tileInfo.Connections
                    };

                    newTilesToBeCreated.Add(tileToCreate);
                    Sprites.Add(newTilesToBeCreated[newTilesToBeCreated.Count - 1]);
  
                    hasFinishedPlacing = false;
                }
            }
            if (hasFinishedPlacing == false)
            {
                PlaceTile(newTilesToBeCreated);
            }
            base.Update(gameTime);
        }
     }
}

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

        Tile newestTileToCreate;

        Button saveButton;

        GraphicsDevice graphics;

        List<Tile> newTilesToBeCreated = new List<Tile>();

        bool hasFinishedPlacing = true;

        Button LoadButton;

        ContentManager content;

        Graph<Tile, ConnectionTypes> TilesGraph = new Graph<Tile, ConnectionTypes>();

        TextLabel savedLabel;
        SpriteFont font;

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
            saveButton = new Button(saveButtonTexture, new Vector2(1200 * Main.SpriteScales["SaveButton"] * Main.ScreenScale, (saveButtonTexture.Width * Main.SpriteScales["SaveButton"]) / 2), Color.White, new Vector2(Main.SpriteScales["SaveButton"] * Main.ScreenScale), null);
            
            font = Content.Load<SpriteFont>("TextFont");
            savedLabel = new TextLabel(new Vector2(saveButton.X + saveButton.ScaledWidth, saveButton.Y), Color.Black, "Saved!", font);
            savedLabel.IsVisible = false;

            LoadButton = new Button(Content.Load<Texture2D>("LoadButton"), new Vector2(saveButton.X, saveButton.ScaledHeight + saveButton.Y), Color.White, new Vector2(Main.SpriteScales["LoadButton"] * Main.ScreenScale), null);
            
            //Don't need these could just add these directly into the TileButtons list

            var topRightRoadPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/RightUpArcPiece"), new Vector2(0 + backgroundBox.Width / 2 * Main.SpriteScales["BackgroundBox"], 200) * Main.ScreenScale);
            var topLeftRoadPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/LeftUpArcPiece"), new Vector2(topRightRoadPiece.Position.X, topRightRoadPiece.Position.Y + topRightRoadPiece.ScaledHeight));
            var bottomRightRoadPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/RightDownArcPiece"), new Vector2(topLeftRoadPiece.Position.X, topLeftRoadPiece.Position.Y + topLeftRoadPiece.ScaledHeight));
            var bottomLeftRoadPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/LeftDownArcPiece"), new Vector2(bottomRightRoadPiece.Position.X, bottomRightRoadPiece.Position.Y + bottomRightRoadPiece.ScaledHeight));
            var straightHorizontalPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/StraightHorizontalPiece"), new Vector2(bottomLeftRoadPiece.Position.X, bottomLeftRoadPiece.Position.Y + bottomLeftRoadPiece.ScaledHeight));
            var straightVerticalPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/StraightVerticalPiece"), new Vector2(straightHorizontalPiece.Position.X, straightHorizontalPiece.Position.Y + straightHorizontalPiece.ScaledHeight));
            var fourSideCrossPiece = Main.CreateButton(graphics, backgroundBox, Content.Load<Texture2D>("RoadPieces/4SideCrossPiece"), new Vector2(straightVerticalPiece.Position.X, straightVerticalPiece.Position.Y + straightVerticalPiece.ScaledHeight));

            topRightRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/RightUpArcPiece"), ConnectionTypes.Right, ConnectionTypes.Bottom);
            topLeftRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/LeftUpArcPiece"), ConnectionTypes.Left, ConnectionTypes.Bottom);
            bottomRightRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/RightDownArcPiece"), ConnectionTypes.Right, ConnectionTypes.Top);
            bottomLeftRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/LeftDownArcPiece"), ConnectionTypes.Left, ConnectionTypes.Top);
            straightHorizontalPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/StraightHorizontalPiece"), ConnectionTypes.Right, ConnectionTypes.Left);
            straightVerticalPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/StraightVerticalPiece"), ConnectionTypes.Bottom, ConnectionTypes.Top);
            fourSideCrossPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/4SideCrossPiece"), ConnectionTypes.Top, ConnectionTypes.Bottom, ConnectionTypes.Right, ConnectionTypes.Left);

            TileButtons.Add(topRightRoadPiece);
            TileButtons.Add(topLeftRoadPiece);
            TileButtons.Add(bottomRightRoadPiece);
            TileButtons.Add(bottomLeftRoadPiece);
            TileButtons.Add(straightHorizontalPiece);
            TileButtons.Add(straightVerticalPiece);
            TileButtons.Add(fourSideCrossPiece);

            Sprites.AddRange(TileButtons);
            Sprites.Add(saveButton);
            Sprites.Add(LoadButton);
            Sprites.Add(savedLabel);
        }

        private void PlaceTile(Tile tile)
        {
            //cuz int math kewl
            tile.Position = new Vector2((int)(Main.mouse.X / ((tile.ScaledWidth))) * ((tile.ScaledWidth)), (int)(Main.mouse.Y / ((tile.ScaledHeight))) * ((tile.ScaledHeight)));

            //If the tile is trying to be placed too close to the buttons, remove it from the list and return from function
            if (tile.Position.X < 200 * Main.ScreenScale)
            {
                tile.IsVisible = false;
                if (Main.mouse.LeftButton == ButtonState.Released)
                {
                    hasFinishedPlacing = true;
                    return;
                }
            }
            else
            {
                tile.IsVisible = true;
            }

            //There are no tiles are on the screen, so you are able to put the tile anywhere on the screen
            if (newTilesToBeCreated.Count == 0)
            {
                if (Main.mouse.LeftButton == ButtonState.Released && Main.oldMouse.LeftButton == ButtonState.Pressed)
                {
                    TilesGraph.AddVertex(tile);
                    newTilesToBeCreated.Add(tile);
                    hasFinishedPlacing = true;
                }
            }
            else //Now we have to make sure the new tiles that is going to be placed is going to connect to an existing item
            {
                if (Main.mouse.LeftButton == ButtonState.Released && Main.oldMouse.LeftButton == ButtonState.Pressed)
                {
                    Tile tileOnTheRight = null;
                    Tile tileOnTheTop = null;
                    Tile tileOnTheBottom = null;
                    Tile tileOnTheLeft = null;
                    for (int i = 0; i < newTilesToBeCreated.Count; i++)
                    {
                        if ((newTilesToBeCreated[i].Position + new Vector2(newTilesToBeCreated[i].ScaledWidth, 0)).VEquals(tile.Position))
                        {
                            //This means the new tile is on the left of an existing tile
                            tileOnTheLeft = newTilesToBeCreated[i];
                            for (int j = 0; j < tile.Connections.Length; j++)
                            {
                                for (int k = 0; k < tileOnTheLeft.Connections.Length; k++)
                                {
                                    if (tile.Connections[j] == ConnectionTypes.Left && tileOnTheLeft.Connections[k] == ConnectionTypes.Right)
                                    {
                                        //Valid placement
                                        var v1 = new Vertex<Tile, ConnectionTypes>(tile);
                                        var v2 = new Vertex<Tile, ConnectionTypes>(tileOnTheLeft);

                                        var me = TilesGraph.FindVertex(v1);
                                        var neighbor = TilesGraph.FindVertex(v2);

                                        try
                                        {
                                            if (me is null)
                                            {
                                                TilesGraph.AddVertex(v1);
                                            }
                                            else
                                            {
                                                //This tile is already in the graph - let's make sure the connection we're making is open
                                                if(me.Edges.Where(e => e.EdgeType == ConnectionTypes.Left).Count() > 0)
                                                {
                                                    throw new InvalidTilePlacementException();
                                                }
                                                v1 = me;
                                            }
                                            TilesGraph.AddEdge(v1, neighbor, tile.Connections[j], tileOnTheLeft.Connections[k], 1);
                                            newTilesToBeCreated.Add(tile);
                                            hasFinishedPlacing = true;
                                        }
                                        catch (InvalidTilePlacementException e)
                                        {
                                            TilesGraph.RemoveVertex(v1);
                                            Sprites.Remove(tile);
                                            hasFinishedPlacing = true;
                                        }
                                    }
                                }
                            }
                        }
                        if ((newTilesToBeCreated[i].Position - new Vector2(newTilesToBeCreated[i].ScaledWidth, 0)).VEquals(tile.Position))
                        {
                            //This means the new tile is on the right of an existing of tile
                            tileOnTheRight = newTilesToBeCreated[i];
                            for (int j = 0; j < tile.Connections.Length; j++)
                            {
                                for (int k = 0; k < tileOnTheRight.Connections.Length; k++)
                                {
                                    if (tile.Connections[j] == ConnectionTypes.Right && tileOnTheRight.Connections[k] == ConnectionTypes.Left)
                                    {
                                        //Valid placement
                                        var v1 = new Vertex<Tile, ConnectionTypes>(tile);
                                        var v2 = new Vertex<Tile, ConnectionTypes>(tileOnTheRight);

                                        var me = TilesGraph.FindVertex(v1);
                                        var neighbor = TilesGraph.FindVertex(v2);

                                        try
                                        {
                                            if (me is null)
                                            {
                                                TilesGraph.AddVertex(v1);
                                            }
                                            else
                                            {
                                                v1 = me;
                                            }
                                            TilesGraph.AddEdge(v1, neighbor, tile.Connections[j], tileOnTheRight.Connections[k], 1);
                                            newTilesToBeCreated.Add(tile);
                                            hasFinishedPlacing = true;
                                        }
                                        catch (InvalidTilePlacementException e)
                                        {
                                            TilesGraph.RemoveVertex(v1);
                                            Sprites.Remove(tile);
                                            hasFinishedPlacing = true;
                                        }
                                    }
                                }
                            }
                        }

                        if ((newTilesToBeCreated[i].Position + new Vector2(0, newTilesToBeCreated[i].ScaledHeight)).VEquals(tile.Position))
                        {
                            //This means the new tile is below existing tile
                            tileOnTheTop = newTilesToBeCreated[i];
                            for (int j = 0; j < tile.Connections.Length; j++)
                            {
                                for (int k = 0; k < tileOnTheTop.Connections.Length; k++)
                                {
                                    if (tile.Connections[j] == ConnectionTypes.Top && tileOnTheTop.Connections[k] == ConnectionTypes.Bottom)
                                    {
                                        //Valid placement
                                        var v1 = new Vertex<Tile, ConnectionTypes>(tile);
                                        var v2 = new Vertex<Tile, ConnectionTypes>(tileOnTheTop);

                                        var me = TilesGraph.FindVertex(v1);
                                        var neighbor = TilesGraph.FindVertex(v2);

                                        try
                                        {
                                            if (me is null)
                                            {
                                                TilesGraph.AddVertex(v1);
                                            }
                                            else
                                            {
                                                v1 = me;
                                            }
                                            TilesGraph.AddEdge(v1, neighbor, tile.Connections[j], tileOnTheTop.Connections[k], 1);
                                            newTilesToBeCreated.Add(tile);
                                            hasFinishedPlacing = true;
                                        }
                                        catch (InvalidTilePlacementException e)
                                        {
                                            TilesGraph.RemoveVertex(v1);
                                            Sprites.Remove(tile);
                                            hasFinishedPlacing = true;
                                        }
                                    }
                                }
                            }
                        }
                        if ((newTilesToBeCreated[i].Position - new Vector2(0, tile.ScaledHeight)).VEquals(tile.Position))
                        {
                            //This means the tile is on the top
                            tileOnTheBottom = newTilesToBeCreated[i];
                            for (int j = 0; j < tile.Connections.Length; j++)
                            {
                                for (int k = 0; k < tileOnTheBottom.Connections.Length; k++)
                                {
                                    if (tile.Connections[j] == ConnectionTypes.Bottom && tileOnTheBottom.Connections[k] == ConnectionTypes.Top)
                                    {
                                        //Valid placement
                                        var v1 = new Vertex<Tile, ConnectionTypes>(tile);
                                        var v2 = new Vertex<Tile, ConnectionTypes>(tileOnTheBottom);

                                        var me = TilesGraph.FindVertex(v1);
                                        var neighbor = TilesGraph.FindVertex(v2);

                                        try
                                        {
                                            if (me is null)
                                            {
                                                TilesGraph.AddVertex(v1);
                                            }
                                            else
                                            {
                                                v1 = me;
                                            }
                                            TilesGraph.AddEdge(v1, neighbor, tile.Connections[j], tileOnTheBottom.Connections[k], 1);
                                            newTilesToBeCreated.Add(tile);
                                            hasFinishedPlacing = true;
                                        }
                                        catch (InvalidTilePlacementException e)
                                        {
                                            TilesGraph.RemoveVertex(v1);
                                            Sprites.Remove(tile);
                                            hasFinishedPlacing = true;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //If all of these are null by the end of the for loop, that means the tile about to be placed is away from all other tiles, which is not okay
                    if (tileOnTheRight == null && tileOnTheTop == null && tileOnTheBottom == null && tileOnTheLeft == null)
                    {
                        Sprites.Remove(tile);
                        hasFinishedPlacing = true;
                        return;
                    }

                    if (hasFinishedPlacing == false)
                    {
                        Sprites.Remove(tile);
                        hasFinishedPlacing = true;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (saveButton.IsClicked(Main.mouse) && !saveButton.IsClicked(Main.oldMouse))
            {
                var saveInfo = newTilesToBeCreated.Select(t => t.GetInfo());
                var serializedInfo = JsonConvert.SerializeObject(saveInfo);
                System.IO.File.WriteAllText("NamesAndPositions.json", serializedInfo);

                savedLabel.IsVisible = true;
            }

            if (LoadButton.IsClicked(Main.mouse) && !LoadButton.IsClicked(Main.oldMouse))
            {
                Main.CurrentScreen = ScreenStates.Game;
                Main.ScreenCameFrom = ScreenStates.ChoosePlayOrMakeMap;
            }

            for (int i = 0; i < TileButtons.Count; i++)
            {
                if (TileButtons[i].IsClicked(Main.mouse) && !TileButtons[i].IsClicked(Main.oldMouse) && hasFinishedPlacing)
                {
                    var tileInfo = (TileCreateInfo)(TileButtons[i].Tag);
                    newestTileToCreate = new Tile(tileInfo, new Vector2(-10, -10), new Vector2(Main.SpriteScales[tileInfo.Texture.Name] * Main.ScreenScale))
                    {
                        Connections = tileInfo.Connections
                    };

                    Sprites.Add(newestTileToCreate);

                    hasFinishedPlacing = false;
                }
            }
            if (hasFinishedPlacing == false)
            {
                PlaceTile(newestTileToCreate);
            }
            base.Update(gameTime);
        }
    }
}

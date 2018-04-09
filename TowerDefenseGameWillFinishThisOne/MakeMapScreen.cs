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
        Button MarkStartTile;
        Button MarkEndTile;

        List<Button> TileButtons = new List<Button>();

        Tile newestTileToCreate;

        Button backButton;

        bool isMarkingStartTile = false;
        bool isMarkingEndTile = false;

        Button saveButton;

        string nameofmap = "";

        bool shouldErase = false;

        GraphicsDevice graphics;

        //List<Tile> TilesList = new List<Tile>();

        bool hasFinishedPlacing = true;

        ContentManager content;

        Button Eraser;

        Tile[,] TilesArray;
        public static Graph<Tile, ConnectionTypes> TilesGraph = new Graph<Tile, ConnectionTypes>();

        TextLabel savedLabel;
        SpriteFont font;

        public MakeMapScreen(GraphicsDevice graphics, ContentManager content)
            : base(graphics, content)
        {
            this.graphics = graphics;
            this.content = content;

            Eraser = new Button(Content.Load<Texture2D>("Eraser"), new Vector2((graphics.Viewport.Width + 2000) * Main.ScreenScale * Main.SpriteScales["Eraser"], 120 * Main.ScreenScale * Main.SpriteScales["Eraser"]), Color.White, new Vector2(Main.ScreenScale * Main.SpriteScales["Eraser"]), null);

            Texture2D backgroundBoxTexture = Content.Load<Texture2D>("BackgroundBox");

            string[] roadPieces = { "RightUp", "LeftUp", "RightDown", "LeftDown" };
            float buttonScale = backgroundBoxTexture.Width / 2 * Main.SpriteScales["BackgroundBox"];
            var position = new Vector2(buttonScale, 200) * Main.ScreenScale;


            foreach (var roadPiece in roadPieces)
            {

            }

            var saveButtonTexture = Content.Load<Texture2D>("SaveButton");
            saveButton = new Button(saveButtonTexture, new Vector2(1200 * Main.SpriteScales["SaveButton"] * Main.ScreenScale, (saveButtonTexture.Width * Main.SpriteScales["SaveButton"]) / 2), Color.White, new Vector2(Main.SpriteScales["SaveButton"] * Main.ScreenScale), null);

            font = Content.Load<SpriteFont>("TextFont");
            savedLabel = new TextLabel(new Vector2(saveButton.X + saveButton.ScaledWidth, saveButton.Y), Color.Black, "Saved!", font);
            savedLabel.IsVisible = false;

            Texture2D backButtonTexture = Content.Load<Texture2D>("BackButton");
            backButton = new Button(backButtonTexture, new Vector2(backButtonTexture.Width / 2 * Main.SpriteScales["BackButton"] * Main.ScreenScale, graphics.Viewport.Height - backButtonTexture.Height / 2 * Main.SpriteScales["BackButton"] * Main.ScreenScale), Color.White, new Vector2(Main.SpriteScales["BackButton"] * Main.ScreenScale), null);

            //Don't need these could just add these directly into the TileButtons list

            var topRightRoadPiece = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("RoadPieces/RightUpArcPiece"), new Vector2(0 + backgroundBoxTexture.Width / 2 * Main.SpriteScales["BackgroundBox"], 200) * Main.ScreenScale);
            var topLeftRoadPiece = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("RoadPieces/LeftUpArcPiece"), new Vector2(topRightRoadPiece.Position.X, topRightRoadPiece.Position.Y + topRightRoadPiece.ScaledHeight));
            var bottomRightRoadPiece = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("RoadPieces/RightDownArcPiece"), new Vector2(topLeftRoadPiece.Position.X, topLeftRoadPiece.Position.Y + topLeftRoadPiece.ScaledHeight));
            var bottomLeftRoadPiece = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("RoadPieces/LeftDownArcPiece"), new Vector2(bottomRightRoadPiece.Position.X, bottomRightRoadPiece.Position.Y + bottomRightRoadPiece.ScaledHeight));
            var straightHorizontalPiece = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("RoadPieces/StraightHorizontalPiece"), new Vector2(bottomLeftRoadPiece.Position.X, bottomLeftRoadPiece.Position.Y + bottomLeftRoadPiece.ScaledHeight));
            var straightVerticalPiece = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("RoadPieces/StraightVerticalPiece"), new Vector2(straightHorizontalPiece.Position.X, straightHorizontalPiece.Position.Y + straightHorizontalPiece.ScaledHeight));
            var fourSideCrossPiece = Main.CreateButton(graphics, backgroundBoxTexture, Content.Load<Texture2D>("RoadPieces/4SideCrossPiece"), new Vector2(straightVerticalPiece.Position.X, straightVerticalPiece.Position.Y + straightVerticalPiece.ScaledHeight));

            MarkStartTile = new Button(Content.Load<Texture2D>("MarkStartTile"), new Vector2(topRightRoadPiece.X, topRightRoadPiece.Y - topRightRoadPiece.ScaledHeight), Color.White, new Vector2(Main.SpriteScales["MarkStartTile"] * Main.ScreenScale), null);
            MarkEndTile = new Button(Content.Load<Texture2D>("MarkEndTile"), new Vector2(topRightRoadPiece.X, fourSideCrossPiece.Y + fourSideCrossPiece.ScaledHeight), Color.White, new Vector2(Main.SpriteScales["MarkEndTile"] * Main.ScreenScale), null);

            topRightRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/RightUpArcPiece"), ConnectionTypes.Right, ConnectionTypes.Bottom);
            topLeftRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/LeftUpArcPiece"), ConnectionTypes.Left, ConnectionTypes.Bottom);
            bottomRightRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/RightDownArcPiece"), ConnectionTypes.Right, ConnectionTypes.Top);
            bottomLeftRoadPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/LeftDownArcPiece"), ConnectionTypes.Left, ConnectionTypes.Top);
            straightHorizontalPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/StraightHorizontalPiece"), ConnectionTypes.Right, ConnectionTypes.Left);
            straightVerticalPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/StraightVerticalPiece"), ConnectionTypes.Bottom, ConnectionTypes.Top);
            fourSideCrossPiece.Tag = new TileCreateInfo(Content.Load<Texture2D>("RoadPieces/4SideCrossPiece"), ConnectionTypes.Top, ConnectionTypes.Bottom, ConnectionTypes.Right, ConnectionTypes.Left);

            newestTileToCreate = new Tile(new TileInfo("RoadPieces/RightUpArcPiece", new Vector2(-10, -10), null), content, "RoadPieces/RightUpArcPiece");

            //comebacktothis at 11;5
            TilesArray = new Tile[(int)(graphics.Viewport.Width / newestTileToCreate.HitBox.Width), (int)(graphics.Viewport.Height / newestTileToCreate.HitBox.Height)];

            TileButtons.Add(topRightRoadPiece);
            TileButtons.Add(topLeftRoadPiece);
            TileButtons.Add(bottomRightRoadPiece);
            TileButtons.Add(bottomLeftRoadPiece);
            TileButtons.Add(straightHorizontalPiece);
            TileButtons.Add(straightVerticalPiece);
            TileButtons.Add(fourSideCrossPiece);

            Sprites.AddRange(TileButtons);
            Sprites.Add(saveButton);
            Sprites.Add(savedLabel);
            Sprites.Add(MarkStartTile);
            Sprites.Add(MarkEndTile);
            Sprites.Add(backButton);
            Sprites.Add(Eraser);
        }

        private bool IsValidMap()
        {
            int count = 0;
            foreach (var vertex in TilesGraph.Vertices)
            {
                if (vertex.Value.Connections.Length != vertex.Edges.Count)
                {
                    count++;
                }
            }
            if (count == 2)
            {
                return true;
            }
            return false;
        }

        private void DrawGridLines(SpriteBatch spriteBatch, int TileWidth, int TileHeight)
        {
            Texture2D pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData(new Color[] { Color.White });
            
            for (int i = TileWidth/2; i <= TilesArray.GetLength(0) * TileWidth; i += TileWidth)
            {
                Line line = new Line(new Vector2(i, 0), new Vector2(i, graphics.Viewport.Height));
                Extensions.DrawLine(spriteBatch, line, pixel, 1, Color.Red, true);
            }
            for (int i = TileHeight/2; i <= TilesArray.GetLength(1) * TileHeight; i+= TileHeight)
            {
                Line line = new Line(new Vector2(0, i), new Vector2(graphics.Viewport.Width, i));
                Extensions.DrawLine(spriteBatch, line, pixel, 1, Color.Red, true);
            }
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
            if (TilesGraph.Count == 0)
            {
                if (Main.mouse.LeftButton == ButtonState.Released && Main.oldMouse.LeftButton == ButtonState.Pressed)
                {
                    TilesGraph.AddVertex(tile);
                    TilesArray[Index(tile).x, Index(tile).y] = tile;
                    hasFinishedPlacing = true;
                }
                return;
            }
            //Now we have to make sure the new tiles that is going to be placed is going to connect to an existing item

            if (!(Main.mouse.LeftButton == ButtonState.Released && Main.oldMouse.LeftButton == ButtonState.Pressed))
            {
                return;
            }


            bool isTilePlacementValid = true;
            Tile neighborTile = null;

            (int x, int y) index = Index(tile);
            Tile top = TilesArray[index.x, index.y-1];
            Tile bottom = TilesArray[index.x, index.y + 1];
            Tile right = TilesArray[index.x + 1, index.y];
            Tile left = TilesArray[index.x - 1, index.y];

            if(!(TilesArray[index.x, index.y] is null))
            {
                //a tile already exists here
                Sprites.Remove(tile);
                hasFinishedPlacing = true;
                return;
            }


            //tile to the left of our left neighbor
            if (!(left is null))
            {
                //This means the new tile is on the left of an existing tile
                ConnectionTypes myConnectionType = ConnectionTypes.Left;
                ConnectionTypes neighborConnectionType = ConnectionTypes.Right;

                neighborTile = left;
                isTilePlacementValid |= checkAndPlaceTileIfValid(myConnectionType, neighborConnectionType);
            }

            if (!(right is null))
            {
                //This means the new tile is on the right of an existing of tile
                ConnectionTypes myConnectionType = ConnectionTypes.Right;
                ConnectionTypes neighborConnectionType = ConnectionTypes.Left;

                neighborTile = right;
                isTilePlacementValid |= checkAndPlaceTileIfValid(myConnectionType, neighborConnectionType);
            }

            if (!(bottom is null))
            {
                //This means the new tile is below existing tile
                ConnectionTypes myConnectionType = ConnectionTypes.Bottom;
                ConnectionTypes neighborConnectionType = ConnectionTypes.Top;

                neighborTile = bottom;
                isTilePlacementValid |= checkAndPlaceTileIfValid(myConnectionType, neighborConnectionType);
            }

            if (!(top is null))
            {
                //This means the tile is on the top
                ConnectionTypes myConnectionType = ConnectionTypes.Top;
                ConnectionTypes neighborConnectionType = ConnectionTypes.Bottom;

                neighborTile = top;
                isTilePlacementValid |= checkAndPlaceTileIfValid(myConnectionType, neighborConnectionType);
            }

            if (!isTilePlacementValid)
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

            bool checkAndPlaceTileIfValid(ConnectionTypes myConnectionType, ConnectionTypes neighborConnectionType)
            {
                for (int j = 0; j < tile.Connections.Length; j++)
                {
                    for (int k = 0; k < neighborTile.Connections.Length; k++)
                    {
                        if (tile.Connections[j] == myConnectionType && neighborTile.Connections[k] == neighborConnectionType)
                        {
                            //Valid placement
                            var v1 = new Vertex<Tile, ConnectionTypes>(tile);
                            var v2 = new Vertex<Tile, ConnectionTypes>(neighborTile);


                            var me = TilesGraph.FindVertex(v1);
                            var neighbor = TilesGraph.FindVertex(v2);

                            if (!(me is null))
                            {
                                v1 = me;
                            }

                            foreach (var edge in neighbor.Edges)
                            {
                                if(edge.EdgeType == neighborConnectionType)
                                {
                                    //the connection already exists
                                    hasFinishedPlacing = true;
                                    return false;
                                }
                            }

                            TilesGraph.AddVertex(v1);
                            TilesArray[Index(v1.Value).x, Index(v1.Value).y] = v1.Value;
                            TilesGraph.AddEdge(v1, neighbor, tile.Connections[j], neighborTile.Connections[k], 1);
                            hasFinishedPlacing = true;
                            return true;

                        
                        }
                    }
                }
                return false;
            }
        }

        private void ColorMarkedTile(Color color, ref Button buttonToRemove, ref bool isMarkingWhichTile, bool isStart)
        {
            foreach (var vertex in TilesGraph.Vertices)
            {
                if (vertex.Value.IsClicked(Main.mouse) && !vertex.Value.IsClicked(Main.oldMouse))
                {
                    if (vertex.Value.Connections.Length == 2)//This ifstatement is checking if this tile only has 2 connections if it only has two, then check for those specific two edges
                    {
                        
                        if (vertex.Value.IsEndingTile == false && vertex.Value.IsStartingTile == false && vertex.Edges.Count < 2)
                        {
                            vertex.Value.Color = color;
                            vertex.Value.IsStartingTile = isStart;
                            vertex.Value.IsEndingTile = !isStart;
                            isMarkingWhichTile = false;
                            buttonToRemove.IsVisible = false;
                        }
                    }
                    else
                    {
                        if (vertex.Value.IsEndingTile == false && vertex.Value.IsStartingTile == false && vertex.Edges.Count < 5)
                        {
                            vertex.Value.Color = color;
                            vertex.Value.IsStartingTile = isStart;
                            vertex.Value.IsEndingTile = !isStart;
                            isMarkingWhichTile = false;
                            buttonToRemove.IsVisible = false;
                        }
                    }
                }
            }
        }

        private void EraseTile()
        {
            for (int i = 0; i < TilesArray.GetLength(0); i++)
            {
                for (int j = 0; j < TilesArray.GetLength(1); j++)
                {
                    if (TilesArray[i, j] is null)
                    {
                        continue;
                    }
                    if (TilesArray[i, j].IsClicked(Main.mouse) && !TilesArray[i, j].IsClicked(Main.oldMouse))
                    {
                        if(TilesArray[i, j].IsStartingTile)
                        {
                            MarkStartTile.IsVisible = true;
                        }
                        if (TilesArray[i, j].IsEndingTile)
                        {
                            MarkEndTile.IsVisible = true;
                        }
                        var vertex = new Vertex<Tile, ConnectionTypes>(TilesArray[i, j]);
                        TilesArray[i, j] = null;
                        var node = TilesGraph.FindVertex(vertex);
                        TilesGraph.RemoveVertex(node);
                        Sprites.Remove(node.Value);
                        shouldErase = false;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (Eraser.IsClicked(Main.mouse) && !Eraser.IsClicked(Main.oldMouse))
            {
                shouldErase = true;
            }

            if (shouldErase)
            {
                EraseTile();
            }

            if (backButton.IsClicked(Main.mouse) && !backButton.IsClicked(Main.oldMouse))
            {
                Main.CurrentScreen = ScreenStates.ChoosePlayOrMakeMap;
                Main.ScreenCameFrom = ScreenStates.MakeMap;
            }

            List<Keys> keysPressed = keyboard.GetPressedKeys().ToList();
            for (int i = 0; i < keysPressed.Count; i++)
            {
                nameofmap += keysPressed[i].ToString();
            }

            for (int i = 0; i < Sprites.Count; i++)
            {
                if (Sprites[i] == MarkStartTile)
                {
                    if (MarkStartTile.IsClicked(Main.mouse) && !MarkStartTile.IsClicked(Main.oldMouse))
                    {
                        isMarkingStartTile = true;
                    }
                    if (isMarkingStartTile)
                    {
                        ColorMarkedTile(Color.Green, ref MarkStartTile, ref isMarkingStartTile, true);
                    }
                }
                if (Sprites[i] == MarkEndTile)
                {
                    if (MarkEndTile.IsClicked(Main.mouse) && !MarkEndTile.IsClicked(Main.oldMouse))
                    {
                        isMarkingEndTile = true;
                    }

                    if (isMarkingEndTile)
                    {
                        ColorMarkedTile(Color.Red, ref MarkEndTile, ref isMarkingEndTile, false);
                    }
                }
            }

            saveButton.IsVisible = TilesGraph.Vertices.Sum(v => v.Value.Connections.Count()) == TilesGraph.Edges.Count + 2;

            if (saveButton.IsClicked(Main.mouse) && !saveButton.IsClicked(Main.oldMouse))
            {
                if (MarkStartTile == null && MarkEndTile == null)
                {
                    SaveMap();
                }
            }

            for (int i = 0; i < TileButtons.Count; i++)
            {
                if (TileButtons[i].IsClicked(Main.mouse) && !TileButtons[i].IsClicked(Main.oldMouse) && hasFinishedPlacing)
                {
                    var tileInfo = (TileCreateInfo)(TileButtons[i].Tag);
                    newestTileToCreate = new Tile(tileInfo, new Vector2(-10, -10), new Vector2(Main.SpriteScales[tileInfo.Texture.Name] * Main.ScreenScale), tileInfo.Texture.Name);
                    //{
                    //    Connections = tileInfo.Connections
                    //};

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

        private void SaveMap()
        {
            //var saveInfo = TilesGraph.Vertices.Select(t => t.Value.GetInfo());
            //var serializedInfo = JsonConvert.SerializeObject(saveInfo);

            var serializedInfo = JsonConvert.SerializeObject(TilesGraph);

            System.IO.File.WriteAllText("NamesAndPositions.json", serializedInfo);

            savedLabel.IsVisible = true;
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
            DrawGridLines(spriteBatch, newestTileToCreate.HitBox.Width, newestTileToCreate.HitBox.Height);
            base.Draw(spriteBatch);
        }
    }
}
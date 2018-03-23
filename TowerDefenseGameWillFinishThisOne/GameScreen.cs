using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MichaelLibrary;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace TowerDefenseGameWillFinishThisOne
{
    public class GameScreen : Screen
    {
        public GameScreen(GraphicsDevice graphics, ContentManager content) : base(graphics, content)
        {
            string serializedInfo = System.IO.File.ReadAllText("NamesAndPositions.json");

            var savedInfo = JsonConvert.DeserializeObject<List<TileInfo>>(serializedInfo);

            for (int i = 0; i < savedInfo.Count; i++)
            {
                var tile = new Tile(savedInfo[i], content);
                Sprites.Add(tile);
            }
        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace TowerDefenseGame
{
    public class TileCreateInfo
    {
        public Texture2D Texture;
        public ConnectionTypes[] Connections;

        public List<Vector2> PathPositions = new List<Vector2>();

        public TileCreateInfo(Texture2D texture, params ConnectionTypes[] connectionTypes)
        {
            Texture = texture;
            Connections = connectionTypes;
        }
    }
}
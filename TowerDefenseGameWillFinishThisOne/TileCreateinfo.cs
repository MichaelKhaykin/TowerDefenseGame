using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefenseGameWillFinishThisOne
{
    public class TileCreateInfo
    {
        public Texture2D Texture;
        public ConnectionTypes[] Connections;

        public TileCreateInfo(Texture2D texture, params ConnectionTypes[] connectionTypes)
        {
            Texture = texture;
            Connections = connectionTypes;
        }
    }
}

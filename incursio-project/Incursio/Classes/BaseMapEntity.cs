using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Incursio.Utils;

namespace Incursio.Classes
{
    public class BaseMapEntity
    {
        public Coordinate location;
        public Boolean passable = true;

        public Texture2D texture;

        public BaseMapEntity()
        {

        }

        public BaseMapEntity(Texture2D tex, bool passable, int x, int y)
        {
            this.texture = tex;
            this.passable = passable;
            this.location = new Coordinate(x, y);
        }

        public virtual void setOccupancy(ref byte[,] grid)
        {
            grid[location.x, location.y] = (byte)(passable ? 1 : 0);
        }

        public virtual void addToMap(MapBase map)
        {
            map.addMapEntity(this, location.x, location.y);
            this.setOccupancy(ref map.occupancyGrid);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Classes.Terrain
{
    public class Tree : BaseMapEntity
    {

        public Tree(int x, int y)
        {
            this.texture = TextureBank.MapTiles.tree1;
            this.passable = false;
            this.location = new Coordinate(x, y);
        }

        public override void setOccupancy(ref byte[,] grid)
        {
            grid[location.x, location.y] = (byte)(passable ? 1 : 0);

            if(location.y + 1 > 0 && location.y - 1 < grid.GetUpperBound(1))
                grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
        }

        public override void addToMap(MapBase map)
        {
            map.addObjectEntity(this);
            this.setOccupancy(ref map.occupancyGrid);
        }

    }
}

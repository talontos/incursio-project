using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Classes.Terrain
{
    class Building : BaseMapEntity
    {
        int type;

        public Building(int x, int y, int type)
        {
            this.location = new Coordinate(x, y);
            this.passable = false;
            this.type = type;

            if(type == 0)
            {
                this.texture = TextureBank.MapTiles.building1;
            }
            else if(type == 1)
            {
                this.texture = TextureBank.MapTiles.building2;
            }
            else if(type == 2)
            {
                this.texture = TextureBank.MapTiles.building3;
            }
        }

        public override void setOccupancy(ref byte[,] grid)
        {
            if(type == 0)
            {
                grid[location.x, location.y] = (byte)(passable ? 1 : 0);

                if (location.x + 1 < grid.GetUpperBound(0))
                {
                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                }

                if (location.y - 1 > 0)
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                if (location.x + 1 < grid.GetUpperBound(0) && location.y - 1 > 0)
                {
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }
            }
            else if(type == 1)
            {
                grid[location.x, location.y] = (byte)(passable ? 1 : 0);

                if (location.x + 1 < grid.GetUpperBound(0))
                {
                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                }

                if (location.y - 1 > 0)
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                if (location.x + 1 < grid.GetUpperBound(0) && location.y - 1 > 0)
                {
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }
            }
            else if(type == 2)
            {
                grid[location.x, location.y] = (byte)(passable ? 1 : 0);

                if (location.x + 1 < grid.GetUpperBound(0))
                {
                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                }

                if (location.y - 1 > 0)
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                if (location.x + 1 < grid.GetUpperBound(0) && location.y - 1 > 0)
                {
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }
            }
        }
    }
}

/****************************************
 * Copyright © 2008, Team RobotNinja:
 * 
 *     - Henry Armstrong
 *     - Andy Burras
 *     - Mitch Martin
 *     - Xuan Yu
 * 
 * All Rights Reserved
 ***************************************/

using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Utils;
using Incursio.Managers;

namespace Incursio.Classes.Terrain
{
    class Rock : BaseMapEntity
    {
        int type;

        public Rock(int x, int y)
        {
            this.type = 0;
            this.texture = TextureBank.getInstance().terrain.terrain.rockSmall.texture;
            this.passable = false;
            this.location = new Coordinate(x, y);
        }

        public Rock(int x, int y, int type)
        {
            this.type = type;

            if(type == 0)
            {
                this.texture = TextureBank.getInstance().terrain.terrain.rockSmall.texture;
            }
            else if(type == 1)
            {
                this.texture = TextureBank.getInstance().terrain.terrain.rockMedium.texture;
            }
            else if(type == 2)
            {
                this.texture = TextureBank.getInstance().terrain.terrain.rockBig.texture;
            }

            this.passable = false;
            this.location = new Coordinate(x, y);
        }

        public override void setOccupancy(ref byte[,] grid)
        {
            if(type == 0 || type == 1)
            {
                grid[location.x, location.y] = (byte)(passable ? 1 : 0);
            }
            else if(type == 2)
            {
                grid[location.x, location.y] = (byte)(passable ? 1 : 0);

                if(location.x + 1 < grid.GetUpperBound(0))
                {
                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                }

                if(location.y - 1 > 0)
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                if(location.x + 1 < grid.GetUpperBound(0) && location.y - 1 > 0)
                {
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }
            }

            base.setOccupancy(ref grid);
        }
    }
}

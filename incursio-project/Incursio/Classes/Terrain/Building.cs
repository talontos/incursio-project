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
                this.texture = TextureBank.getInstance().terrain.terrain.building1.texture;
            }
            else if(type == 1)
            {
                this.texture = TextureBank.getInstance().terrain.terrain.building2.texture;
            }
            else if(type == 2)
            {
                this.texture = TextureBank.getInstance().terrain.terrain.building3.texture;
            }
            else if (type == 3)
            {
                this.texture = TextureBank.getInstance().terrain.terrain.buildingGroup.texture;
            }
            else if (type == 4)
            {
                this.texture = TextureBank.getInstance().terrain.terrain.buildingGroupEndRight.texture;
            }
            else if (type == 5)
            {
                this.texture = TextureBank.getInstance().terrain.terrain.buildingGroupEndLeft.texture;
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
            else if (type == 3)
            {
                grid[location.x, location.y] = (byte)(passable ? 1 : 0);

                if (location.y + 1 > 0 && location.y - 1 < grid.GetUpperBound(1))
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                /*if(location.x + 2 < grid.GetUpperBound(0))
                {
                    grid[location.x + 2, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 2, location.y - 1] = (byte)(passable ? 1 : 0);

                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }*/
                if (location.x + 1 < grid.GetUpperBound(0))
                {
                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                if (location.x - 1 > 0)
                {
                    grid[location.x - 1, location.y] = (byte)(passable ? 1 : 0);
                }

                if (location.x - 1 > 0 && location.y - 2 > 0)
                {
                    grid[location.x - 1, location.y - 2] = (byte)(passable ? 1 : 0);
                    grid[location.x - 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if (location.x - 1 > 0 && location.y - 1 > 0)
                {
                    grid[location.x - 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                if (location.y - 3 > 0)
                {
                    grid[location.x, location.y - 3] = (byte)(passable ? 1 : 0);
                    grid[location.x, location.y - 2] = (byte)(passable ? 1 : 0);

                    if (location.x + 1 < grid.GetUpperBound(0))
                    {
                        grid[location.x + 1, location.y - 2] = (byte)(passable ? 1 : 0);
                    }

                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if (location.y - 2 > 0)
                {
                    grid[location.x, location.y - 2] = (byte)(passable ? 1 : 0);
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if (location.y - 1 > 0)
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
            }
            else if (type == 4)
            {
                grid[location.x, location.y] = (byte)(passable ? 1 : 0);

                if (location.y + 1 > 0 && location.y - 1 < grid.GetUpperBound(1))
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                /*if(location.x + 2 < grid.GetUpperBound(0))
                {
                    grid[location.x + 2, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 2, location.y - 1] = (byte)(passable ? 1 : 0);

                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }*/
                /*if (location.x + 1 < grid.GetUpperBound(0))
                {
                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }*/

                if (location.x - 1 > 0)
                {
                    grid[location.x - 1, location.y] = (byte)(passable ? 1 : 0);
                }

                if (location.x - 1 > 0 && location.y - 2 > 0)
                {
                    grid[location.x - 1, location.y - 2] = (byte)(passable ? 1 : 0);
                    grid[location.x - 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if (location.x - 1 > 0 && location.y - 1 > 0)
                {
                    grid[location.x - 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                if (location.y - 3 > 0)
                {
                    grid[location.x, location.y - 3] = (byte)(passable ? 1 : 0);
                    grid[location.x, location.y - 2] = (byte)(passable ? 1 : 0);

                    /*if (location.x + 1 < grid.GetUpperBound(0))
                    {
                        grid[location.x + 1, location.y - 2] = (byte)(passable ? 1 : 0);
                    }*/

                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if (location.y - 2 > 0)
                {
                    grid[location.x, location.y - 2] = (byte)(passable ? 1 : 0);
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if (location.y - 1 > 0)
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
            }
            else if (type == 5)
            {
                grid[location.x, location.y] = (byte)(passable ? 1 : 0);

                if (location.y + 1 > 0 && location.y - 1 < grid.GetUpperBound(1))
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                /*if(location.x + 2 < grid.GetUpperBound(0))
                {
                    grid[location.x + 2, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 2, location.y - 1] = (byte)(passable ? 1 : 0);

                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }*/
                if (location.x + 1 < grid.GetUpperBound(0))
                {
                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                /*if (location.x - 1 > 0)
                {
                    grid[location.x - 1, location.y] = (byte)(passable ? 1 : 0);
                }*/

                /*if (location.x - 1 > 0 && location.y - 2 > 0)
                {
                    grid[location.x - 1, location.y - 2] = (byte)(passable ? 1 : 0);
                    grid[location.x - 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if (location.x - 1 > 0 && location.y - 1 > 0)
                {
                    grid[location.x - 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }*/

                if (location.y - 3 > 0)
                {
                    grid[location.x, location.y - 3] = (byte)(passable ? 1 : 0);
                    grid[location.x, location.y - 2] = (byte)(passable ? 1 : 0);

                    if (location.x + 1 < grid.GetUpperBound(0))
                    {
                        grid[location.x + 1, location.y - 2] = (byte)(passable ? 1 : 0);
                    }

                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if (location.y - 2 > 0)
                {
                    grid[location.x, location.y - 2] = (byte)(passable ? 1 : 0);
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if (location.y - 1 > 0)
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
            }
        }
    }
}

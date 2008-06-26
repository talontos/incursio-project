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
using Incursio.Entities;

namespace Incursio.World.Terrain
{
    public class Tree : BaseMapEntity
    {

        bool isGroup = false;

        public Tree(int x, int y)
        {
            this.texture = TextureBank.getInstance().terrain.terrain.tree1.texture;
            this.passable = false;
            this.location = new Coordinate(x, y);
        }

        public Tree(int x, int y, bool isGroup)
        {
            if(isGroup)
            {
                this.texture = TextureBank.getInstance().terrain.terrain.groupOfTrees.texture;
            }
            else
            {
                this.texture = TextureBank.getInstance().terrain.terrain.tree1.texture;
            }
            
            this.passable = false;
            this.location = new Coordinate(x, y);
            this.isGroup = isGroup;
        }

        public override void setOccupancy(ref byte[,] grid)
        {
            if(isGroup){
                grid[location.x, location.y] = (byte)(passable ? 1 : 0);

                if(location.y + 1 > 0 && location.y -1 < grid.GetUpperBound(1))
                {
                    grid[location.x, location.y -1] = (byte)(passable ? 1 : 0);
                }

                /*if(location.x + 2 < grid.GetUpperBound(0))
                {
                    grid[location.x + 2, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 2, location.y - 1] = (byte)(passable ? 1 : 0);

                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }*/
                if(location.x + 1 < grid.GetUpperBound(0))
                {
                    grid[location.x + 1, location.y] = (byte)(passable ? 1 : 0);
                    grid[location.x + 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                if(location.x - 1 > 0)
                {
                    grid[location.x - 1, location.y] = (byte)(passable ? 1 : 0);
                }

                if(location.x - 1 > 0 && location.y - 2 > 0)
                {
                    grid[location.x - 1, location.y - 2] = (byte)(passable ? 1 : 0);
                    grid[location.x - 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if(location.x - 1 > 0 && location.y - 1 > 0)
                {
                    grid[location.x - 1, location.y - 1] = (byte)(passable ? 1 : 0);
                }

                if(location.y - 3 > 0)
                {
                    grid[location.x, location.y - 3] = (byte)(passable ? 1 : 0);
                    grid[location.x, location.y - 2] = (byte)(passable ? 1 : 0);

                    if(location.x + 1 < grid.GetUpperBound(0))
                    {
                        grid[location.x + 1, location.y - 2] = (byte)(passable ? 1 : 0);
                    }
                    
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if(location.y - 2 > 0)
                {
                    grid[location.x, location.y - 2] = (byte)(passable ? 1 : 0);
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
                else if(location.y - 1 > 0)
                {
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
                }
            }
            else
            {
                grid[location.x, location.y] = (byte)(passable ? 1 : 0);

                if (location.y + 1 > 0 && location.y - 1 < grid.GetUpperBound(1))
                    grid[location.x, location.y - 1] = (byte)(passable ? 1 : 0);
            }

            
        }

        public override void addToMap(MapBase map)
        {
            map.addObjectEntity(this);
            this.setOccupancy(ref map.occupancyGrid);
        }

    }
}

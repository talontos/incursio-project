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
using Incursio.Entities;

namespace Incursio.World.Terrain
{
    class Water : BaseMapEntity
    {
        public Water(int x, int y, State.WaterType type)
        {
            this.passable = false;
            this.location = new global::Incursio.Utils.Coordinate(x, y);

            switch (type)
            {
                case State.WaterType.OpenWater:
                    this.texture = TextureBank.getInstance().terrain.terrain.openWater.texture;
                    break;
                case State.WaterType.ShoreDown:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreDown.texture;
                    break;
                case State.WaterType.ShoreUp:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreUp.texture;
                    break;
                case State.WaterType.ShoreLeft:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreLeft.texture;
                    break;
                case State.WaterType.ShoreRight:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreRight.texture;
                    break;
                case State.WaterType.ShoreLowerLeft:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreLowerLeftCorner.texture;
                    break;
                case State.WaterType.ShoreLowerRight:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreLowerRightCorner.texture;
                    break;
                case State.WaterType.ShoreUpperLeft:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreUpperLeftCorner.texture;
                    break;
                case State.WaterType.ShoreUpperRight:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreUpperRightCorner.texture;
                    break;
                case State.WaterType.ShoreOpenLowerLeft:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreOpenLowerLeftCorner.texture;
                    break;
                case State.WaterType.ShoreOpenLowerRight:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreOpenLowerRightCorner.texture;
                    break;
                case State.WaterType.ShoreOpenUpperLeft:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreOpenUpperLeftCorner.texture;
                    break;
                case State.WaterType.ShoreOpenUpperRight:
                    this.texture = TextureBank.getInstance().terrain.terrain.shoreOpenUpperRightCorner.texture;
                    break;
                default:
                    this.texture = TextureBank.getInstance().terrain.terrain.openWater.texture;
                    break;
            }

        }
    }
}

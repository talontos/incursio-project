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

namespace Incursio.Classes.Terrain
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
                    this.texture = TextureBank.MapTiles.openWater;
                    break;
                case State.WaterType.ShoreDown:
                    this.texture = TextureBank.MapTiles.shoreDown;
                    break;
                case State.WaterType.ShoreUp:
                    this.texture = TextureBank.MapTiles.shoreUp;
                    break;
                case State.WaterType.ShoreLeft:
                    this.texture = TextureBank.MapTiles.shoreLeft;
                    break;
                case State.WaterType.ShoreRight:
                    this.texture = TextureBank.MapTiles.shoreRight;
                    break;
                case State.WaterType.ShoreLowerLeft:
                    this.texture = TextureBank.MapTiles.shoreLowerLeftCorner;
                    break;
                case State.WaterType.ShoreLowerRight:
                    this.texture = TextureBank.MapTiles.shoreLowerRightCorner;
                    break;
                case State.WaterType.ShoreUpperLeft:
                    this.texture = TextureBank.MapTiles.shoreUpperLeftCorner;
                    break;
                case State.WaterType.ShoreUpperRight:
                    this.texture = TextureBank.MapTiles.shoreUpperRightCorner;
                    break;
                case State.WaterType.ShoreOpenLowerLeft:
                    this.texture = TextureBank.MapTiles.shoreOpenLowerLeftCorner;
                    break;
                case State.WaterType.ShoreOpenLowerRight:
                    this.texture = TextureBank.MapTiles.shoreOpenLowerRightCorner;
                    break;
                case State.WaterType.ShoreOpenUpperLeft:
                    this.texture = TextureBank.MapTiles.shoreOpenUpperLeftCorner;
                    break;
                case State.WaterType.ShoreOpenUpperRight:
                    this.texture = TextureBank.MapTiles.shoreOpenUpperRightCorner;
                    break;
                default:
                    this.texture = TextureBank.MapTiles.openWater;
                    break;
            }

        }
    }
}

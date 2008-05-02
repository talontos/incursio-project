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
    class Road : BaseMapEntity
    {
        public Road(int x, int y, State.RoadType type){
            this.passable = true;
            this.location = new global::Incursio.Utils.Coordinate(x, y);

            switch(type){
                case State.RoadType.Horizontal:
                    this.texture = TextureBank.MapTiles.roadHorizontal;
                    break;
                case State.RoadType.Vertical:
                    this.texture = TextureBank.MapTiles.roadVertical;
                    break;

                case State.RoadType.ElbowDownLeft:
                    this.texture = TextureBank.MapTiles.roadElbowDownLeft;
                    break;
                case State.RoadType.ElbowDownRight:
                    this.texture = TextureBank.MapTiles.roadElbowDownRight;
                    break;
                case State.RoadType.ElbowUpLeft:
                    this.texture = TextureBank.MapTiles.roadElbowUpLeft;
                    break;
                case State.RoadType.ElbowUpRight:
                    this.texture = TextureBank.MapTiles.roadElbowUpRight;
                    break;
                default:
                    break;
            };
        }
    }
}

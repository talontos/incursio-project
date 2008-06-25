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
    class Road : BaseMapEntity
    {
        public Road(int x, int y, State.RoadType type){
            this.passable = true;
            this.location = new global::Incursio.Utils.Coordinate(x, y);

            switch(type){
                case State.RoadType.Horizontal:
                    this.texture = TextureBank.getInstance().terrain.terrain.roadHorizontal.texture;
                    break;
                case State.RoadType.Vertical:
                    this.texture = TextureBank.getInstance().terrain.terrain.roadVertical.texture;
                    break;

                case State.RoadType.ElbowDownLeft:
                    this.texture = TextureBank.getInstance().terrain.terrain.roadElbowDownLeft.texture;
                    break;
                case State.RoadType.ElbowDownRight:
                    this.texture = TextureBank.getInstance().terrain.terrain.roadElbowDownRight.texture;
                    break;
                case State.RoadType.ElbowUpLeft:
                    this.texture = TextureBank.getInstance().terrain.terrain.roadElbowUpLeft.texture;
                    break;
                case State.RoadType.ElbowUpRight:
                    this.texture = TextureBank.getInstance().terrain.terrain.roadElbowUpRight.texture;
                    break;
                default:
                    break;
            };
        }
    }
}

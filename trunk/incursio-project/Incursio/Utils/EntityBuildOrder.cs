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
using Incursio.Classes;

namespace Incursio.Utils
{
    public class EntityBuildOrder
    {
        //TODO: IMPLEMENT BUILDING USING STRING, THIS WILL BE MORE EFFICIENT
        public BaseGameEntity entity;
        public Coordinate location;
        public KeyPoint keyPoint;

        public EntityBuildOrder(Coordinate l, BaseGameEntity e, KeyPoint k){
            location = l;
            entity = e;
            keyPoint = k;
        }

        public EntityBuildOrder(Coordinate l, BaseGameEntity e){
            location = l;
            entity = e;
        }
    }
}

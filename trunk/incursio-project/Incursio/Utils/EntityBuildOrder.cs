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
        public State.EntityName entity;
        public Coordinate location;
        public KeyPoint keyPoint;

        public EntityBuildOrder(Coordinate l, State.EntityName e, KeyPoint k)
        {
            location = l;
            entity = e;
            keyPoint = k;
        }

        public EntityBuildOrder(Coordinate l, State.EntityName e)
        {
            location = l;
            entity = e;
        }

        public EntityBuildOrder(Coordinate l, int entityId){

        }

        public EntityBuildOrder(Coordinate l, int entityId, KeyPoint k){

        }
    }
}

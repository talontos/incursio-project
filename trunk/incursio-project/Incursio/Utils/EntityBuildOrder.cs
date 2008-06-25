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

using Incursio.Entities;

namespace Incursio.Utils
{
    public class EntityBuildOrder
    {
        public string entity = "";
        public int entityId;
        public Coordinate location;
        public KeyPoint keyPoint;

        public EntityBuildOrder(Coordinate l, int entityId){
            //TODO: Lookup entityId & get name & set to 'entity'
            this.location = l;
            this.entityId = entityId;

            this.entity = this.lookupEntityName(ref entityId);
        }

        public EntityBuildOrder(Coordinate l, int entityId, KeyPoint k){
            //TODO: Lookup entityId & get name & set to 'entity'
            this.location = l;
            this.entityId = entityId;
            keyPoint = k;

            this.entity = this.lookupEntityName(ref entityId);
        }

        private string lookupEntityName(ref int id){
            foreach(BaseGameEntityConfiguration c in ObjectFactory.getInstance().entities){
                if (c.classID == id)
                    return c.className;
            }

            //TODO: Throw ERROR?
            return "";
        }
    }
}

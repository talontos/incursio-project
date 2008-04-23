using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;

namespace Incursio.Utils
{
    public class EntityBuildOrder
    {
        public Coordinate location;
        public BaseGameEntity entity;

        public EntityBuildOrder(Coordinate l, BaseGameEntity e){
            location = l;
            entity = e;
        }
    }
}

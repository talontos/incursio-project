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

using Microsoft.Xna.Framework;
using Incursio.Classes;

namespace Incursio.Utils
{
    public class GameEvent
    {
        public State.EventType type;
        public String audibleMessage;
        public String stringMessage;
        public Coordinate location;
        public int displayTick = 0;
        public BaseGameEntity entity;

        public GameEvent(State.EventType type, BaseGameEntity entity, String sound, String stringMsg, Coordinate loc){
            this.type = type;
            this.audibleMessage = sound;
            this.stringMessage = stringMsg;
            this.location = loc;
            this.entity = entity;
        }
    }
}

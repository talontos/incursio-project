using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace Incursio.Utils
{
    public class GameEvent
    {
        public State.EventType type;
        //public SOUND? audibleMessage;
        public String stringMessage;
        public Coordinate location;

        public GameEvent(State.EventType type, /*SOUND,*/ String stringMsg, Coordinate loc){
            this.type = type;
            //this.audibleMessage = sound?;
            this.stringMessage = stringMsg;
            this.location = loc;
        }
    }
}

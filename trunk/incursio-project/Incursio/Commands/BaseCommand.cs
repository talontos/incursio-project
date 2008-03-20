using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;

namespace Incursio.Commands
{
    class BaseCommand
    {
        public State.Command type = State.Command.NONE;
        public Boolean finishedExecution = false;
        
        public BaseCommand(){}

        public virtual State.Command getType(){
            return this.type;
        }

        public virtual void execute(ref BaseGameEntity subject){

        }
    }
}

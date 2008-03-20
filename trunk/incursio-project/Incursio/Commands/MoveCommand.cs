using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Utils;
using Incursio.Commands;

namespace Incursio.Commands
{
    class MoveCommand : BaseCommand
    {
        public Coordinate destination;

        public MoveCommand(Coordinate destination){
            this.destination = destination;
        }

        public override void execute(ref BaseGameEntity subject)
        {
            //move towards destination
             
        }
    }
}

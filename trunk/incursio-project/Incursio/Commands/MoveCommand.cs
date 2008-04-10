using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Utils;
using Incursio.Commands;

namespace Incursio.Commands
{
    public class MoveCommand : BaseCommand
    {
        public Coordinate destination;

        public MoveCommand(Coordinate destination){
            this.destination = destination;
        }

        public override void execute(ref BaseGameEntity subject)
        {
            //move towards destination

            if(subject.canMove || subject.isConstructor){
                //move unit
                subject.setDestination(this.destination);
                (subject as Unit).setCurrentState(State.UnitState.Moving);
                this.finishedExecution = subject.updateMovement();
            }
            else{
                //not a unit, do nothing?
                this.finishedExecution = true;
            }
        }
    }
}

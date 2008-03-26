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

            if(subject is Unit && subject.getPlayer() == State.PlayerId.HUMAN){ //U DUN WAN 2 MOV DA UNIT IF DA UNIT DUN BELON 2 U
                //move unit
                //do we have logic here, and actually move in Unit,
                // or do we do all movement/state changing, etc here?
                (subject as Unit).destination = this.destination;
                this.finishedExecution = (subject as Unit).updateMovement();
            }
            else{
                //not a unit, do nothing?
                this.finishedExecution = true;
            }
        }
    }
}

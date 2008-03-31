using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Utils;
using Incursio.Commands;

namespace Incursio.Commands
{
    public class AttackMoveCommand : BaseCommand
    {
        public Coordinate destination;

        public GuardCommand guard;
        public MoveCommand move;

        public AttackMoveCommand(Coordinate destination)
        {
            this.destination = destination;
            move = new MoveCommand(destination);
            guard = new GuardCommand();
        }

        public override void execute(ref BaseGameEntity subject)
        {
            move.execute(ref subject);
            guard.execute(ref subject);

            this.finishedExecution = move.finishedExecution || guard.finishedExecution;
        }
    }
}

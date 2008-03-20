using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Commands;

namespace Incursio.Commands
{
    class FollowCommand : BaseCommand
    {
        public BaseGameEntity followTarget;

        public FollowCommand(BaseGameEntity target){
            this.type = State.Command.FOLLOW;
            this.followTarget = target;
        }

        public override void execute(ref BaseGameEntity subject)
        {
            //if target is not dead, move toward target
            //otherwise, set finishedExecution = true
        }
    }
}

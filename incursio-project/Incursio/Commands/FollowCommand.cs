using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Commands;
using Microsoft.Xna.Framework;

namespace Incursio.Commands
{
    public class FollowCommand : BaseCommand
    {
        public BaseGameEntity followTarget;
        public MoveCommand moveCommand;

        public FollowCommand(BaseGameEntity target){
            this.type = State.Command.FOLLOW;
            this.followTarget = target;
            this.moveCommand = new MoveCommand(followTarget.getLocation());
        }

        private void updateMoveCommand(){
            this.moveCommand.destination = this.followTarget.getLocation();
        }

        public override void execute(GameTime gameTime, ref BaseGameEntity subject)
        {

            //if target is dead, set finishedExecution = true
            //otherwise, move toward target
            if (followTarget.getHealth() <= 0){
                this.finishedExecution = true;
            }
            else{
                this.updateMoveCommand();
                moveCommand.execute(gameTime, ref subject);
            }

        }
    }
}

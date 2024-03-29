/****************************************
 * Copyright � 2008, Team RobotNinja:
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


using Incursio.Commands;
using Microsoft.Xna.Framework;
using Incursio.Managers;
using Incursio.Entities;

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
            this.moveCommand = new MoveCommand(followTarget.getLocation());
        }

        public override void execute(GameTime gameTime, ref BaseGameEntity subject)
        {
            //if target is dead, set finishedExecution = true
            //otherwise, move toward target
            if (followTarget.isDead()){
                this.finishedExecution = true;
            }
            else{
                if(MapManager.getInstance().currentMap.getCellDistance(this.followTarget.location, subject.location) > 1){
                    //TODO: fix movement; there is a problem with this.
                    //  sometimes they'll just stand there
                    //this.updateMoveCommand();
                }

                moveCommand.execute(gameTime, ref subject);
            }

        }
    }
}

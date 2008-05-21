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

using Incursio.Classes;
using Incursio.Commands;
using Microsoft.Xna.Framework;
using Incursio.Managers;

namespace Incursio.Commands
{
    public class CaptureCommand : BaseCommand
    {
        public BaseGameEntity capTarget;
        public MoveCommand moveCommand;

        public CaptureCommand(BaseGameEntity target){
            this.type = State.Command.CAPTURE;
            this.capTarget = target;
            this.moveCommand = new MoveCommand(capTarget.getLocation());
        }

        public override void execute(GameTime gameTime, ref BaseGameEntity subject)
        {
            if( !(subject.isHero) ){
                this.finishedExecution = true;
                return;
            }

            //if target is dead, set finishedExecution = true
            //otherwise, move toward target
            if (capTarget.owner == subject.owner){
                this.finishedExecution = true;
            }
            else{
                if(MapManager.getInstance().currentMap.getCellDistance(subject.location, capTarget.location) >= 4){
                    moveCommand.execute(gameTime, ref subject);
                }
                else{
                    if(!capTarget.capturableComponent.isCapping()){
                        subject.currentState = State.EntityState.Capturing;
                        capTarget.capturableComponent.startCap(subject);
                    }
                }
            }

        }
    }
}

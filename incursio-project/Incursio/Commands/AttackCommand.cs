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


using Incursio.Utils;
using Incursio.Commands;
using Microsoft.Xna.Framework;
using Incursio.Entities;

namespace Incursio.Commands
{
    public class AttackCommand : BaseCommand
    {
        public BaseGameEntity target;
        public FollowCommand followCommand;

        public AttackCommand(BaseGameEntity target){
                this.type = State.Command.ATTACK;
                this.target = target;
                this.followCommand = new FollowCommand(target);
        }

        public override void execute(GameTime gameTime, ref BaseGameEntity subject)
        {
            if( !subject.canAttack ){
                this.finishedExecution = true;
                return;
            }

            if(target.isControlPoint){
                this.finishedExecution = true;
                subject.setIdle();
            }

            if(this.target.isDead()){
                this.finishedExecution = true;
                subject.setIdle();
                subject.issueAdditionalOrder(new GuardCommand());
            }
            else{
                //if subject is in attack range of target, set attack state & notify manager
                bool result = false;

                if(subject.canAttack){
                    subject.setTarget(target);
                    subject.setAttacking();
                    result = subject.attackTarget();
                }

                if(!result){
                    //subject is not in attack range, move toward target:
                    if (subject.canMove)
                        followCommand.execute(gameTime, ref subject);
                    else
                        this.finishedExecution = true;
                }
            }
        }
    }
}

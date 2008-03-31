using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Utils;
using Incursio.Commands;

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

        public override void execute(ref BaseGameEntity subject)
        {
            if( !subject.canAttack ){
                this.finishedExecution = true;
                return;
            }

            if(this.target.getHealth() <= 0){
                this.finishedExecution = true;
                subject.issueAdditionalOrder(new GuardCommand());
            }
            else{
                //if subject is in attack range of target, set attack state & notify manager
                bool result = false;

                //if(subject is Unit){
                    subject.setTarget(target);
                    result = subject.attackTarget();
                //}
                //else{
                    //TODO: REFACTOR CLASS FOR GUARD TOWER
                    //result = (subject as GuardTowerStructure).attackTarget();
                //}

                if(!result){
                    //subject is not in attack range, move toward target:
                    followCommand.execute(ref subject);
                }
            }
        }
    }
}

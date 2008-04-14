using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Commands;
using Incursio.Managers;
using Incursio.Classes;
using Microsoft.Xna.Framework;

namespace Incursio.Commands
{
    public class GuardCommand : BaseCommand
    {
        public GuardCommand(){
            this.type = State.Command.GUARD;
        }

        public override void execute(GameTime gameTime, ref BaseGameEntity subject)
        {
            //return;

            //TODO: REIMPLEMENT

            //look for enemy units within subject's sightRange
            List<BaseGameEntity> enemiesInRange = EntityManager.getInstance().getEntitiesInRange(ref subject, subject.sightRange);

            if(enemiesInRange.Count > 0){
                //TODO: Select a random enemy (possibly weight them)
                subject.issueImmediateOrder(new AttackCommand(enemiesInRange[ Incursio.rand.Next(0, enemiesInRange.Count) ]));
            }
        }
    }
}

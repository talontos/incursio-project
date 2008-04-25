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
            //look for enemy units within subject's sightRange
            List<BaseGameEntity> enemiesInRange, friendliesInRange;
            EntityManager.getInstance().getAllEntitiesInRange(ref subject, subject.sightRange, out friendliesInRange, out enemiesInRange);
            
            List<BaseGameEntity> formidableEnemies = this.analyzeEnemiesInRange(enemiesInRange, subject);

            if(formidableEnemies.Count > 0){

                //TODO: Select a random enemy (possibly weight them)
                BaseGameEntity e = formidableEnemies[Incursio.rand.Next(0, formidableEnemies.Count)];

                if( !(e is ControlPoint) )
                    subject.issueImmediateOrder(new AttackCommand(e));
            }
        }

        private List<BaseGameEntity> analyzeEnemiesInRange(List<BaseGameEntity> enemies, BaseGameEntity subject){

            //right now just use for guard towers - they'll attack anything
            if (!subject.smartGuarding)
                return enemies;

            List<BaseGameEntity> formidableEnemies = new List<BaseGameEntity>();
            int myDamage = subject.getAttackDamage();
            int myArmor = subject.getArmor();
            int myHealth = (int)subject.health;
            int myRange = subject.getAttackRange();
            int myAttackSpeed = subject.getAttackSpeed();

            //analyze enemies to determine who to attack
            enemies.ForEach(delegate(BaseGameEntity e)
            {
                switch(EntityManager.getInstance().analyzeThreat(ref myDamage, ref myArmor, 
                        ref myHealth, ref myRange, ref myAttackSpeed, ref e))
                {
                    case State.ThreatLevel.High:
                        //RUN MOFO
                        break;
                    case State.ThreatLevel.Medium:
                        //I can maybe take him
                        //just merge with Low:
                    case State.ThreatLevel.Low:
                        //I can take him
                        formidableEnemies.Add(e);
                        break;
                    case State.ThreatLevel.None:
                        break;
                }
            });

            return formidableEnemies;
        }
    }
}

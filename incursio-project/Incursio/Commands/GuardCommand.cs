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
            EntityManager.getInstance().getAllEntitiesInRange(subject.owner, subject.location, subject.sightRange, out friendliesInRange, out enemiesInRange);
            
            List<BaseGameEntity> formidableEnemies;

            if (subject is GuardTowerStructure || subject is HeavyInfantryUnit)
                formidableEnemies = enemiesInRange;
            else
                formidableEnemies = this.analyzeEnemiesInRange(enemiesInRange, subject);

            if(formidableEnemies.Count > 0){

                //TODO: Select a random enemy (possibly weight them)
                BaseGameEntity e = formidableEnemies[Incursio.rand.Next(0, formidableEnemies.Count)];

                if( !(e is ControlPoint) )
                    subject.issueImmediateOrder(new AttackCommand(e));
                else if( subject is Hero){
                    subject.issueImmediateOrder(new CaptureCommand(e as ControlPoint));
                }
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

            bool done = false;

            //analyze enemies to determine who to attack
            enemies.ForEach(delegate(BaseGameEntity e)
            {
                if(subject is Hero && e is ControlPoint && e.owner != subject.owner){
                    //capture the point!
                    formidableEnemies = new List<BaseGameEntity>();
                    formidableEnemies.Add(e);
                    done = true;
                }

                if(!done){
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
                }
            });

            return formidableEnemies;
        }
    }
}

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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Incursio.Managers;

namespace Incursio.Classes
{
    public class Structure : BaseGameEntity
    {
        public const int RESOURCE_TICK = 4;
        private bool playedDeathSound = false;
        //for building things
        protected int timeBuilt = 0;
        protected int timeRequired = 0;
        protected int timeForResource = 0;
        protected EntityBuildOrder buildProject;

        public Structure()
        {

        }

        public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
        {
            this.updateBounds();

            if (Incursio.getInstance().currentState == State.GameState.InPlay)
            {

                base.Update(gameTime, ref myRef);

                switch (this.currentState)
                {
                    /////////////////////
                    case State.EntityState.Idle:
                        //we're chillin
                        break;

                    /////////////////////
                    case State.EntityState.BeingBuilt:
                        beingBuiltTick();
                        break;

                    /////////////////////
                    case State.EntityState.Building:
                        buildTick();
                        break;

                    /////////////////////
                    case State.EntityState.Destroyed:

                        break;

                    /////////////////////
                    case State.EntityState.Attacking:
                        attackTarget();
                        break;

                    /////////////////////
                    default: break;
                }

                //THIS IS BEING DONE IN A COMPONENT
                //updateResourceTick();

                if(health <= 0 && !playedDeathSound){
                    playedDeathSound = true;
                    this.currentState = State.EntityState.Destroyed;
                    SoundManager.getInstance().PlaySound(SoundManager.getInstance().AudioCollection.attack.Explosion, false);
                }
            }
        }

        public State.EntityState getCurrentState()
        {
            return currentState;
        }

        public void setCurrentState(State.EntityState state)
        {
            this.currentState = state;
        }

        public virtual void build(EntityBuildOrder toBeBuilt)
        {
            this.buildProject = toBeBuilt;
        }

        public virtual void buildTick()
        {

        }

        public virtual bool isBuilding(){
            if (this.factoryComponent != null)
                return this.factoryComponent.isBuilding();
            else
                return false;
        }

        public virtual void beingBuiltTick()
        {

        }

        public override void takeDamage(int damage, BaseGameEntity attacker)
        {
            //this.health -= damage;
            base.takeDamage(damage, attacker);

            if (this.health <= 0)
            {
                this.health = 0;
                EntityManager.getInstance().removeEntity(this.keyId);
            }

            notifyUnderAttack();
        }

        public override bool isDead()
        {
            return currentState == State.EntityState.Destroyed;
        }

        public virtual void updateResourceTick()
        {

        }

        protected override void notifyUnderAttack()
        {
            PlayerManager.getInstance().notifyPlayer(
                this.owner,
                new GameEvent(State.EventType.UNDER_ATTACK, this, SoundManager.getInstance().AudioCollection.messages.baseAttack, "Base Under Attack!", this.location)
            );
        }
    }
}

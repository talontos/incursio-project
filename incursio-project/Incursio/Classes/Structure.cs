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

        //for building things
        protected int timeBuilt = 0;
        protected int timeRequired = 0;
        protected int timeForResource = 0;
        protected EntityBuildOrder buildProject;

        protected State.StructureState currentState;
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
                    case State.StructureState.Idle:
                        //we're chillin
                        break;

                    /////////////////////
                    case State.StructureState.BeingBuilt:
                        beingBuiltTick();
                        break;

                    /////////////////////
                    case State.StructureState.Building:
                        buildTick();
                        break;

                    /////////////////////
                    case State.StructureState.Destroyed:

                        break;

                    /////////////////////
                    case State.StructureState.Attacking:
                        attackTarget();
                        break;

                    /////////////////////
                    default: break;
                }

                updateResourceTick();
            }
        }

        public State.StructureState getCurrentState()
        {
            return currentState;
        }

        public void setCurrentState(State.StructureState state)
        {
            this.currentState = state;
        }
/*
        public virtual void build(BaseGameEntity toBeBuilt)
        {
            this.buildProject = toBeBuilt;
        }
*/
        public virtual void build(EntityBuildOrder toBeBuilt)
        {
            this.buildProject = toBeBuilt;
        }

        public virtual void buildTick()
        {

        }

        public virtual bool isBuilding(){
            return this.buildProject != null;
        }

        public virtual void beingBuiltTick()
        {

        }

        public virtual void setMap(MapBase map)
        {
            this.map = map;
        }

        public override void takeDamage(int damage, BaseGameEntity attacker)
        {
            //TODO: some math using my armor
            //this.health -= damage;
            base.takeDamage(damage, attacker);

            if (this.health <= 0)
            {
                this.health = 0;
                EntityManager.getInstance().removeEntity(this.keyId);
                //SoundManager.getInstance().PlaySound("../../../Content/Audio/explosion.wav", false);
            }

            notifyUnderAttack();
        }

        public override bool isDead()
        {
            return currentState == State.StructureState.Destroyed;
        }

        public virtual void updateResourceTick()
        {

        }

        protected override void notifyUnderAttack()
        {
            PlayerManager.getInstance().notifyPlayer(
                this.owner,
                new GameEvent(State.EventType.UNDER_ATTACK, this, SoundCollection.MessageSounds.baseAttack, "Base Under Attack!", this.location)
            );
        }
    }
}

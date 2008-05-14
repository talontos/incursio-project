using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;
using Incursio.Managers;

namespace Incursio.Entities.Components
{
    public class CombatComponent : BaseComponent
    {
        public bool smartGuarding = true;
        public int damage = 0;
        public int updateAttackTimer = 0;
        public int attackSpeed = 3;     //temp default
        public int attackRange = 2;     //temp default
        public BaseGameEntity target;

        public CombatComponent(BaseGameEntity entity) : base(entity){
            entity.canAttack = true;
        }

        public override void setAttributes(List<KeyValuePair<string, object>> attributes)
        {
            base.setAttributes(attributes);

            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i].Key)
                {
                    case "damage":      damage      = (int)attributes[i].Value; break;
                    case "attackSpeed": attackSpeed = (int)attributes[i].Value; break;
                    case "attackRange": attackRange = (int)attributes[i].Value; break;
                    default: break;
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime); 
        }

        public bool attackTarget()
        {
            if(target == null){
                return true;
            }

            //if target is in attackRange, attack it.

            int largeTargetBufferZone = 0;

            if (target.getType() == State.EntityName.Camp)
            {
                largeTargetBufferZone = (int)(64 / MapManager.TILE_WIDTH);
            }
            else if (target.getType() == State.EntityName.GuardTower)
            {
                largeTargetBufferZone = (int)(64 / MapManager.TILE_WIDTH);
            }

            if (MapManager.getInstance().currentMap.getCellDistance(this.bgEntity.location, target.location) <= attackRange + largeTargetBufferZone)
            {
                if (this.updateAttackTimer == 0)    //this is the unit's attack time (attack every 1.5 seconds for example)
                {
                    if (target.getLocation().x > this.bgEntity.location.x)
                    {
                        (this.bgEntity as Unit).setDirectionState(State.Direction.East);
                    }
                    else if (target.getLocation().x < this.bgEntity.location.x)
                    {
                        (this.bgEntity as Unit).setDirectionState(State.Direction.West);
                    }

                    //this.bgEntity.playAttackSound();
                    target.takeDamage(this.damage, this.bgEntity);

                    //if we just killed the thing

                    //if (target is Unit && (target as Unit).getCurrentState() == State.UnitState.Dead ||
                    //   target is Structure && (target as Structure).getCurrentState() == State.EntityState.Destroyed)
                    if (target.isDead())
                    {
                        //NOTE: killedTarget needs to be performed BEFORE
                        //  target is set to null so that we know WHAT we killed
                        this.bgEntity.killedTarget();

                        target = null;
                        //destination = null;
                        this.bgEntity.setIdle();
                    }

                    this.updateAttackTimer = this.attackSpeed * 60;
                }
                else
                {
                    this.updateAttackTimer--;
                }

                return true;
            }
            else return false;
        }
    }
}

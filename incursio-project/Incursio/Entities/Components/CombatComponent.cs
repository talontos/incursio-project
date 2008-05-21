using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;
using Incursio.Managers;
using Incursio.Entities.Projectiles;
using Incursio.Utils;

namespace Incursio.Entities.Components
{
    public class CombatComponent : BaseComponent
    {
        public bool smartGuarding = true;
        public int damage = 0;
        public int updateAttackTimer = 0;
        public int attackSpeed = 0;
        public int attackRange = 0;
        public BaseProjectile projectile;
        public BaseGameEntity target;

        public CombatComponent(BaseGameEntity entity) : base(entity){
            entity.canAttack = true;
        }

        public override void setAttributes(List<KeyValuePair<string, string>> attributes)
        {
            base.setAttributes(attributes);

            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i].Key)
                {
                    case "damage":          damage          = int.Parse(attributes[i].Value); break;
                    case "attackSpeed":     attackSpeed     = int.Parse(attributes[i].Value); break;
                    case "attackRange":     this.setAttackRange(int.Parse(attributes[i].Value)); break;
                    case "smartGuarding":   smartGuarding   = bool.Parse(attributes[i].Value); break;
                    default: break;
                }
            }
        }

        private void setAttackRange(int range){
            this.attackRange = range;

            if(this.attackRange > 1){
                //ranged, need projectile
                this.projectile = new BaseProjectile();
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if(projectile != null){
                projectile.Update();
            }
        }

        public bool attackTarget()
        {
            if(target == null){
                return true;
            }

            //if target is in attackRange, attack it.

            int largeTargetBufferZone = 0;

            //TODO: REDO THIS BUFFER ZONE CALCULATION
            if(target.movementComponent == null){
                largeTargetBufferZone = (int)(64 / MapManager.TILE_WIDTH);
            }

            if (MapManager.getInstance().currentMap.getCellDistance(this.bgEntity.location, target.location) <= attackRange + largeTargetBufferZone)
            {
                if (this.updateAttackTimer == 0)    //this is the unit's attack time (attack every 1.5 seconds for example)
                {
                    if (target.getLocation().x > this.bgEntity.location.x)
                    {
                        this.bgEntity.renderComponent.directionState = State.Direction.East;
                    }
                    else if (target.getLocation().x < this.bgEntity.location.x)
                    {
                        this.bgEntity.renderComponent.directionState = State.Direction.West;
                    }

                    this.bgEntity.playAttackSound();

                    //TODO: DRAW ARROWS
                    if(this.attackRange > 1){
                        //shoot your projectile!
                        this.projectile.startProjectile(bgEntity.location.toVector2(), target.location.toVector2());
                        target.takeDamage(this.damage, this.bgEntity);
                    }
                    else{
                        target.takeDamage(this.damage, this.bgEntity);
                    }
                    //if we just killed the thing

                    //if (target is Unit && (target as Unit).getCurrentState() == State.UnitState.Dead ||
                    //   target is Structure && (target as Structure).getCurrentState() == State.EntityState.Destroyed)
                    if (target.isDead())
                    {
                        //NOTE: killedTarget needs to be performed BEFORE
                        //  target is set to null so that we know WHAT we killed
                        this.bgEntity.killedTarget(ref target);

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

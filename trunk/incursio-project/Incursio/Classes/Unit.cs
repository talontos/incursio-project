using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Managers;
using Incursio.Commands;
using Incursio.Classes.PathFinding;

namespace Incursio.Classes
{
    public class Unit : MovableObject
    {
        protected const int TIME_DEAD_UNTIL_DESPAWN = 5;

        protected int damage = 0;
        protected int armor = 0;
        //protected int speed = 0;
        protected int attackSpeed = 0;
        protected int updateAttackTimer = 0;
        protected int attackRange = 0;
        protected int deadTimer = 0;
        public bool playedDeath = false;
        protected State.UnitState currentState = State.UnitState.Idle;
        protected State.Direction directionState = State.Direction.Still;
        protected bool isClose = false;

        private Coordinate destination = null;
        public BaseGameEntity target = null;

        //TEXTURES
        public List<Texture2D> textureAnim_N;
        public List<Texture2D> textureAnim_S;
        public List<Texture2D> textureAnim_E;
        public List<Texture2D> textureAnim_W;
        public int textureIndex;

        public Unit() : base(){
            canAttack = true;
            canMove = true;
        }

        public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
        {
            this.updateBounds();

            //only perform actions when we are actively playing a map
            if (Incursio.getInstance().currentState == State.GameState.InPlay)
            {
                if(this.currentState == State.UnitState.Dead){
                    die();
                    return;
                }

                //base update will execute commands
                base.Update(gameTime, ref myRef);

                //in case I go idle; look for bad guys
                if (orders.Count == 0)
                    EntityManager.getInstance().issueCommand_SingleEntity(State.Command.GUARD, false, this);

                if(!this.isDead()){
                    updateOccupancy(true);
                }
            }
        }

        //TODO: We need to somehow take into account the game time to allow for smoother movement
        public override bool updateMovement(float ElapsedTime)
        {

            this.updateOccupancy(false);

            //move
            bool retVal = base.updateMovement(ElapsedTime);

            this.updateOccupancy(true);

            //MOVED TO BOTTOM
            //return retVal;
            
            
            float xMinimumThreshold = 0.05F;
            float yMinimumThreshold = 0.05F;

            //get the direction to the target
            Vector2 direction = new Vector2(destination.x - location.x, destination.y - location.y);

            if (retVal) //movement finished
            {
                currentState = State.UnitState.Idle;
            }
            else
            {
                float xDirection = Vector2.Normalize(direction).X;
                float yDirection = Vector2.Normalize(direction).Y;

                //int newX = location.x;
                //int newY = location.y;

                if (xDirection > xMinimumThreshold && (xDirection / yDirection) >= 1)
                {
                    //newX += speed;
                    this.directionState = State.Direction.East;
                }
                else if (xDirection < -xMinimumThreshold && (xDirection / yDirection) <= -1)
                {
                    //newX += -1 * speed;
                    this.directionState = State.Direction.West;
                }
                else
                {
                    //newX = destination.x;
                }

                if (yDirection > yMinimumThreshold && ((Math.Abs(xDirection)) / yDirection) < 1)
                {
                    //newY += speed;
                    this.directionState = State.Direction.South;
                }
                else if (yDirection < -yMinimumThreshold && (xDirection / yDirection) < 1)
                {
                    //newY += -1 * speed;
                    this.directionState = State.Direction.North;
                }
                else
                {
                    //newY = destination.y;
                }
                /*
                if(!Incursio.getInstance().currentMap.requestMove(location.x, location.y, newX, newY)){
                    //new point is occupied; we have to turn!
                    this.currentState = State.UnitState.Idle;

                    //TODO: PATHING, insert move commands in my queue
                    return true;
                }
                else{
                    //open up our old space
                    MapManager.getInstance().currentMap.setSingleCellOccupancy(location.x, location.y, 1);

                    //move
                    location = new Coordinate(newX, newY);

                    //set occupancy
                    MapManager.getInstance().currentMap.setSingleCellOccupancy(location.x, location.y, 0);

                    return false;
                }
                */
            }

            return retVal;
            
        }

        /// <summary>
        /// Changes the unit's state and destination so that 
        /// it will move toward that destination
        /// </summary>
        /// <param name="destination">coordinate to move toward</param>
        public void move(Coordinate destination){
            this.currentState = State.UnitState.Moving;

            this.destination = destination;
            this.target = null;
        }

        /// <summary>
        /// Movement for maps larger than the present screen size
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="currentMap"></param>
        public void move(Coordinate destination, MapBase currentMap)
        {
            this.currentState = State.UnitState.Moving;

            //convert the destination from on screen coords to map coords
            this.destination = new Coordinate(destination.x + (currentMap.getMinimumX() * currentMap.getTileWidth()), destination.y + (currentMap.getMinimumY() * currentMap.getTileHeight()));
            this.target = null;
            this.map = currentMap;
            //this.destination = destination;
        }

        /// <summary>
        /// Changes the unit's state and target so that
        /// it will move toward that entity; essentially following it.
        /// 
        /// If target entity is an enemy, the unit will move to attack it
        /// </summary>
        /// <param name="targetEntity">The entity to follow</param>
        public void move(BaseGameEntity targetEntity){
            if (targetEntity.getPlayer() != owner)
                this.currentState = State.UnitState.Attacking;
            else
                this.currentState = State.UnitState.Moving;

            this.target = targetEntity;
        }

        /// <summary>
        /// Changes the unit's state and destination so that
        /// it will move toward that destination and attack
        /// enemy entities on the way
        /// </summary>
        /// <param name="targetCoord">Target coordinates</param>
        public void attack(Coordinate targetCoord){
            this.currentState = State.UnitState.Attacking;
            this.destination = targetCoord;
        }

        /// <summary>
        /// Changes the unit's state and target so that
        /// it will move toward that entity.  When close, 
        /// the unit will attack the target entity
        /// </summary>
        /// <param name="targetEntity">Target entity</param>
        public virtual void attack(BaseGameEntity targetEntity)
        {
            /*this.currentState = State.UnitState.Attacking;
            this.target = targetEntity;*/
            if (targetEntity is Unit && ((targetEntity as Unit).getCurrentState() != State.UnitState.Dead && (targetEntity as Unit).getCurrentState() != State.UnitState.Buried))
            {
                this.currentState = State.UnitState.Attacking;
                this.target = targetEntity;
            }
            else if (targetEntity is Structure && (targetEntity as Structure).getCurrentState() != State.StructureState.Destroyed)
            {
                this.currentState = State.UnitState.Attacking;
                this.target = targetEntity;
            }
        }

        //Getters & Setters
        public override void setLocation(Coordinate coords)
        {
            //empty our current location
            Incursio.getInstance().currentMap.setSingleCellOccupancy(location.x, location.y, 0);
            base.setLocation(coords);
            //occupy new location
            Incursio.getInstance().currentMap.setSingleCellOccupancy(location.x, location.y, 1);
        }

        //since units can take damage and then give damage, we'll set up a function just for them
        public override void takeDamage(int damage, BaseGameEntity attacker)
        {
            //TODO: some math using my armor
            this.health -= damage;

            if (this.health <= 0)
            {
                this.health = 0;
                currentState = State.UnitState.Dead;
            }
            else
            {
                //help, help, I'm being attacked!
                //oh well, en garde!

                //We should only do this if they aren't already attacking - that way they won't 
                //constantly switch targets in a big battle
                if(currentState != State.UnitState.Attacking){
                    this.issueImmediateOrder(new AttackCommand(attacker));
                    this.setAttacking();
                    PlayerManager.getInstance().notifyPlayer(
                        this.owner,
                        new GameEvent(State.EventType.UNDER_ATTACK, /*SOUND,*/ "Unit under attack", this.location)
                    );
                }
            }
        }

        public long getDamage(){
            return this.damage;
        }

        public void setDamage(int damage){
            this.damage = damage;
        }

        public long getArmor()
        {
            return this.armor;
        }

        public void setArmor(int armor)
        {
            this.armor = armor;
        }

        public State.UnitState getCurrentState()
        {
            return this.currentState;
        }

        public State.Direction getDirection()
        {
            return this.directionState;
        }

        public void setCurrentState(State.UnitState newState)
        {
            this.currentState = newState;
        }

        public void setMap(MapBase map)
        {
            this.map = map;
        }

        //Private helper functions//
        /// <summary>
        /// If target is in range, attack it.  otherwise, move toward it.
        /// </summary>
        public override bool attackTarget(){
            //if target is in attackRange, attack it.

            int largeTargetBufferZone = 0;

            if (target.getType() == State.EntityName.Camp)
            {
                largeTargetBufferZone = (int)(64 / map.getTileWidth());
            }
            else if (target.getType() == State.EntityName.GuardTower)
            {
                largeTargetBufferZone = (int)(64 / map.getTileWidth());
            }

            if(MapManager.getInstance().currentMap.getCellDistance(location, target.location) <= attackRange + largeTargetBufferZone){
                //TODO: do some math randomizing damage?
                if (this.updateAttackTimer == this.attackSpeed * 60)    //this is the unit's attack time (attack every 1.5 seconds for example)
                {
                    //basically, if we are attacking something that can attack back
                    /*if (target.getType() == State.EntityName.Archer || target.getType() == State.EntityName.LightInfantry || 
                        target.getType() == State.EntityName.Hero   || target.getType() == State.EntityName.HeavyInfantry)
                    {
                        (target as Unit).takeDamage(this.damage, this);

                        //if we just killed the unit
                        if ((target as Unit).getCurrentState() == State.UnitState.Dead)
                        {
                            //TODO:
                            //add AI for attacking more enemies!
                            //but for now:
                            target = null;
                            currentState = State.UnitState.Idle;
                        }
                    }
                    else if (target.getType() == State.EntityName.GuardTower)
                    {
                        //since guard towers are a little different, different notice
                        (target as Structure).takeDamage(this.damage);
                    }
                    else
                    {
                        target.takeDamage(this.damage);
                    }*/

                    target.takeDamage(this.damage, this);

                    //if we just killed the thing

                    //if (target is Unit && (target as Unit).getCurrentState() == State.UnitState.Dead ||
                    //   target is Structure && (target as Structure).getCurrentState() == State.StructureState.Destroyed)
                    if(target.isDead())
                    {
                        //TODO:
                        //add AI for attacking more enemies!
                        //but for now:

                        //NOTE: killedTarget needs to be performed BEFORE
                        //  target is set to null so that we know WHAT we killed
                        this.killedTarget();

                        target = null;
                        destination = null;
                        currentState = State.UnitState.Idle;
                    }


                    this.updateAttackTimer = 0;
                }
                else
                {
                    this.updateAttackTimer++;
                }

                return true;
            }
            else return false;// updateMovement();
        }

        public override bool isDead()
        {
            return currentState == State.UnitState.Dead || currentState == State.UnitState.Buried;
        }

        protected virtual void die(){
            currentState = State.UnitState.Dead;
            if (orders.Count != 0)
                orders = new List<BaseCommand>();

            //TODO: How to handle death of entities?
            //  It'd be a waste of memory to just leave them in the entityBank,
            //    but then we'd have to reassign keyIds.
            //  But do we need to remove them?
            if (deadTimer == TIME_DEAD_UNTIL_DESPAWN * 60)
            {
                map.setSingleCellOccupancy(location.x, location.y, 1);
                EntityManager.getInstance().removeEntity(keyId);
                deadTimer++;
            }
            else
            {
                deadTimer++;
            }
        }

        public override Texture2D getCurrentTexture()
        {
            switch(this.directionState){
                case State.Direction.North:
                    return textureAnim_N[textureIndex];

                case State.Direction.South:
                    return textureAnim_S[textureIndex];

                case State.Direction.East:
                    return textureAnim_E[textureIndex];

                case State.Direction.West:
                    return textureAnim_W[textureIndex];
            }

            return null;
        }

        protected virtual void updateDirectionTexture(){
            switch(directionState){
                //TODO: reload my texture depending on my directionState
            }
        }

        public override void setTarget(BaseGameEntity target)
        {
            this.target = target;
        }

        public override void setDestination(Coordinate dest)
        {
            this.destination = dest;
        }

        public override void setAttacking()
        {
            this.currentState = State.UnitState.Attacking;
        }

        public override void setIdle()
        {
            this.currentState = State.UnitState.Idle;
        }
    }
}

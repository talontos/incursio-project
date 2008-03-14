using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;

namespace Incursio.Classes
{
    public class Unit : BaseGameEntity
    {
        protected long damage = 0;
        protected long armor = 0;
        protected int speed = 0;
        protected int attackRange = 0;
        protected State.UnitState currentState = State.UnitState.Idle;
        protected MapBase map;
        protected bool isClose = false;

        public Coordinate destination = null;
        public BaseGameEntity target = null;

        public Unit() : base(){

        }

        public override void Update(GameTime gameTime)
        {
            //check health for death...
            if (health <= 0)
                die();

            //only perform actions when we are actively playing a map
            if (Incursio.getInstance().currentState == State.GameState.InPlay)
            {
                //check state and act accordingly
                switch (this.currentState)
                {

                    ///////////////////////////////
                    case State.UnitState.Attacking:
                        this.destination = this.target.getLocation();

                        //if target is in range, attack.  otherwise move toward enemy
                        if (Math.Sqrt((destination.x - location.x) ^ 2 + (destination.y - location.y) ^ 2) <= attackRange)
                        {
                            //attack!!!
                            attackTarget();
                            break;
                        }
                        else updateMovement();
                        break;

                    ///////////////////////////////
                    case State.UnitState.Moving:

                        if (this.destination == null)
                            this.currentState = State.UnitState.Idle;
                        else
                        {
                            updateMovement();
                            //recalculateLocation();
                        }

                        break;

                    ///////////////////////////////
                    case State.UnitState.Wandering:
                        this.move(new Coordinate(Incursio.rand.Next(0, 1024), Incursio.rand.Next(0, 768)));
                        break;

                    ///////////////////////////////
                    case State.UnitState.Idle:
                        //TODO: change; this is temporary
                        //this.currentState = State.UnitState.Wandering;
                        break;

                    ///////////////////////////////

                    default: break;
                }
            }
        }

        public void updateMovement()
        {
            float xMinimumThreshold = 0.05F;
            float yMinimumThreshold = 0.05F;

            //get the direction to the target
            Vector2 direction = new Vector2(destination.x - location.x, destination.y - location.y);

            if (direction.Length() < speed)
            {
                destination = location;
            }
            else
            {
                float xDirection = Vector2.Normalize(direction).X;
                float yDirection = Vector2.Normalize(direction).Y;

                int newX = location.x;
                int newY = location.y;

                if (xDirection > xMinimumThreshold)
                {
                    newX += speed;
                }
                else if (xDirection < -xMinimumThreshold)
                {
                    newX += -1 * speed;
                }
                else
                {
                    newX = destination.x;
                }

                if (yDirection > yMinimumThreshold)
                {
                    newY += speed;
                }
                else if (yDirection < -yMinimumThreshold)
                {
                    newY += -1 * speed;
                }
                else
                {
                    newY = destination.y;
                }

                if(!Incursio.getInstance().currentMap.requestMove(location.x, location.y, newX, newY)){
                    //new point is occupied; we have to turn!
                    this.currentState = State.UnitState.Idle;
                }
                else{
                    //open up our old space
                    Incursio.getInstance().currentMap.setSingleCellOccupancy(location.x, location.y, true);

                    //move
                    location = new Coordinate(newX, newY);

                    //set occupancy
                    Incursio.getInstance().currentMap.setSingleCellOccupancy(location.x, location.y, false);
                }
            }
        }

        /// <summary>
        /// Changes the unit's state and destination so that 
        /// it will move toward that destination
        /// </summary>
        /// <param name="destination">coordinate to move toward</param>
        public void move(Coordinate destination){
            this.currentState = State.UnitState.Moving;

            this.destination = destination;
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
            this.currentState = State.UnitState.Attacking;
            this.target = targetEntity;
        }

        //Getters & Setters
        public override void setLocation(Coordinate coords)
        {
            //empty our current location
            Incursio.getInstance().currentMap.setSingleCellOccupancy(location.x, location.y, false);
            base.setLocation(coords);
            //occupy new location
            Incursio.getInstance().currentMap.setSingleCellOccupancy(location.x, location.y, true);
        }

        public long getDamage(){
            return this.damage;
        }

        public void setDamage(long damage){
            this.damage = damage;
        }

        public long getArmor()
        {
            return this.armor;
        }

        public void setArmor(long armor)
        {
            this.armor = armor;
        }

        public State.UnitState getCurrentState()
        {
            return this.currentState;
        }

        public void setCurrentState(State.UnitState newState)
        {
            this.currentState = newState;
        }

        //Private helper functions//
        private void recalculateLocation()
        {
            if (this.target != null)
            {
                //move SPEED units toward target's current location
                this.destination = this.target.getLocation();
            }

            //TODO: condense this code so as not to declare so many variables...
            double x2 = this.destination.x;
            double y2 = this.destination.y;

            //Atan is in radians...convert to degrees
            double theta = (180 * Math.Atan((y2 - location.y) / (x2 - location.x))) / Math.PI;
            if (Double.IsNaN(theta))
                theta = 0;

            //now we have the angle between our location and destination
            //find destination
            x2 = this.speed * Math.Cos(theta);
            y2 = this.speed * Math.Sin(theta);

            //colision detection time!
            //if (x2,y2) is not passable, we can't go there...
            //TODO: Colision detection

            int ix = Convert.ToInt32(x2);
            int iy = Convert.ToInt32(y2);

            if(Incursio.getInstance().currentMap.getCellOccupancy(ix, iy)){
                //cell is occupied, we can't go there.
                this.currentState = State.UnitState.Idle;
                return;
            }

            //open up our previous occupancy
            Incursio.getInstance().currentMap.setSingleCellOccupancy(this.location.x, this.location.y, false);

            //our final location
            this.location.x = location.x + ix;
            this.location.y = location.y + iy;

            //set our current occupancy
            Incursio.getInstance().currentMap.setSingleCellOccupancy(this.location.x, this.location.y, true);

            //If destination is within our bound, go idle
            if (location.x <= (destination.x + DebugUtil.UnitStopMoveRange) && location.y <= (destination.y + DebugUtil.UnitStopMoveRange) &&
                 location.x >= (destination.x - DebugUtil.UnitStopMoveRange) && location.y >= (destination.y - DebugUtil.UnitStopMoveRange) &&
                !isClose)
            {
                isClose = true;
            }

            if (isClose)
            {
                if (this.location.x == this.destination.x && this.location.y == this.destination.y)
                {
                    isClose = false;
                    this.currentState = State.UnitState.Idle;
                }
                else
                {
                    if (this.location.x - this.destination.x <= this.speed)
                    {
                        this.location.x = this.destination.x;
                    }
                    else if (this.destination.x - this.location.x <= this.speed)
                    {
                        this.location.x = this.destination.x;
                    }
                    else
                    {
                        if (this.location.x - this.destination.x < 0)
                        {
                            this.location.x = location.x - this.speed;
                        }
                        else
                        {
                            this.location.x = location.x + this.speed;
                        }
                    }

                    if (this.location.y - this.destination.y <= this.speed)
                    {
                        this.location.y = this.destination.y;
                    }
                    else if (this.destination.y - this.location.y <= this.speed)
                    {
                        this.location.y = this.destination.y;
                    }
                    else
                    {
                        if (this.location.y - this.destination.y < 0)
                        {
                            this.location.y = location.y - this.speed;
                        }
                        else
                        {
                            this.location.y = location.y + this.speed;
                        }
                    }
                }  
            }
                

            //If unit is now on the destination tile, go idle
            //NOTE: both this conditional statement and the one above it are the same, want to use the simpler statement?
            /*if (location.x / map.getTileWidth() == destination.x / map.getTileWidth() && location.y / map.getTileHeight() == destination.y / map.getTileHeight())
            {
                this.currentState = State.UnitState.Idle;
            }*/
        }

        protected virtual void attackTarget(){
            //um..attack!
        }

        protected virtual void die(){
            currentState = State.UnitState.Dead;
            //TODO: How to handle death of entities?
            //  It'd be a waste of memory to just leave them in the entityBank,
            //    but then we'd have to reassign keyIds.
            //  But do we need to remove them?
        }


    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Commands;
using Incursio.Managers;

namespace Incursio.Classes
{
    public class BaseGameEntity
    {
        //protected String type = "";    //unit type-name?

        public State.EntityName entityType;
        public long maxHealth = 100;
        public long health = 100;
        public int  sightRange = 0;
        public State.PlayerId owner;
        public Coordinate location = new Coordinate(0,0);
        protected MapBase map;
        public int keyId = -1;
        public bool visible = false;
        public bool highlighted = false;
        public bool canAttack = false;
        public bool canMove = false;
        public bool isConstructor = false;
        public bool justDrawn = false;
        public int currentFrameX = 0;       //for animation
        public int currentFrameY = 0;       //for animation

        public Rectangle boundingBox;

        public int pointValue = 0;

        protected List<BaseCommand> orders;

        public BaseGameEntity(){
            orders = new List<BaseCommand>();
        }

        /// <summary>
        /// General update function for updating entities
        /// </summary>
        /// <param name="gameTime">Game time passed from main loop</param>
        /// <param name="myRef">Mostly a hack, used for executing commands.  It is a reference of 'this'</param>
        public virtual void Update(GameTime gameTime, ref BaseGameEntity myRef){
            //TODO: draw unit here?
            //TODO: check if i'm clicked?

            if (this.isDead())
                return;

            if (orders.Count > 0){
                //check validity of next command
                if (orders[0].finishedExecution)
                    orders.RemoveAt(0);

            }

            //if I still have orders...
            if(orders.Count > 0){
                orders[0].execute(gameTime, ref myRef);

                //check type for player notification

            }
        }

        /// <summary>
        /// Empties the entity's command list, and inserts 'order'
        /// so that it is its only order
        /// </summary>
        /// <param name="order">The new command</param>
        public virtual void issueSingleOrder(BaseCommand order)
        {
            this.orders = new List<BaseCommand>();
            this.orders.Add(order);
        }

        /// <summary>
        /// Replaces the entity's command list with another list
        /// of commands
        /// </summary>
        /// <param name="commands">The new commands</param>
        public virtual void issueOrderList(params BaseCommand[] commands)
        {
            this.orders = new List<BaseCommand>();
            if(commands != null)this.orders.AddRange(commands);
        }

        /// <summary>
        /// Adds a new command to the end of the entity's command list
        /// </summary>
        /// <param name="order">The new command</param>
        public virtual void issueAdditionalOrder(BaseCommand order){
            this.orders.Add(order);
        }

        /// <summary>
        /// Adds a new command to the beginning of the entity's command list
        /// </summary>
        /// <param name="order">The new important command</param>
        public virtual void issueImmediateOrder(BaseCommand order)
        {
            this.orders.Insert(0, order);
        }

        public virtual Texture2D getCurrentTexture(){
            //check state & return appropriate texture from TextureBank.EntityTextures
            return null;
        }

        public virtual void takeDamage(int damage, BaseGameEntity attacker){
            //TODO: some math using my armor
            this.health -= damage;
            if (this.health < 0)
            {
                this.health = 0;
            }
        }

        public virtual String getTextureName(){
            return null;
        }

        public virtual bool isDead(){
            return health <= 0;
        }

        public virtual void updateBounds(){

        }

        public virtual bool attackTarget(){
            return false;
        }

        public virtual void killedTarget(){

        }

        /// <summary>
        /// Virtual function for moving
        /// </summary>
        /// <returns>True when destination is reached.  By default returns true.</returns>
        public virtual bool updateMovement(float ElapsedTime){
            return true;
        }

        /// <summary>
        /// Sets the location of this entity in the current map as occupied (true) or unocupied (false)
        /// </summary>
        /// <param name="occupied"></param>
        public virtual void updateOccupancy(bool occupied){
            MapManager.getInstance().currentMap.setSingleCellOccupancy(location.x, location.y,
                (occupied ? (byte)0 : (byte)1));
        }

        #region Getters/Setters
        public virtual void setType(State.EntityName name)
        {
            this.entityType = name;
        }

        public virtual State.EntityName getType()
        {
            return this.entityType;
        }

        public virtual long getHealth(){
            return this.health;
        }

        public virtual void setHealth(int health){
            this.health = health;
        }

        public virtual long getMaxHealth()
        {
            return this.maxHealth;
        }

        public virtual void setMaxHealth(int newHealth)
        {
            this.maxHealth = newHealth;
        }

        public virtual State.PlayerId getPlayer(){
            return this.owner;
        }

        public virtual void setPlayer(State.PlayerId owner){
            this.owner = owner;
        }

        public virtual long getSightRange(){
            return this.sightRange;
        }

        public virtual void setSightRange(int sightRange){
            this.sightRange = sightRange;
        }

        public virtual Coordinate getLocation()
        {
            return this.location;
        }

        public virtual void setLocation(Coordinate coords)
        {
            this.updateOccupancy(false);
            this.location = coords;
            this.updateOccupancy(true);
        }

        public virtual int getKeyId(){
            return this.keyId;
        }

        public virtual void setKeyId(int key){
            this.keyId = key;
        }
        #endregion

        #region SPECIAL GET-SET
        
        public virtual void setTarget(BaseGameEntity target){
        
        }
        
        public virtual void setDestination(Coordinate dest){

        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Commands;

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

            if (orders.Count > 0){
                //check validity of next command
                if (orders[0].finishedExecution)
                    orders.RemoveAt(0);

            }

            if(orders.Count > 0){
                orders[0].execute(ref myRef);
            }
        }

        /// <summary>
        /// Empties the entity's command list, and inserts 'order'
        /// so that it is its only order
        /// </summary>
        /// <param name="order">The new command</param>
        public void issueSingleOrder(BaseCommand order){
            this.orders = new List<BaseCommand>();
            this.orders.Add(order);
        }

        /// <summary>
        /// Replaces the entity's command list with another list
        /// of commands
        /// </summary>
        /// <param name="commands">The new commands</param>
        public void issueOrderList(params BaseCommand[] commands){
            this.orders = new List<BaseCommand>();
            if(commands != null)this.orders.AddRange(commands);
        }

        /// <summary>
        /// Adds a new command to the end of the entity's command list
        /// </summary>
        /// <param name="order">The new command</param>
        public void issueAdditionalOrder(BaseCommand order){
            this.orders.Add(order);
        }

        /// <summary>
        /// Adds a new command to the beginning of the entity's command list
        /// </summary>
        /// <param name="order">The new important command</param>
        public void issueImmediateOrder(BaseCommand order){
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
            this.location = coords;
        }

        public virtual int getKeyId(){
            return this.keyId;
        }

        public virtual void setKeyId(int key){
            this.keyId = key;
        }
        #endregion
    }
}
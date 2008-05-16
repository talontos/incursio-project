using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Commands;
using Incursio.Managers;
using Incursio.Entities.Components;
using Incursio.Classes;

namespace Incursio.Entities
{
    public class BaseEntity
    {
        //ENTITY STATS
        public int keyId = -1;
        public String type = "";    //unit type-name?
        public int maxHealth = 100;
        public int health = 100;
        public int armor = 0;
        public int sightRange = 0;
        public Coordinate location = new Coordinate(0, 0);
        public int pointValue = 0;
        protected List<BaseCommand> orders;
        public int playerId = -1;

        public List<BaseComponent> components;

        public BaseEntity()
        {
            orders = new List<BaseCommand>();
        }

        /// <summary>
        /// General update function for updating entities
        /// </summary>
        /// <param name="gameTime">Game time passed from main loop</param>
        /// <param name="myRef">Mostly a hack, used for executing commands.  It is a reference of 'this'</param>
        public virtual void Update(GameTime gameTime, ref BaseEntity myRef)
        {
            //TODO: draw unit here?
            //TODO: check if i'm clicked?

            if (this.isDead())
            {
                this.updateOccupancy(false);
                return;
            }

            this.processOrderList(gameTime, ref myRef);

            if (this.health > this.maxHealth)
                this.health = this.maxHealth;
        }

        #region Orders
        protected virtual void processOrderList(GameTime gameTime, ref BaseEntity myRef)
        {
            if (orders.Count > 0)
            {
                //check validity of next command
                if (orders[0].finishedExecution)
                    orders.RemoveAt(0);

            }

            //if I still have orders...
            if (orders.Count > 0)
            {
                //taken out for refactoring
                //orders[0].execute(gameTime, ref myRef);

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
            if (commands != null) this.orders.AddRange(commands);
        }

        /// <summary>
        /// Adds a new command to the end of the entity's command list
        /// </summary>
        /// <param name="order">The new command</param>
        public virtual void issueAdditionalOrder(BaseCommand order)
        {
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
        #endregion
        
        public virtual Texture2D getCurrentTexture()
        {
            //check state & return appropriate texture from TextureBank.EntityTextures
            return null;
        }

        public virtual void takeDamage(int damage, BaseEntity attacker)
        {
            //TODO: some math using my armor
            int damageTaken = (damage - armor) + (Incursio.rand.Next(0, 10) - 5);  //[-5,+5]
            if (damageTaken < 0)
                damageTaken = 0;

            this.health -= damageTaken;

            //taken out for refactoring
            //MessageManager.getInstance().addMessage(new GameEvent(State.EventType.TAKING_DAMAGE, this, "", Convert.ToString(damageTaken), this.location));

            if (this.health < 0)
            {
                this.health = 0;
            }
        }

        public virtual String getTextureName()
        {
            return null;
        }

        public virtual bool isDead()
        {
            return health <= 0;
        }

        public virtual void updateBounds()
        {

        }

        public virtual bool attackTarget()
        {
            return false;
        }

        public virtual void killedTarget()
        {

        }

        /// <summary>
        /// Virtual function for moving
        /// </summary>
        /// <returns>True when destination is reached.  By default returns true.</returns>
        public virtual bool updateMovement(float ElapsedTime)
        {
            return true;
        }

        /// <summary>
        /// Sets the location of this entity in the current map as occupied (true) or unocupied (false)
        /// </summary>
        /// <param name="occupied"></param>
        public virtual void updateOccupancy(bool occupied)
        {
            MapManager.getInstance().currentMap.setSingleCellOccupancy(location.x, location.y,
                (occupied ? (byte)0 : (byte)1));

            MapManager.getInstance().currentMap.setSingleCellEntity(location.x, location.y,
                (occupied ? this.keyId : -1));
        }

        public virtual void setIdle()
        {

        }

        public virtual void setAttacking()
        {

        }

        public virtual void drawThyself(ref SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
        {

        }

        public virtual bool isCapturing()
        {
            return false;
        }

        public virtual bool isAttacking()
        {
            return false;
        }

        public virtual bool isMoving()
        {
            return false;
        }

        public virtual int getAttackDamage()
        {
            return -1;
        }

        public virtual int getArmor()
        {
            return -1;
        }

        public virtual int getAttackRange()
        {
            return 0;
        }

        public virtual int getAttackSpeed()
        {
            return -1;
        }

        protected virtual void notifyUnderAttack()
        {
            //taken out for refactoring
            /*PlayerManager.getInstance().notifyPlayer(
                this.playerId,
                new GameEvent(State.EventType.UNDER_ATTACK, this, "", "We're Under Attack!", this.location)
            );*/
        }

        public virtual void heal(int boost)
        {
            if (health < maxHealth)
            {
                this.health += boost;
                //taken out for refactoring
                //MessageManager.getInstance().addMessage(new GameEvent(State.EventType.HEALING, this, "", Convert.ToString(boost), this.location));
            }

            if (health > maxHealth)
                health = maxHealth;
        }

        #region Getters/Setters
        public virtual void setType(State.EntityName name)
        {
            //taken out for refactoring
            //this.entityType = name;
        }

        public virtual State.EntityName getType()
        {
            //taken out for refactoring
            //return this.entityType;
            return State.EntityName.NONE;
        }

        public virtual long getHealth()
        {
            return this.health;
        }

        public virtual void setHealth(int health)
        {
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

        public virtual int getPlayer()
        {
            //taken out for refactoring
            //return this.playerId;
            return PlayerManager.getInstance().computerPlayerId;
        }

        public virtual void setPlayer(int playerId)
        {
            this.playerId = playerId;
        }

        public virtual long getSightRange()
        {
            return this.sightRange;
        }

        public virtual void setSightRange(int sightRange)
        {
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

        public virtual int getKeyId()
        {
            return this.keyId;
        }

        public virtual void setKeyId(int key)
        {
            this.keyId = key;
        }

        public virtual void setMap(MapBase map)
        {
            //taken out for refactoring
            //this.map = map;
        }

        public virtual void playSelectionSound()
        {
            //taken out for refactoring
            //if (owner == PlayerManager.getInstance().computerPlayerId)
                return;
        }

        public virtual void playOrderMoveSound()
        {
            //taken out for refactoring
            //if (owner == PlayerManager.getInstance().computerPlayerId)
                return;

        }

        public virtual void playOrderAttackSound()
        {
            //taken out for refactoring
            //if (owner == PlayerManager.getInstance().computerPlayerId)
                return;

        }

        public virtual void playDeathSound()
        {

        }

        public virtual void playEnterBattlefieldSound()
        {
            //taken out for refactoring
            //if (owner == PlayerManager.getInstance().computerPlayerId)
                return;

        }

        #endregion

        #region SPECIAL GET-SET

        public virtual void setTarget(BaseEntity target)
        {

        }

        public virtual void setDestination(Coordinate dest)
        {

        }

        #endregion
    }
}

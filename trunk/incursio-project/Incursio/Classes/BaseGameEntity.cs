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
        public int maxHealth = 100;
        public int health = 100;
        public int armor = 0;
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
        public bool smartGuarding = true;
        public int currentFrameX = 0;       //for animation
        public int currentFrameY = 0;       //for animation
        public int currentFrameXAttackDeath = 0;
        public int currentFrameYAttackDeath = 0;
        public int attackFramePause = 0;

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

            if (this.isDead()){
                this.updateOccupancy(false);
                return;
            }

            this.processOrderList(gameTime, ref myRef);

            if (this.health > this.maxHealth)
                this.health = this.maxHealth;
        }

        protected virtual void processOrderList(GameTime gameTime, ref BaseGameEntity myRef)
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
            int damageTaken = (damage - armor) + (Incursio.rand.Next(0, 10) - 5);  //[-5,+5]
            if (damageTaken < 0)
                damageTaken = 0;

            this.health -= damageTaken;

            MessageManager.getInstance().addMessage(new GameEvent(State.EventType.TAKING_DAMAGE, this, Convert.ToString(damageTaken), this.location));

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

            MapManager.getInstance().currentMap.setSingleCellEntity(location.x, location.y, 
                (occupied ? this.keyId : -1));
        }

        public virtual void setIdle(){

        }

        public virtual void setAttacking(){

        }

        public virtual void drawThyself(ref SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
        {

        }

        public virtual bool isCapturing(){
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

        public virtual int getAttackDamage(){
            return -1;
        }

        public virtual int getArmor(){
            return -1;
        }

        public virtual int getAttackRange(){
            return 0;
        }

        public virtual int getAttackSpeed(){
            return -1;
        }

        protected virtual void notifyUnderAttack()
        {
            PlayerManager.getInstance().notifyPlayer(
                this.owner,
                new GameEvent(State.EventType.UNDER_ATTACK, this,/*SOUND,*/ "We're Under Attack!", this.location)
            );
        }

        public virtual void heal(int boost){
            if(health < maxHealth){
                this.health += boost; 
                MessageManager.getInstance().addMessage(new GameEvent(State.EventType.HEALING, this, Convert.ToString(boost), this.location));
            }

            if (health > maxHealth)
                health = maxHealth;
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

        public virtual void setMap(MapBase map){
            this.map = map;
        }

        public virtual void playSelectionSound(){
            if (owner == State.PlayerId.COMPUTER)
                return;
        }

        public virtual void playOrderMoveSound()
        {
            if (owner == State.PlayerId.COMPUTER)
                return;

        }

        public virtual void playOrderAttackSound()
        {
            if (owner == State.PlayerId.COMPUTER)
                return;

        }

        public virtual void playDeathSound()
        {

        }

        public virtual void playEnterBattlefieldSound()
        {
            if (owner == State.PlayerId.COMPUTER)
                return;

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

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

using Incursio.Commands;
using Incursio.Managers;
using Incursio.Entities.Components;

namespace Incursio.Entities
{
    public class BaseGameEntity
    {
        public string entityName;
        public int maxHealth = 100;
        public int health = 100;
        public int armor = 0;
        public int sightRange = 0;
        public int owner;
        public Coordinate location = new Coordinate(0,0);
        public int keyId = -1;
        public int pointValue = 0;

        public State.EntityState currentState = State.EntityState.Idle;

        //Entity Size
        public Vector2 size = new Vector2(1, 1);

        //BOOL VALUES for COMPONENTS
        public bool canAttack = false;
        public bool canMove = false;
        public bool isConstructor = false;
        public bool isStructure = false;

        //BOOL VALUES for SPECIAL ENTITIES
        public bool isHero = false;
        public bool isMainBase = false;
        public bool isControlPoint = false;
        public bool isTurret = false;

        private List<BaseCommand> orders;

        //Death stuff
        private int TIME_DEAD_UNTIL_DESPAWN = 5;
        private int deadTimer = 0;
        private bool playedDeathSound = false;
        public bool hasDied = false;

        public RenderComponent renderComponent;
        public MovementComponent movementComponent;
        public FactoryComponent factoryComponent;
        public CapturableComponent capturableComponent;
        public CaptureComponent captureComponent;
        public CombatComponent combatComponent;
        public ExperienceComponent experienceComponent;
        public ResourceComponent resourceComponent;
        public AudioComponent audioComponent;
        public HealComponent healComponent;

        public BaseGameEntity(){
            orders = new List<BaseCommand>();
        }

        /// <summary>
        /// General update function for updating entities
        /// </summary>
        /// <param name="gameTime">Game time passed from main loop</param>
        /// <param name="myRef">Mostly a hack, used for executing commands.  It is a reference of 'this'</param>
        public virtual void Update(GameTime gameTime, ref BaseGameEntity myRef){
            if(hasDied){
                return;
            }
            else if (this.isDead()){
                //this.updateOccupancy(false);
                this.die();
                return;
            }

            if(!this.isStructure)
                this.updateOccupancy(false);

            #region Update components
            //TODO: IS THERE AN EASIER WAY TO UPDATE ALL COMPONENTS?
            if (renderComponent != null)
                renderComponent.Update(gameTime);
            
            if (movementComponent != null)
                movementComponent.Update(gameTime);
            
            if (factoryComponent != null)
                factoryComponent.Update(gameTime);

            if (capturableComponent != null)
                capturableComponent.Update(gameTime);

            if (captureComponent != null)
                captureComponent.Update(gameTime);
            
            if (combatComponent != null)
                combatComponent.Update(gameTime);
            
            if (experienceComponent != null)
                experienceComponent.Update(gameTime);

            if (resourceComponent != null)
                resourceComponent.Update(gameTime);

            if (audioComponent != null)
                audioComponent.Update(gameTime);

            if (healComponent != null)
                healComponent.Update(gameTime);
            #endregion
            
            this.processOrderList(gameTime, ref myRef);

            if (this.health > this.maxHealth)
                this.health = this.maxHealth;

            this.updateOccupancy(true);
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

                //check type for player notification?
            }
            else if(EntityManager.getInstance().ENTITIES_AUTO_GUARD){
                this.issueSingleOrder(new GuardCommand());
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

        public virtual void takeDamage(int damage, BaseGameEntity attacker){
            int damageTaken = (damage - armor) + (Incursio.rand.Next(0, 10) - 5);  //[-5,+5]
            if (damageTaken < 0)
                damageTaken = 0;

            this.health -= damageTaken;

            MessageManager.getInstance().addMessage(new GameEvent(State.EventType.TAKING_DAMAGE, this, "", Convert.ToString(damageTaken), this.location));

            if (this.health < 0)
            {
                this.health = 0;
            }
            else{
                //retaliate
                this.issueImmediateOrder(new AttackCommand(attacker));
            }
        }

        /// <summary>
        /// Returns if this entity is dead
        /// </summary>
        /// <returns>Am I dead?</returns>
        public virtual bool isDead(){
            return health <= 0;
        }

        public void die()
        {
            currentState = State.EntityState.Dead;

            if (orders.Count != 0)
                orders = new List<BaseCommand>();

            if (!playedDeathSound)
            {
                this.playDeathSound();
                playedDeathSound = true;
            }

            //TODO: How to handle death of entities?
            //  It'd be a waste of memory to just leave them in the entityBank,
            //    but then we'd have to reassign keyIds.
            //  But do we need to remove them?
            // HASHMAP!!!!
            if (deadTimer == TIME_DEAD_UNTIL_DESPAWN * 60 || this.movementComponent == null)
            {
                //map.setSingleCellOccupancy(location.x, location.y, 1);
                this.updateOccupancy(false);
                EntityManager.getInstance().removeEntity(keyId);
                //deadTimer++;
                this.hasDied = true;
            }
            else
            {
                deadTimer++;
            }
        }

        /// <summary>
        /// Attack this entity's current target
        /// </summary>
        /// <returns>True if target is in range</returns>
        public virtual bool attackTarget(){
            //return false;

            if(this.combatComponent != null){
                return this.combatComponent.attackTarget();
            }
            else{
                return false;
            }
        }

        /// <summary>
        /// Gain experience and process statistical information when This kills a target
        /// </summary>
        public virtual void killedTarget(ref BaseGameEntity deadGuy){
            if(this.experienceComponent != null){
                this.experienceComponent.gainExperience(deadGuy.pointValue);
            }
        }

        /// <summary>
        /// Virtual function for moving
        /// </summary>
        /// <returns>True when destination is reached.  By default returns true.</returns>
        public virtual bool updateMovement(float ElapsedTime){
            if(this.movementComponent != null){
                return this.movementComponent.updateMovement(ElapsedTime);
            }
            else{
                return true;
            }
        }

        /// <summary>
        /// Sets the location of this entity in the current map as occupied (true) or unocupied (false)
        /// </summary>
        /// <param name="occupied"></param>
        public void updateOccupancy(bool open){
            int x1, y1, x2, y2, y;

            //translate location
            MapManager.getInstance().currentMap.translatePixelToMapCell(
                this.renderComponent.boundingBox.X,
                this.renderComponent.boundingBox.Y,
                out x1, out y1);

            x2 = x1 + (int)this.size.X;
            y2 = y1 + (int)this.size.Y;

            while (x1 <= x2)
            {
                y = y1;
                while (y <= y2)
                {
                    MapManager.getInstance().currentMap.setSingleCellOccupancy_cell((x1), (y), (byte)(open ? 0 : 1));
                    MapManager.getInstance().currentMap.setSingleCellEntity_cell(x1, y, (open ? this.keyId : -1));
                    y++;
                }
                x1++;
            }
        }

        /// <summary>
        /// Sets the location of this entity in the current map as occupied (true) or unocupied (false)
        /// </summary>
        /// <param name="occupied"></param>
        public void updateOccupancy_OLD(bool open)
        {
            //TODO: THIS DOESN'T WORK OUT AS WELL WITH LARGE ENTITES
            //  ***We could try increasing the resolution of the occupancy/id grids.
            //  ***We could try defining size classes instead of calculating size based on Texture!
            //      - Either:
            //          - set an entity width and height
            //          - define a size class and map to entities

            //  we need some buffer room...if one pixel is overflowing to another cell that whole cell becomes occupied.
            
            //occupancy should extend/retract to the nearest cell

            //NOTE: It seems that the upper-left corner of an entity is NOT its origin...
            //  it's looking to be more like the center of the entity is its origin.

            //Use the bounding-box to easily set the occupancy
            int x1, y1, x2, y2, y;
            
            //translate location
            MapManager.getInstance().currentMap.translatePixelToMapCell(
                this.renderComponent.boundingBox.X, 
                this.renderComponent.boundingBox.Y, 
                out x1, out y1);

            //translate width/height
            MapManager.getInstance().currentMap.translatePixelToMapCell(
                this.renderComponent.boundingBox.X + this.renderComponent.boundingBox.Width, 
                this.renderComponent.boundingBox.Y + this.renderComponent.boundingBox.Height, 
                out x2, out y2);
            
            while (x1 <= x2)
            {
                y = y1;
                while (y <= y2)
                {
                    MapManager.getInstance().currentMap.setSingleCellOccupancy_cell((x1), (y), (byte)(open ? 0 : 1));
                    MapManager.getInstance().currentMap.setSingleCellEntity_cell(x1, y, (open ? this.keyId : -1));
                    y++;
                }
                x1++;
            }
        }

        public virtual void setIdle(){
            this.currentState = State.EntityState.Idle;
        }

        public virtual void setAttacking(){
            this.currentState = State.EntityState.Attacking;
        }

        /// <summary>
        /// Draws this entity on the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="frameTimer"></param>
        /// <param name="FRAME_LENGTH"></param>
        public void drawThyself(ref SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
        {
            if(this.renderComponent != null && !this.hasDied)
                this.renderComponent.drawThyself(ref spriteBatch, frameTimer, FRAME_LENGTH);
        }

        public virtual bool isCapturing(){
            return currentState == State.EntityState.Capturing;
        }

        public virtual bool isAttacking()
        {
            return currentState == State.EntityState.Attacking;
        }

        public virtual bool isMoving()
        {
            return currentState == State.EntityState.Moving;
        }

        public virtual int getAttackDamage(){
            if(combatComponent != null){
                return combatComponent.damage;
            }

            return 0;
        }

        public virtual int getArmor(){
            return this.armor;
        }

        public virtual int getAttackRange(){
            if (combatComponent != null)
                return combatComponent.attackRange;

            return 0;
        }

        public virtual int getAttackSpeed(){
            if(combatComponent != null)
                return combatComponent.attackSpeed;

            return 0;
        }

        protected virtual void notifyUnderAttack()
        {
            PlayerManager.getInstance().notifyPlayer(
                this.owner,
                new GameEvent(State.EventType.UNDER_ATTACK, this,"", "We're Under Attack!", this.location)
            );
        }

        /// <summary>
        /// Heal me for boost hitpoints, unless I'm dead
        /// </summary>
        /// <param name="boost"></param>
        public virtual void heal(int boost){
            if (this.isDead())
                return;

            if(health < maxHealth){
                this.health += boost; 
                MessageManager.getInstance().addMessage(new GameEvent(State.EventType.HEALING, this, "", Convert.ToString(boost), this.location));
            }

            if (health > maxHealth)
                health = maxHealth;
        }

        public virtual string getClassName(){
            return "";
        }

        #region Getters/Setters

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

        public virtual int getPlayer(){
            return this.owner;
        }

        public virtual void setPlayer(int owner){
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

            if (this.factoryComponent != null)
                this.factoryComponent.setSpawnAndDestination();
        }

        public virtual int getKeyId(){
            return this.keyId;
        }

        public virtual void setKeyId(int key){
            this.keyId = key;
        }

        public virtual void playSelectionSound(){
            if(audioComponent != null)
                this.audioComponent.playSelectionSound();
        }

        public virtual void playOrderMoveSound()
        {
            if (audioComponent != null)
                this.audioComponent.playOrderMoveSound();
        }

        public virtual void playOrderAttackSound()
        {
            if (audioComponent != null)
                this.audioComponent.playOrderAttackSound();
        }

        public virtual void playDeathSound()
        {
            if (audioComponent != null)
                this.audioComponent.playDeathSound();
        }

        public virtual void playEnterBattlefieldSound()
        {
            if (audioComponent != null)
                this.audioComponent.playEnterBattlefieldSound();
        }

        public virtual void playAttackSound(){
            if (audioComponent != null)
                this.audioComponent.playAttackSound();
        }

        #endregion

        #region SPECIAL GET-SET
        
        public virtual void setTarget(BaseGameEntity target){
            if(this.combatComponent != null)
                combatComponent.target = target;
        }
        
        public virtual void setDestination(Coordinate dest){
            if(this.movementComponent != null){
                this.movementComponent.destination = dest;
            }
        }

        #endregion
    }
}

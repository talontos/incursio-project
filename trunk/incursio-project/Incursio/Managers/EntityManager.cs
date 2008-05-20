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

using Microsoft.Xna.Framework;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Interface;
using Incursio.Utils;
using Incursio.Commands;
using Microsoft.Xna.Framework.Graphics;
using Incursio.Entities;

namespace Incursio.Managers
{
    public class EntityManager
    {
        private static EntityManager instance;

        private List<BaseGameEntity> entityBank;
        private List<BaseGameEntity> selectedUnits;

        public List<List<BaseGameEntity>> groups;

        private int nextKeyId;

        private EntityManager(){
            entityBank = new List<BaseGameEntity>();
            selectedUnits = new List<BaseGameEntity>();
            nextKeyId = 0;
            Hero.reinitNames();

            groups = new List<List<BaseGameEntity>>(10);
            for (int i = 0; i < groups.Capacity; i++)
                groups.Add( new List<BaseGameEntity>());
        }

        public static EntityManager getInstance()
        {
            if (instance == null)
                instance = new EntityManager();
            return instance;
        }

        public void reinitializeInstance()
        {
            instance = new EntityManager();
        }

        public List<BaseGameEntity> getSelectedUnits(){
            return selectedUnits;
        }

        public List<BaseGameEntity> getAllEntities(){
            return entityBank;
        }

        public void addEntity(BaseGameEntity e){

        }

        ////////////Functional Methods////////////

        public void updateAllEntities(GameTime gameTime)
        {
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                e.Update(gameTime, ref e);
            });
        }

        public List<DefenseReport> updatePlayerKeyPoints(int player){
            List<DefenseReport> reports = new List<DefenseReport>();
            
            MapManager.getInstance().keyPoints.ForEach(delegate(KeyPoint point)
            {
                if(point.owner == player){
                    reports.Add(point.Update());
                }
            });

            return reports;
        }

        public List<KeyPoint> getPlayerKeyPoints(int player){
            List<KeyPoint> points = new List<KeyPoint>();
            MapManager.getInstance().keyPoints.ForEach(delegate(KeyPoint point)
            {
                if (point.owner == player)
                {
                    //update to make sure stats are correct
                    point.Update();
                    points.Add(point);
                }
            });

            return points;
        }

        public void addToSelectedUnits(BaseGameEntity e){
            if (selectedUnits.Count < 12){
                selectedUnits.Add(e);
                e.playSelectionSound();
            }
        }

        public void addToSelectedUnits(List<BaseGameEntity> es){
            for(int i = 0; i < 12 && i < es.Count; i++){
                if (selectedUnits.Count < 12)
                {
                    selectedUnits.Add(es[i]);
                    if (i == 0)
                    {
                        es[i].playSelectionSound();
                    }
                }
                else break;
            }
        }

        public void updateUnitSelection(Rectangle area)
        {

            //IF NO UNITS ARE IN THE SELECTED AREA,
            //  WE SHOULD PROBABLY KEEP OUR CURRENT SELECTION
            List<BaseGameEntity> unitsInArea = new List<BaseGameEntity>();
            //bool selectingStructures = true;

            //TODO: select a single structure if no units are in range
            entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if( area.Contains(e.getLocation().toPoint()) ){
                    if(e.getPlayer() == PlayerManager.getInstance().currentPlayerId && !e.isDead()){
                        if( !(e.movementComponent == null) )
                            unitsInArea.Add(e);
                    }
                }
            });

            if(unitsInArea.Count != 0){
                if (!InputManager.getInstance().shifting()){
                    this.selectedUnits = new List<BaseGameEntity>();
                }

                this.addToSelectedUnits(unitsInArea);
            }
        }

        public void updateUnitSelection(Vector2 point){
            //CLICKING ENTITY?
            bool done = false;

            //find who i'm clicking
            entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.isDead())
                {
                    //do nothing
                }
                else
                {
                    if (e.visible)
                    { //only check visible ones so we don't waste time

                        //get selection rectangle for current unit
                        Rectangle unit = e.boundingBox;
                        if (unit.Contains(new Point(Convert.ToInt32(point.X), Convert.ToInt32(point.Y))))
                        {

                            if (e.getPlayer() == PlayerManager.getInstance().computerPlayerId){
                                selectedUnits = new List<BaseGameEntity>();
                                this.addToSelectedUnits(e);
                            }
                            else
                            {
                                if (selectedUnits.Count == 1)
                                {
                                    if (selectedUnits[0].getPlayer() == PlayerManager.getInstance().computerPlayerId)
                                    {
                                        selectedUnits = new List<BaseGameEntity>();
                                    }
                                }
                                bool newUnitIsSelected = selectedUnits.Contains(e as Unit);
                                if (InputManager.getInstance().shifting())
                                {
                                    //just add/remove new guy
                                    if (newUnitIsSelected)
                                    {
                                        selectedUnits.Remove(e);
                                    }
                                    else if (!e.isDead())
                                    {
                                        this.addToSelectedUnits(e);
                                    }
                                }
                                else
                                {
                                    //shift not pressed

                                    selectedUnits = new List<BaseGameEntity>();
                                    addToSelectedUnits(e);
                                }
                            }
                            done = true;
                        }
                    } 
                }
            });
            if(!done){   //not clicking a unit
                selectedUnits = new List<BaseGameEntity>();
            }
        }

        public void updateUnitOrders(Vector2 point){
            bool done = false;

            //Don't bother running if there is no selection,
            //And don't let user command computer's units
            if (selectedUnits.Count == 0 || selectedUnits[0].owner == PlayerManager.getInstance().computerPlayerId)
                return;

            //clicking entity
            //e represents the entity that is possibly clicked on
            entityBank.ForEach(delegate(BaseGameEntity e)
            {

                if (e.visible && !e.isDead() )
                { //only check visible ones so we don't waste time

                    //get selection rectangle for e
                    Rectangle unit = e.boundingBox;
                    if (unit.Contains(new Point(Convert.ToInt32(point.X), Convert.ToInt32(point.Y))))
                    {
                        //if it's capturable and doesn't belong to the current player, initialize a cap
                        if(e.getPlayer() != selectedUnits[0].getPlayer() && e.capturableComponent != null)
                        {
                            //if there is a unit that can capture among the selected units, start the cap
                            selectedUnits.ForEach(delegate (BaseGameEntity u){
                                if (u.captureComponent != null)
                                {
                                    EntityManager.getInstance().issueCommand_SingleEntity(State.Command.CAPTURE, false, u, e);
                                }
                                else{
                                    EntityManager.getInstance().issueCommand(State.Command.MOVE, false, null, e.getLocation());
                                }
                            });
                        }
                        //NOW, if unit is enemy, selected units attack!
                        else if (e.getPlayer() != selectedUnits[0].getPlayer())
                        {
                            EntityManager.getInstance().issueCommand(State.Command.ATTACK, false, null, e);
                        }
                        else
                        {
                            EntityManager.getInstance().issueCommand(State.Command.FOLLOW, false, null, e);
                        }
                        done = true;
                    }
                }
            });

            //NOT ENTITY, SO MOVE SELECTED UNITS

            if (!done && selectedUnits.Count > 0)
            {
                if (InputManager.getInstance().alting())
                    EntityManager.getInstance().issueCommand(State.Command.ATTACK_MOVE, false, null, new Coordinate(Convert.ToInt32(point.X), Convert.ToInt32(point.Y)));
                else
                    EntityManager.getInstance().issueCommand(State.Command.MOVE, false, null, new Coordinate(Convert.ToInt32(point.X), Convert.ToInt32(point.Y)));
            }

            done = true;
        
        }

        public BaseGameEntity getEntity(int keyId){
            if(keyId <= this.entityBank.Count)
                return this.entityBank[keyId];

            return null;
        }

        public List<BaseGameEntity> getPlayerEntities(int player){
            List<BaseGameEntity> playerEntities = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e){
                if (e.getPlayer() == player)
                    playerEntities.Add(e);
            });

            return playerEntities;
        }

        public List<BaseGameEntity> getLivePlayerEntities(int player){
            List<BaseGameEntity> playerEntities = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e){
                if (e.getPlayer() == player && !e.isDead())
                    playerEntities.Add(e);
            });

            return playerEntities;
        }

        public List<BaseGameEntity> getLivePlayerUnits(int player){
            List<BaseGameEntity> playerUnits = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getPlayer() == player && !e.isDead() && e is Unit)
                    playerUnits.Add(e);
            });
            return playerUnits;
        }

        public List<BaseGameEntity> getLivePlayerHeros(int player){
            List<BaseGameEntity> playerHeros = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getPlayer() == player && !e.isDead() && e.experienceComponent != null)
                    playerHeros.Add(e);
            });

            return playerHeros;
        }

        public List<BaseGameEntity> getLivePlayerStructures(int player)
        {
            List<BaseGameEntity> playerStructs = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getPlayer() == player && !e.isDead() && e.movementComponent == null)//structures are denoted by non-movable entities
                    playerStructs.Add(e);
            });

            return playerStructs;
        }

        public List<BaseGameEntity> getLivePlayerCamps(int player)
        {
            List<BaseGameEntity> playerStructs = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getPlayer() == player && !e.isDead() && e.factoryComponent != null)
                    playerStructs.Add(e);
            });

            return playerStructs;
        }

        public List<BaseGameEntity> getPlayerControlPoints(int player)
        {
            List<BaseGameEntity> playerStructs = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getPlayer() == player && !e.isDead() && e.capturableComponent != null)
                    playerStructs.Add(e);
            });

            return playerStructs;
        }

        public int getPlayerTotalOwnedControlPoints(int id)
        {
            int numPoints = 0;

            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.capturableComponent != null && e.owner == id)
                {
                    numPoints++;
                }
            });

            return numPoints;
        }

        public int getTotalControlPoints()
        {
            int numPoints = 0;

            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.capturableComponent != null)
                {
                    numPoints++;
                }
            });

            return numPoints;
        }

        public bool isPlayerCampDestroyed(int id)
        {
            return false;
            //TODO: REVISIT THIS
            /*
            bool result = true;
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getType() == State.EntityName.Camp && e.owner == id)
                {
                    if ((e as CampStructure).getCurrentState() == State.EntityState.Destroyed)
                    {

                    }
                    else
                    {
                        result = false;
                    }
                   
                }
            });

            return result;
            */
        }
        
        public void tryToBuild(int toBuildId){
            if (selectedUnits.Count > 0 && selectedUnits[0].isConstructor && selectedUnits[0].owner == PlayerManager.getInstance().currentPlayerId)
            {
                //TODO: IMPL
                this.issueCommand(State.Command.BUILD, true, null, toBuildId);
            }
        }

        public void tryToBuild(State.EntityName toBuild){
            if (selectedUnits.Count > 0 && selectedUnits[0].isConstructor && selectedUnits[0].owner == PlayerManager.getInstance().currentPlayerId)
            {
                this.issueCommand(State.Command.BUILD, true, null, toBuild);
            }
        }

        public void tryToBuild(State.EntityName toBuild, Vector2 point){
            if (selectedUnits.Count > 0 && selectedUnits[0].isConstructor)
            {
                this.issueCommand(State.Command.BUILD, true, null, toBuild, new Coordinate((int)point.X, (int)point.Y));
            }
        }

        public BaseGameEntity createNewEntity(int entityClassId, int player){
            BaseGameEntity product = ObjectFactory.getInstance().create(entityClassId, player);
            product.keyId = nextKeyId;
            this.entityBank.Insert(nextKeyId++, product);

            return product;
        }

        public BaseGameEntity createNewEntity(String entityType, int player){
            BaseGameEntity product = ObjectFactory.getInstance().create(entityType, player);
            product.keyId = nextKeyId;
            this.entityBank.Insert(nextKeyId++, product);

            return product;
        }

        /// <summary>
        /// Issues a command to units in entitiesToCommand.
        /// If entitiesToCommand is null, selectedUnis will be used in its stead
        /// </summary>
        /// <param name="commandType">Enumerated command type identifying the command</param>
        /// <param name="append">Boolean value denoting if the command should override all current commands, or
        ///     if it should be appended to any current command lists
        /// </param>
        /// <param name="entitiesToCommand">The entities to be issued the command.  If 'null' is provided,
        ///     selectedUnits will be used instead.
        /// </param>
        /// <param name="args">A list of arguments dependent upon the command type</param>
        public void issueCommand(State.Command commandType, bool append, List<BaseGameEntity> entitiesToCommand, params Object[] args){

            BaseCommand command = null;
            
            if (entitiesToCommand == null)
                entitiesToCommand = selectedUnits;

            int i = 0;
            entitiesToCommand.ForEach(delegate(BaseGameEntity e)
            {
                if(!e.isDead()){
                    switch (commandType)
                    {
                        ////////////////////////
                        case State.Command.MOVE:
                            command = new MoveCommand(args[0] as Coordinate);
                            break;

                        ////////////////////////
                        case State.Command.ATTACK_MOVE:
                            //TODO: PATHING!!
                            command = new AttackMoveCommand(args[0] as Coordinate);
                            break;

                        ////////////////////////
                        case State.Command.ATTACK:
                            if (args[0] is ControlPoint)
                            {

                            }
                            else{
                                command = new AttackCommand(args[0] as BaseGameEntity);
                                e.setAttacking();
                            }
                             
                            break;

                        ////////////////////////
                        case State.Command.STOP:
                            command = new StopCommand();
                            break;

                        ////////////////////////
                        case State.Command.FOLLOW:
                            if (args[0] is Unit)
                                command = new FollowCommand(args[0] as Unit);
                            else
                                command = new MoveCommand((args[0] as BaseGameEntity).location);

                            break;

                        ////////////////////////
                        case State.Command.GUARD:
                            command = new GuardCommand();
                            break;

                        ////////////////////////
                        case State.Command.BUILD:
                            //TODO: TEMPORARY!!!!  we need to get rid of EntityName
                            if(args[0] is State.EntityName)
                                command = new BuildCommand( (State.EntityName)args[0], (args.Length > 1 ? args[1] : null) as Coordinate);
                            else
                                command = new BuildCommand((int)args[0], (args.Length > 1 ? args[1] : null) as Coordinate);
                            break;

                        ////////////////////////
                        case State.Command.CAPTURE:
                            //try in case args[0] is not a controlpoint
                            try{
                                command = new CaptureCommand(args[0] as ControlPoint);
                            }
                            finally{}
                            break;

                        ////////////////////////
                    }

                    //add command if not null
                    if (command != null){
                        if (append){
                            e.issueAdditionalOrder(command);
                        }
                        else{
                            e.issueSingleOrder(command);
                        }

                        if (i == 0)
                        {
                            if (command is MoveCommand)
                                e.playOrderMoveSound();
                            else if (command is AttackCommand || command is AttackMoveCommand)
                                e.playOrderAttackSound();
                        }
                    }
                }

                i++;
            });
        }

        public void issueCommand_SingleEntity(State.Command commandType, bool append, BaseGameEntity entityToCommand, params Object[] args){
            List<BaseGameEntity> list = new List<BaseGameEntity>(1);
            list.Add(entityToCommand);

            this.issueCommand(commandType, append, list, args);
        }

        public void getAllEntitiesInRange(int owner, Coordinate location, int cellSightRange, 
                out List<BaseGameEntity> friendlies, out List<BaseGameEntity> enemies){
            friendlies = new List<BaseGameEntity>();
            enemies = new List<BaseGameEntity>();

            List<int> ids = MapManager.getInstance().currentMap.getEntitiesInRange(location, cellSightRange);
            BaseGameEntity e;

            for(int i = 0; i < ids.Count; i++){
                e = this.getEntity(ids[i]);
                if (!e.isDead())
                {
                    if (e.owner != owner)
                        enemies.Add(e);
                    else
                        friendlies.Add(e);
                }
            }
        }

        public void healEntitiesInRange(BaseGameEntity entity, int healRange, int healthBoost, bool healNonHeros){

            List<int> ids = MapManager.getInstance().currentMap.getEntitiesInRange(entity.location, healRange);
            int hOwner = entity.owner;
            BaseGameEntity e;

            ids.ForEach(delegate(int key)
            {
                e = this.getEntity(key);
                if (!e.isDead() && e.owner == hOwner && ( e is Hero || (healNonHeros && e is Unit) ) ){
                    //HEAL ME!
                    e.heal(healthBoost);
                }
            });
        }

        public List<BaseGameEntity> getEntitiesInRange(BaseGameEntity hunter, int cellSightRange){
            return this.getEntitiesInRange(ref hunter, cellSightRange);
        }

        public List<BaseGameEntity> getEntitiesInRange(ref BaseGameEntity hunter, int cellSightRange)
        {
            List<int> ids = MapManager.getInstance().currentMap.getEntitiesInRange(hunter.location, cellSightRange);
            List<BaseGameEntity> enemies = new List<BaseGameEntity>();
            int hOwner = hunter.owner;
            BaseGameEntity e;

            ids.ForEach(delegate(int key)
            {
                e = this.getEntity(key);
                if (!e.isDead() && e.owner != hOwner)
                    enemies.Add(e);
            });

            return enemies;
        }

        public void battleEntities(BaseGameEntity attacker, BaseGameEntity attackee){
            
        }

        public void removeEntity(int keyId)
        {
            if (keyId >= 0 && keyId <= entityBank.Count)
            {
                if (this.entityBank[keyId] is Unit)
                {
                    (this.entityBank[keyId] as Unit).setCurrentState(State.EntityState.Buried);
                }
                else if (this.entityBank[keyId] is Structure)
                {
                    (this.entityBank[keyId] as Structure).setCurrentState(State.EntityState.Destroyed);
                }

            }
        }

        /// <summary>
        /// Assigns the selected entites to a group
        /// </summary>
        /// <param name="groupNum">Group Number [0,9]</param>
        public void assignGroup(int groupNum){
            groups[groupNum] = selectedUnits;
        }

        /// <summary>
        /// Selects the specified group number, [0,9]
        /// </summary>
        /// <param name="groupNum"></param>
        public void selectGroup(int groupNum){

            if(!InputManager.getInstance().shifting()){
                selectedUnits = new List<BaseGameEntity>();
            }

            //we need to ensure that dead units are removed
            // from groups
            for(int i = 0; i < groups[groupNum].Count; i++){
                if (groups[groupNum][i].isDead())
                    groups[groupNum].RemoveAt(i);
            }

            addToSelectedUnits(groups[groupNum]);
        }

        public void selectPlayerHero(){
            this.selectedUnits = new List<BaseGameEntity>();
            this.addToSelectedUnits(this.getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId)[0] as BaseGameEntity);
        }

        public void selectPlayerCamp(){
            this.selectedUnits = new List<BaseGameEntity>();
            this.addToSelectedUnits(this.getLivePlayerCamps(PlayerManager.getInstance().currentPlayerId)[0] as BaseGameEntity);
        }

        public Color getColorMask(int player){
            return (player == PlayerManager.getInstance().currentPlayerId ? Color.White : Color.OrangeRed);
        }

        public State.ThreatLevel analyzeThreat(ref int myDamage, ref int myArmor, 
            ref int myHealth, ref int myRange, ref int myAttackSpeed, ref BaseGameEntity e){

            int eDamage, eArmor, eHealth, eRange, eAttackSpeed, myThreat, eThreat;

            eDamage = e.getAttackDamage();
            eArmor = e.getArmor();
            eHealth = (int)e.health;
            eRange = e.getAttackRange();
            eAttackSpeed = e.getAttackSpeed();

            //eThreat is much lower if they are already in battle
            eThreat = (((eHealth + eRange) / myDamage) / (e.isAttacking() ? 2 : 1)) / eAttackSpeed;
            myThreat = ((myHealth + myRange) / eDamage) / myAttackSpeed;

            if (e.isCapturing())
            {
                //if e is a hero and is capturing, we must stop him!
                return State.ThreatLevel.Medium;
            }
            else if (eThreat < 0)
            {
                //poses no threat, don't worry about it
                return State.ThreatLevel.None;
            }
            else if (eThreat > myThreat)
            {
                //he'll probably kill me, stay away
                return State.ThreatLevel.High;
            }
            else if (eThreat == myThreat)
            {
                return State.ThreatLevel.Medium;
            }
            else if (eThreat <= myThreat)
            {
                //at most it is formidable
                return State.ThreatLevel.Low;
            }

            else return State.ThreatLevel.None;
        }

        public void cancelCurrentBuildOrder(int id){
            if(selectedUnits.Count > 0 && selectedUnits[0] is CampStructure && selectedUnits[0].owner == id){
                (selectedUnits[0] as CampStructure).cancelCurrentOrder();
            }
        }

        public void killSelectedUnits(){
            selectedUnits.ForEach(delegate(BaseGameEntity e)
            {
                if( !(e is ControlPoint)){
                    e.health = 0;
                    if (e is Structure)
                        (e as Structure).setCurrentState(State.EntityState.Destroyed);
                    else if (e is Unit)
                        (e as Unit).setCurrentState(State.EntityState.Dead);
                }
            });
        }
    }
}

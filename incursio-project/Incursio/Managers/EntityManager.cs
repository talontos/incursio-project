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

namespace Incursio.Managers
{
    public class EntityManager
    {
        private static EntityManager instance;

        private List<BaseGameEntity> entityBank;
        private List<BaseGameEntity> selectedUnits;

        public List<List<BaseGameEntity>> groups;

        private ObjectFactory factory;

        private int nextKeyId;

        private EntityManager(){
            entityBank = new List<BaseGameEntity>();
            selectedUnits = new List<BaseGameEntity>();
            factory = ObjectFactory.getInstance();
            nextKeyId = 0;

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

        public void updateUnitSelection(Rectangle area){

            //IF NO UNITS ARE IN THE SELECTED AREA,
            //  WE SHOULD PROBABLY KEEP OUR CURRENT SELECTION
            List<BaseGameEntity> unitsInArea = new List<BaseGameEntity>();
            //bool selectingStructures = true;

            entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if( area.Contains(e.getLocation().toPoint()) ){
                    if(e.getPlayer() == State.PlayerId.HUMAN && !e.isDead()){
                        //if( !(e is Structure) )
                        //    selectingStructures = false;

                        //if( selectingStructures || !(e is Structure) )
                        if( !(e is Structure) )
                            unitsInArea.Add(e);
                    }
                }
            });

            if(unitsInArea.Count != 0){
                if (InputManager.getInstance().shifting())   //append
                    this.selectedUnits.AddRange(unitsInArea);
                else
                    this.selectedUnits = unitsInArea;
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

                            if (e.getPlayer() == State.PlayerId.COMPUTER){
                                selectedUnits = new List<BaseGameEntity>();
                                selectedUnits.Add(e);                             //numUnitsSelected = 1;
                            }
                            else
                            {
                                if (selectedUnits.Count == 1)//(numUnitsSelected == 1)
                                {
                                    if (selectedUnits[0].getPlayer() == State.PlayerId.COMPUTER)
                                    {
                                        selectedUnits = new List<BaseGameEntity>();
                                        //numUnitsSelected = 0;
                                    }
                                }
                                bool newUnitIsSelected = selectedUnits.Contains(e as Unit);
                                if (InputManager.getInstance().shifting())
                                {
                                    //just add/remove new guy
                                    if (newUnitIsSelected)
                                    {
                                        selectedUnits.Remove(e);
                                        //numUnitsSelected--;
                                    }
                                    else if (!e.isDead())
                                    {
                                        selectedUnits.Add(e);
                                        //numUnitsSelected++;
                                    }
                                }
                                else
                                {
                                    //shift not pressed

                                    selectedUnits = new List<BaseGameEntity>();
                                    selectedUnits.Add(e);
                                    //numUnitsSelected = 1;
                                }
                            }
                            done = true;
                        }
                    } 
                }
            });
            if(!done){   //not clicking a unit
                selectedUnits = new List<BaseGameEntity>();
                //numUnitsSelected = 0;
            }
        }

        public void updateUnitOrders(Vector2 point){
            bool done = false;

            //Don't bother running if there is no selection,
            //And don't let user command computer's units
            if (selectedUnits.Count == 0 || selectedUnits[0].owner == State.PlayerId.COMPUTER)
                return;

            //clicking entity
            entityBank.ForEach(delegate(BaseGameEntity e)
            {

                if (e.visible && !e.isDead() ) //(((e is Unit) && (e as Unit).getCurrentState() != State.UnitState.Dead && (e as Unit).getCurrentState() != State.UnitState.Buried) || ((e is Structure) && (e as Structure).getCurrentState() != State.StructureState.Destroyed)))
                { //only check visible ones so we don't waste time

                    //get selection rectangle for e
                    Rectangle unit = e.boundingBox;
                    if (unit.Contains(new Point(Convert.ToInt32(point.X), Convert.ToInt32(point.Y))))
                    {
                        //if it's a control point, initialize a cap
                        if(e.getPlayer() == State.PlayerId.COMPUTER && e.getType() == State.EntityName.ControlPoint)
                        {
                            //if there is a HERO among the selected units, start the cap
                            selectedUnits.ForEach(delegate (BaseGameEntity u){
                                if (u is HeavyInfantryUnit)
                                {
                                    EntityManager.getInstance().issueCommand_SingleEntity(State.Command.CAPTURE, false, u, e);
                                }
                                else{
                                    EntityManager.getInstance().issueCommand(State.Command.MOVE, false, null, e.getLocation());
                                }
                            });
                        }
                        //NOW, if unit is enemy, selected units attack!
                        else if (e.getPlayer() == State.PlayerId.COMPUTER)
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

        public List<BaseGameEntity> getPlayerEntities(State.PlayerId player){
            List<BaseGameEntity> playerEntities = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e){
                if (e.getPlayer() == player)
                    playerEntities.Add(e);
            });

            return playerEntities;
        }

        public List<BaseGameEntity> getLivePlayerEntities(State.PlayerId player){
            List<BaseGameEntity> playerEntities = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e){
                if (e.getPlayer() == player && !e.isDead())
                    playerEntities.Add(e);
            });

            return playerEntities;
        }

        public List<BaseGameEntity> getLivePlayerUnits(State.PlayerId player){
            List<BaseGameEntity> playerUnits = new List<BaseGameEntity>();
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getPlayer() == player && !e.isDead() && e is Unit)
                    playerUnits.Add(e);
            });
            return playerUnits;
        }

        public List<Hero> getLivePlayerHeros(State.PlayerId player){
            List<Hero> playerHeros = new List<Hero>();
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getPlayer() == player && !e.isDead() && e.getType() == State.EntityName.Hero)
                    playerHeros.Add(e as Hero);
            });

            return playerHeros;
        }

        public List<Structure> getLivePlayerStructures(State.PlayerId player)
        {
            List<Structure> playerStructs = new List<Structure>();
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getPlayer() == player && !e.isDead() && e is Structure)
                    playerStructs.Add(e as Structure);
            });

            return playerStructs;
        }

        public int getPlayerControlPoints(State.PlayerId id)
        {
            int numPoints = 0;

            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getType() == State.EntityName.ControlPoint && e.owner == id)
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
                if (e.getType() == State.EntityName.ControlPoint)
                {
                    numPoints++;
                }
            });

            return numPoints;
        }

        public bool isPlayerCampDestroyed(State.PlayerId id)
        {
            bool result = true;
            this.entityBank.ForEach(delegate(BaseGameEntity e)
            {
                if (e.getType() == State.EntityName.Camp && e.owner == id)
                {
                    if ((e as CampStructure).getCurrentState() == State.StructureState.Destroyed)
                    {

                    }
                    else
                    {
                        result = false;
                    }
                   
                }
            });

            return result;
        }

        public void tryToBuild(BaseGameEntity toBuild /*String entityType*/){
            if(selectedUnits.Count > 0 && selectedUnits[0] is CampStructure){
                this.issueCommand(State.Command.BUILD, true, null, toBuild);
                //this.createNewEntity(entityType, (selectedUnits[0] as CampStructure).owner);
            }
        }

        public void tryToBuild(BaseGameEntity toBuild, Vector2 point)
        {
            if (selectedUnits.Count > 0 && selectedUnits[0] is CampStructure)
            {
                (selectedUnits[0] as CampStructure).setNewStructureCoords(new Coordinate((int)point.X, (int)point.Y));
                this.issueCommand(State.Command.BUILD, false, null, toBuild);
            }
        }

        public BaseGameEntity createNewEntity(String entityType, State.PlayerId player){
            BaseGameEntity product = this.factory.create(entityType, player);
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

            entitiesToCommand.ForEach(delegate(BaseGameEntity e)
            {
                switch (commandType)
                {
                    ////////////////////////
                    case State.Command.MOVE:
                        //TODO: PATHING!!
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
                        //TODO: We probably shouldn't be passing unit objects through all this
                        command = new BuildCommand(args[0] as BaseGameEntity);
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
                }
            });
        }

        public void issueCommand_SingleEntity(State.Command commandType, bool append, BaseGameEntity entityToCommand, params Object[] args){
            List<BaseGameEntity> list = new List<BaseGameEntity>(1);
            list.Add(entityToCommand);

            this.issueCommand(commandType, append, list, args);
        }

        public List<BaseGameEntity> getEntitiesInRange(ref BaseGameEntity hunter, int cellSightRange){
            List<int> ids = MapManager.getInstance().currentMap.getEntitiesInRange(hunter.location, cellSightRange);
            List<BaseGameEntity> enemies = new List<BaseGameEntity>();
            State.PlayerId hOwner = hunter.owner;
            BaseGameEntity e;

            ids.ForEach(delegate(int key)
            {
                e = this.getEntity(key);
                if (e.owner != hOwner)
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
                    (this.entityBank[keyId] as Unit).setCurrentState(State.UnitState.Buried);
                }
                else if (this.entityBank[keyId] is Structure)
                {
                    (this.entityBank[keyId] as Structure).setCurrentState(State.StructureState.Destroyed);
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
                selectedUnits.Clear();
            }

            //we need to ensure that dead units are removed
            // from groups
            for(int i = 0; i < groups[groupNum].Count; i++){
                if (groups[groupNum][i].isDead())
                    groups[groupNum].RemoveAt(i);
            }

            selectedUnits.AddRange(groups[groupNum]);
        }

        public Color getColorMask(State.PlayerId player){
            return (player == State.PlayerId.HUMAN ? Color.White : Color.OrangeRed);
        }

    }
}

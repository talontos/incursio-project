using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Incursio.Managers;

namespace Incursio.Classes
{
    /// <summary>
    /// SimpleAI is a very basic, but functional, reactive artificial intelligence architecture.
    /// 
    /// AIPlayers with this AI will actively build an army, but will not actively hunt the player.
    /// This AI will also act as a BaseAI
    /// </summary>
    public class SimpleAI : BaseAI
    {

        private List<EntityBuildOrder> buildList = new List<EntityBuildOrder>();
        private int minPreferredArmySize = 10;
        private bool baseSecure = false;

        private bool preparingAssault = false;
        private bool assaultReady = false;
        private Coordinate assaultRallyPoint = null;
        private int preferredAssaultForceSize = 5;
        private List<BaseGameEntity> assaultForce = new List<BaseGameEntity>();

        //init'd at 60 so it analyzes immediately
        private int baseAnalyzeCounter = 60;

        public SimpleAI(){

        }

        /// <summary>
        /// This is where all AI actions are performed.  Actions are based off of this player's
        /// event list; along with any other rules in sub-classes
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, AIPlayer player){
            //continue building army, if necessary
            //this.queueBuildRandomUnit(true);

            //checks events
            this.processEvents(ref player);

            //processEvents will catch a creation_complete event; but if the camp is idle, we need to use it!
            if(!EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.COMPUTER)[0].isBuilding()){
                this.buildNextEntity();
            }

            //analyze defenses periodically
            if(baseAnalyzeCounter < 60){
                baseAnalyzeCounter++;
            }
            else{
                this.queueBuildRandomUnit(true, null);
                this.analyzeDefenses();

                if (baseSecure){
                    //get ready to attack player
                    this.prepareAssault();
                }
                else{
                    this.preparingAssault = false;
                }

                baseAnalyzeCounter = 0;
            }

        }

        public override void processEvents(ref AIPlayer player)
        {
            State.PlayerId id = player.id;
            EntityManager manager = EntityManager.getInstance();
            player.events.ForEach(delegate(GameEvent e)
            {
                switch (e.type)
                {
                    case State.EventType.ENEMY_CAPTURING_POINT:
                        //we need to stop them

                        //don't send everyone unless it's last one?
                        if (EntityManager.getInstance().getPlayerTotalOwnedControlPoints(id) == 1)
                        {
                            this.allUnitsAssault(e.location);
                        }
                        else{
                            //find some guys to send
                            int numAttackers = EntityManager.getInstance().getEntitiesInRange(ref e.entity, e.entity.sightRange).Count;

                            this.sendUnitsToAttack(numAttackers, e.location);
                        }
                        break;
                    case State.EventType.POINT_CAPTURED:
                        //we need to fortify this point
                        //we should build 1-2 towers near the point
                        //  in between the point and the enemy base
                        //  we also should station a few units here
                        //      *enqueue towers, enqueue OR re-assign units
                        this.buildGuardTowerNearLocation(e.entity.location);
                        break;
                    case State.EventType.UNDER_ATTACK:
                        //we should help, depending on what it is
                        if(e.entity is Hero){
                            //send help to hero?
                            //hero retreat?
                        }
                        else if(e.entity is CampStructure){
                            //send help to camp
                            this.allUnitsAssault(e.location);
                        }
                        break;
                    case State.EventType.CREATION_COMPLETE:
                        //we just finished building something;
                        //build the next thing on our list
                        if(e.entity is Unit && this.preparingAssault){
                            this.assaultForce.Add(e.entity as BaseGameEntity);
                        }

                        this.buildNextEntity();
                        break;
                }
            });

            player.events = new List<GameEvent>();
        }

        /// <summary>
        /// Orders a Guard Tower to be build between location 'c' and the Human Base
        /// </summary>
        /// <param name="c"></param>
        private void buildGuardTowerNearLocation(Coordinate c){
            this.buildList.Add(new EntityBuildOrder(
               MapManager.getInstance().currentMap.getClosestPassableLocation(EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.HUMAN)[0].location, c),
               new GuardTowerStructure())
           );
        }

        private void buildNextEntity(){
            if (buildList.Count > 0)
            {
                //pop next order off queue and build
                EntityBuildOrder order = buildList[0];
                buildList.Remove(order);

                //right now we only have one constructor-class structure; so this is okay
                CampStructure camp = EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.COMPUTER)[0];
                camp.build(order);
            }
        }

        private void queueAssaultUnit(){
            this.queueBuildRandomUnit(false, this.assaultRallyPoint);
        }

        private void queueBuildRandomUnit(bool checkCap, Coordinate location){
            List<Structure> myGuys = EntityManager.getInstance().getLivePlayerStructures(State.PlayerId.COMPUTER);

            if (EntityManager.getInstance().getLivePlayerUnits(State.PlayerId.COMPUTER).Count + this.buildList.Count <= this.minPreferredArmySize
                || !checkCap)
            {
                //'order' random unit
                int randU = Incursio.rand.Next(0, 100);
                Coordinate dest = location;

                if (dest == null)
                    dest = new Coordinate(Incursio.rand.Next(20, 2000), Incursio.rand.Next(20, 2000));

                if (randU > 60)
                    this.buildList.Add(new EntityBuildOrder(dest, new LightInfantryUnit()));

                else if (randU > 30)
                    this.buildList.Add(new EntityBuildOrder(dest, new ArcherUnit()));

                else
                    this.buildList.Add(new EntityBuildOrder(dest, new HeavyInfantryUnit()));
            }
        }

        private void sendUnitsToAttack(int num, Coordinate location){
            List<BaseGameEntity> myGuys = EntityManager.getInstance().getLivePlayerUnits(State.PlayerId.COMPUTER);
            List<BaseGameEntity> toSend = new List<BaseGameEntity>();

            //select num entities that are not busy
            for(int i = Incursio.rand.Next(0, myGuys.Count - 1); i < myGuys.Count; i++){
                if( !(myGuys[i] is Unit))
                    continue;

                if(toSend.Count < num){
                        
                    Unit u = myGuys[i] as Unit;
                    switch(u.getCurrentState()){
                        case State.UnitState.Guarding:
                        case State.UnitState.Idle:
                        case State.UnitState.Wandering:
                            toSend.Add(myGuys[i]);
                            break;
                    }
                }
            }

            EntityManager.getInstance().issueCommand(State.Command.ATTACK_MOVE, false, toSend, location);
        }

        private void sendUnitsToAttack(Coordinate location){
            EntityManager.getInstance().issueCommand(State.Command.ATTACK_MOVE, false, assaultForce, location);
        }

        private void analyzeDefenses(){
            baseSecure = true;

            List<Structure> myStructs = EntityManager.getInstance().getLivePlayerStructures(State.PlayerId.COMPUTER);

            List<BaseGameEntity> friendlies = new List<BaseGameEntity>();

            //TODO: ANALYZE ENEMIES?
            List<BaseGameEntity> enemies = new List<BaseGameEntity>();
            int numTowers;
            int numMen;
            int numEnemies;

            myStructs.ForEach(delegate(Structure s)
            {
                numTowers = 0;
                numMen = 0;
                numEnemies = 0;

                //ignore towers?
                if( !(s is GuardTowerStructure) ){
                    //make sure we have towers and men guarding it
                    EntityManager.getInstance().getAllEntitiesInRange( s.owner, s.location, s.sightRange, out friendlies, out enemies);

                    friendlies.ForEach(delegate(BaseGameEntity e)
                    {
                        if (e is Unit)
                            numMen++;
                        else if (e is GuardTowerStructure)
                            numTowers++;
                        
                    });

                    if(numTowers < 1){
                        //BUILD SOME TOWERS!
                        this.buildGuardTowerNearLocation(s.location);
                        baseSecure = false;
                    }

                    if(numMen < 2){
                        //GET SOME MEN OVER HERE!
                        this.queueBuildRandomUnit(false, s.location);
                        baseSecure = false;
                    }
                }
            });
        }

        private void prepareAssault(){
            //FIXME OMG: Queue is populating TOO FAST
            if (preparingAssault == false)
                this.buildList = new List<EntityBuildOrder>();

            this.preparingAssault = true;

            //TODO: DECIDE WHAT TO ATTACK
            if(assaultRallyPoint == null){
                assaultRallyPoint = MapManager.getInstance().currentMap.getClosestPassableLocation(
                    EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.HUMAN)[0].location,
                    EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.COMPUTER)[0].location);
            }

            assaultReady = (assaultForce.Count >= preferredAssaultForceSize);

            if(assaultReady){
                //ATTACK!!!!
                preparingAssault = false;
                this.sendUnitsToAttack(EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.HUMAN)[0].location);
            }
            else{
                //queue unit
                if (this.buildList.Count == 0)
                    this.queueAssaultUnit();
            }

        }
    }
}

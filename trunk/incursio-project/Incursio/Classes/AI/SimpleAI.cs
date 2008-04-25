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
        private int minPreferredArmySize = 20;

        private int baseAnalyzeCounter = 0;

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
            this.queueBuildRandomUnit(true);

            //checks events
            this.processEvents(ref player);

            //processEvents will catch a creation_complete event; but if the camp is idle, we need to use it!
            if(!EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.COMPUTER)[0].isBuilding()){
                this.buildNextEntity();
            }

            //analyze defenses
            if(baseAnalyzeCounter < 60){
                baseAnalyzeCounter++;
            }
            else{
                this.analyzeDefenses();
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
                        }
                        break;
                    case State.EventType.CREATION_COMPLETE:
                        //we just finished building something;
                        //build the next thing on our list
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

                /*
                if (order.entity is GuardTowerStructure)
                    camp.setNewStructureCoords(order.location);
                else
                    camp.setDestination(order.location);
                */
            }
        }

        private void queueBuildRandomUnit(bool checkCap){
            List<Structure> myGuys = EntityManager.getInstance().getLivePlayerStructures(State.PlayerId.COMPUTER);

            if (EntityManager.getInstance().getLivePlayerUnits(State.PlayerId.COMPUTER).Count + this.buildList.Count <= this.minPreferredArmySize
                || !checkCap)
            {
                //'order' random unit
                int randU = Incursio.rand.Next(0, 100);
                Coordinate dest = new Coordinate(Incursio.rand.Next(20, 2000), Incursio.rand.Next(20, 2000));

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

        private void analyzeDefenses(){
            List<Structure> myStructs = EntityManager.getInstance().getLivePlayerStructures(State.PlayerId.COMPUTER);

            List<BaseGameEntity> surroundings = new List<BaseGameEntity>();
            int numTowers = 0;
            int numMen = 0;
            int numEnemies = 0;

            myStructs.ForEach(delegate(Structure s)
            {
                //ignore towers?
                if( !(s is GuardTowerStructure) ){
                    //make sure we have towers and men guarding it
                    surroundings = EntityManager.getInstance().getEntitiesInRange( (s as BaseGameEntity), s.sightRange);

                    surroundings.ForEach(delegate(BaseGameEntity e)
                    {
                        if (e.owner == State.PlayerId.COMPUTER)
                        {
                            if (e is Unit)
                                numMen++;
                            else if (e is GuardTowerStructure)
                                numTowers++;
                        }
                        else
                            numEnemies++;
                    });

                    if(numTowers == 0){
                        //BUILD SOME TOWERS!
                        this.buildGuardTowerNearLocation(s.location);
                    }

                    if(numMen == 0){
                        //GET SOME MEN OVER HERE!
                        this.queueBuildRandomUnit(false);
                    }
                }
            });

        }
    }
}

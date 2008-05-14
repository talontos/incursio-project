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
        private int minPreferredArmySize = 15;
        private bool baseSecure = false;

        private bool preparingAssault = false;
        private bool assaultReady = false;
        private bool assaultWithHero = false;
        private Coordinate assaultRallyPoint = null;
        private Coordinate assaultTarget = null;
        private int preferredAssaultForceSize = 5;
        private List<BaseGameEntity> assaultForce = new List<BaseGameEntity>();

        //init'd at 60 so it analyzes immediately
        private int baseAnalyzeCounter = 60;

        public SimpleAI(){
            this.minPreferredArmySize = Incursio.rand.Next(10, 16);
            this.preferredAssaultForceSize = Incursio.rand.Next(3, 10);
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
            List<CampStructure> camps = EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.COMPUTER);
            if(camps.Count == 0){
                return;
            }

            //checks events
            this.processEvents(ref player);

            //processEvents will catch a creation_complete event; but if the camp is idle, we need to use it!
            if(!camps[0].isBuilding()){
                this.buildNextEntity();
            }

            //analyze defenses periodically
            if(baseAnalyzeCounter < 60){
                baseAnalyzeCounter++;
            }
            else{
                this.queueBuildRandomUnit(true, null, null);
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
                        this.buildGuardTowerNearLocation(e.entity.location, null);
                        break;
                    case State.EventType.UNDER_ATTACK:
                        //we should help, depending on what it is
                        if(e.entity is Hero){
                            //send help to hero?
                            //hero retreat?
                            if(e.entity.health < e.entity.maxHealth * 0.4){
                                EntityManager.getInstance().issueCommand_SingleEntity(State.Command.MOVE,
                                    false, e.entity, 
                                    EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.COMPUTER)[0].location);
                            }
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
        private void buildGuardTowerNearLocation(Coordinate c, KeyPoint k){
            this.buildList.Add(new EntityBuildOrder(
               MapManager.getInstance().currentMap.getClosestPassableLocation(EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.HUMAN)[0].location, c),
               State.EntityName.GuardTower,
               k)
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
            this.queueBuildRandomUnit(false, this.assaultRallyPoint, null);
        }

        private void queueBuildRandomUnit(bool checkCap, Coordinate location, KeyPoint keyPoint){
            List<Structure> myGuys = EntityManager.getInstance().getLivePlayerStructures(State.PlayerId.COMPUTER);
            
            if (EntityManager.getInstance().getLivePlayerUnits(State.PlayerId.COMPUTER).Count + this.buildList.Count <= this.minPreferredArmySize
                || !checkCap)
            {
                //'order' random unit
                int randU = Incursio.rand.Next(0, 100);
                Coordinate dest = location;

                if (dest == null){
                    dest = new Coordinate(Incursio.rand.Next(20, MapManager.getInstance().currentMap.width_pix),
                                            Incursio.rand.Next(20, MapManager.getInstance().currentMap.height_pix));
                }

                if (randU > 60)
                    this.buildList.Add(new EntityBuildOrder(dest, State.EntityName.LightInfantry, keyPoint));

                else if (randU > 30)
                    this.buildList.Add(new EntityBuildOrder(dest, State.EntityName.Archer, keyPoint));

                else
                    this.buildList.Add(new EntityBuildOrder(dest, State.EntityName.HeavyInfantry, keyPoint));
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
            List<DefenseReport> reports = EntityManager.getInstance().updatePlayerKeyPoints(State.PlayerId.COMPUTER);
            this.baseSecure = true;

            reports.ForEach(delegate(DefenseReport r)
            {
                if(!r.secure){
                    this.baseSecure = false;

                    int i;
                    //point not secure, order stuff for it
                    for(i = 0; i < r.numUnitsToBuild; i++){
                        this.queueBuildRandomUnit(false, r.location, r.keyPoint);
                    }

                    for(i = 0; i < r.numTowersToBuild; i++){
                        this.buildGuardTowerNearLocation(r.location, r.keyPoint);
                    }
                }
            });
        }

        private void analyzeEnemyDefenses(){
            List<KeyPoint> points = EntityManager.getInstance().getPlayerKeyPoints(State.PlayerId.HUMAN);

            points.ForEach(delegate(KeyPoint p)
            {
                //TODO: BETTER ANALYSIS
                //p.priority = p.numGuardTowers + p.numUnits - this.assaultForce.Count;

                
                if(p.numGuardTowers == 0 && p.numUnits == 0){
                    //perfect time to attack
                    p.priority = 5;
                }
                else if(p.numGuardTowers == 0 && p.numUnits < this.assaultForce.Count){
                    //not a bad target
                    p.priority = 4;
                }
                else if(p.numUnits == 0 && p.numGuardTowers < this.assaultForce.Count/2){
                    p.priority = 3;
                }
                //TODO: perhaps differentiate between camps and CPs?
                else{
                    //they have many guard towers and many men...let's put this off a bit
                    p.priority = 1;
                }
                
            });

            //Find the highest priority target (best chance of sucess)

            //priority defaults to -1
            KeyPoint target = new KeyPoint();

            for(int i = 0; i < points.Count; i++){
                if(points[i].priority > target.priority){
                    target = points[i];
                }
            }

            this.assaultTarget = target.structure.location;

            //if target is a camp, the hero can sit back
            //   if it's a control point, we'll need him to capture it
            if(target.structure is CampStructure){
                //don't really need hero
                assaultWithHero = false;
            }
            else if(target.structure is ControlPoint){
                //include hero in attack
                assaultWithHero = true;
            }

            //EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.HUMAN)[0].location
        }

        private void prepareAssault()
        {
            this.preparingAssault = true;

            if(assaultRallyPoint == null){
                assaultRallyPoint = MapManager.getInstance().currentMap.getClosestPassableLocation(
                    EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.HUMAN)[0].location,
                    EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.COMPUTER)[0].location);
            }

            assaultReady = (assaultForce.Count >= preferredAssaultForceSize);

            if(assaultReady){
                //DECIDE WHAT TO ATTACK
                this.analyzeEnemyDefenses();

                //ATTACK!!!!
                preparingAssault = false;
                if(assaultWithHero){
                    this.assaultForce.Add(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.COMPUTER)[0]);
                }
                this.sendUnitsToAttack(this.assaultTarget);
                this.assaultForce = new List<BaseGameEntity>();

                this.preferredAssaultForceSize = Incursio.rand.Next(3, 10);
            }
            else{
                //queue unit
                if (this.buildList.Count == 0)
                    this.queueAssaultUnit();
            }

        }
    }
}

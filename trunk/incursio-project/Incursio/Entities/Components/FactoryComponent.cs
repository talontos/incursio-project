using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;
using Incursio.Utils;
using Incursio.Managers;
using Incursio.Commands;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Incursio.Entities.Components
{
    public class FactoryComponent : BaseComponent
    {
        public int buildCostToTimeFactor = 9;

        //TEMPORARY HARD-CODE-----------------
        /*
        public const int LIGHT_INFANTRY_BUILD_TIME = 5;
        public const int HEAVY_INFANTRY_BUILD_TIME = 10;
        public const int ARCHER_BUILD_TIME = 7;
        public const int GUARD_TOWER_BUILD_TIME = 15;

        public int COST_LIGHT_INFANTRY = EntityConfiguration.EntityPrices.COST_LIGHT_INFANTRY;
        public int COST_HEAVY_INFANTRY = EntityConfiguration.EntityPrices.COST_HEAVY_INFANTRY;
        public int COST_ARCHER = EntityConfiguration.EntityPrices.COST_ARCHER;
        public int COST_GUARD_TOWER = EntityConfiguration.EntityPrices.COST_GUARD_TOWER;
        */

        int newUnitPlacementX = 10;
        int newUnitPlacementY = 120;    //little bit of hard coding, but can't really help it here 
        //-------------------------------------

        public int timeBuilt = 0;
        public int timeRequired = 0;
        public EntityBuildOrder buildProject;
        public string currentlyBuildingThis = "";
        public int currentBuildForObjectFactory = -1;
        public Coordinate destination;
        public Coordinate newStructureCoords;
        public Coordinate spawnPoint;

        public List<EntityBuildOrder> buildOrders;

        //TODO: should we be able to define a list of buildable entities?
        //  so that certain buildings build certain entities
        //public List<int> buildableEntityIds;

        public FactoryComponent(BaseGameEntity entity) : base(entity){
            entity.isConstructor = true;
            buildOrders = new List<EntityBuildOrder>();

            setSpawnAndDestination();
        }

        public override void setAttributes(List<KeyValuePair<string, string>> attributes)
        {
            base.setAttributes(attributes);

            for(int i = 0; i < attributes.Count; i++){
                switch(attributes[i].Key){
                    case "builtTimeFactor": this.buildCostToTimeFactor = int.Parse(attributes[i].Value); break;
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if(buildProject == null && this.buildOrders.Count > 0){
                this.build(buildOrders[0]);
                buildOrders.RemoveAt(0);
            }

            this.buildTick();
        }

        public void setSpawnAndDestination(){
            this.destination = new Coordinate(bgEntity.location.x + newUnitPlacementX, bgEntity.location.y + newUnitPlacementY);
            spawnPoint = new Coordinate(bgEntity.location.x, bgEntity.location.y + 10);
        }

        public void addOrder(EntityBuildOrder o){
            this.buildOrders.Add(o);
        }

        public void build(EntityBuildOrder toBeBuilt)
        {
            if (buildProject != null)
            {
                //add toBeBuild to the queue
                this.buildOrders.Add(toBeBuilt);
            }
            else
            {
                Player owningPlayer = PlayerManager.getInstance().getPlayerById(this.bgEntity.owner);

                if (toBeBuilt.location != null)
                {
                    //TODO: FIX FOR STRUCTURES
                    /*if (toBeBuilt.entity == State.EntityName.GuardTower)
                    {
                        this.setNewStructureCoords(toBeBuilt.location);
                    }
                    else
                    {
                        this.setDestination(toBeBuilt.location);
                    }
                    */
                    this.setDestination(toBeBuilt.location);
                }
                else if (this.destination != null)
                {
                    toBeBuilt.location = this.destination;
                }
                else
                {
                    //can't do it... :-(
                    Console.WriteLine("FACTORY COMPONENT CANNOT BUILD: location and destination are null");
                    return;
                }

                BaseGameEntityConfiguration config = ObjectFactory.getInstance().entities[toBeBuilt.entityId];

                if (owningPlayer.MONETARY_UNIT >= config.costToBuild)
                {
                    this.currentlyBuildingThis = config.className;
                    this.currentBuildForObjectFactory = config.classID;

                    this.timeBuilt = 0;
                    this.timeRequired = (config.costToBuild / this.buildCostToTimeFactor) * 60;
                    this.buildProject = toBeBuilt;

                    this.bgEntity.currentState = State.EntityState.Building;
                    owningPlayer.MONETARY_UNIT = owningPlayer.MONETARY_UNIT - config.costToBuild;
                }
                else
                {
                    //send message to player
                    owningPlayer.dispatchEvent(
                        new GameEvent(
                            State.EventType.NOT_ENOUGH_RESOURCES,
                            this.bgEntity,
                            "",
                            "Not Enough Resources",
                            this.bgEntity.location
                        )
                    );
                }
            }
        }

        public void buildTick()
        {
            if (this.buildProject == null)
                return;

            //check for order cancellation
            if (this.buildProject.keyPoint != null && this.buildProject.keyPoint.owner != this.bgEntity.owner)
            {
                //can't build anymore
                timeBuilt = 0;
                timeRequired = 0;
                this.bgEntity.setIdle();
                this.currentBuildForObjectFactory = -1;
                this.currentlyBuildingThis = "IDLE";

                PlayerManager.getInstance().notifyPlayer(
                    this.bgEntity.owner,
                    new GameEvent(State.EventType.CANT_MOVE_THERE,
                        this.bgEntity,
                        SoundManager.getInstance().AudioCollection.messages.cantBuild,
                        "Cannot Build There",
                        this.buildProject.location
                    )
                );

                this.buildProject = null;
                return;
            }

            if (this.timeBuilt >= this.timeRequired)
            {
                //if (buildProject.entity != State.EntityName.GuardTower)
                //{
                    BaseGameEntity temp = (EntityManager.getInstance().createNewEntity(currentBuildForObjectFactory, this.bgEntity.owner));
                    temp.setLocation(this.spawnPoint);
                    temp.issueSingleOrder(new MoveCommand(this.destination));
                    timeBuilt = 0;
                    timeRequired = 0;
                    this.bgEntity.setIdle();
                    this.currentBuildForObjectFactory = -1;
                    this.currentlyBuildingThis = "IDLE";

                    if (this.buildProject.keyPoint != null)
                    {
                        this.buildProject.keyPoint.numUnitsOrdered--;
                    }

                    this.buildProject = null;

                    PlayerManager.getInstance().notifyPlayer(
                        this.bgEntity.owner,
                        new GameEvent(State.EventType.CREATION_COMPLETE,
                            temp,
                            "",
                            "Unit Ready",
                            temp.location
                        )
                    );

                    temp.playEnterBattlefieldSound();
                //}
                //TODO: REWORK FOR STRUCTURES!
                /*
                else
                {
                    GuardTowerStructure temp = (EntityManager.getInstance().createNewEntity(currentBuildForObjectFactory, this.bgEntity.owner) as GuardTowerStructure);
                    temp.setLocation(this.newStructureCoords);
                    timeBuilt = 0;
                    timeRequired = 0;
                    this.bgEntity.setIdle();
                    this.currentBuildForObjectFactory = "IDLE";
                    this.currentlyBuildingThis = "IDLE";

                    if (this.buildProject.keyPoint != null)
                    {
                        this.buildProject.keyPoint.numGuardTowersOrdered--;
                    }

                    this.buildProject = null;

                    PlayerManager.getInstance().notifyPlayer(
                        this.bgEntity.owner,
                        new GameEvent(State.EventType.CREATION_COMPLETE,
                            temp,
                            SoundManager.getInstance().AudioCollection.messages.towerBuilt,
                            "Construction Complete",
                            this.bgEntity.location
                        )
                    );
                }
                */
            }
            else
            {
                this.timeBuilt++;
            }
        }

        public void setDestination(Coordinate dest)
        {
            if (dest != null)
                this.destination = dest;
        }

        public void setNewStructureCoords(Coordinate coords)
        {
            if (coords != null && MapManager.getInstance().currentMap.getCellOccupancy_pixels(coords.x, coords.y) == (byte)1)
            {
                this.newStructureCoords = coords;
            }
            else
            {
                PlayerManager.getInstance().notifyPlayer(
                    this.bgEntity.owner,
                    new GameEvent(State.EventType.CANT_MOVE_THERE,
                        this.bgEntity,
                        SoundManager.getInstance().AudioCollection.messages.cantBuild,
                        "Cannot Build There",
                        this.bgEntity.location
                    )
                );
            }
        }

        public bool isBuilding(){
            return this.buildProject != null;
        }

        public void drawBuildQueue(SpriteBatch spriteBatch)
        {
            //debugging; draw my queue
            if (this.buildOrders.Count > 0)
            {
                string orderList = "";
                if (this.isBuilding())
                    orderList += "0: " + this.buildProject.entity.ToString() + "\n";

                for (int i = 0; i < this.buildOrders.Count; i++)
                {
                    orderList += (i + 1) + ": " + this.buildOrders[i].entity.ToString() + "\n";
                }

                spriteBatch.DrawString(Incursio.getInstance().getFont_Courier(), orderList, new Vector2(0, 0), Color.White);
            }
        }

        public double getPercentDone()
        {
            if (this.isBuilding())
            {
                return (float)timeBuilt / timeRequired;
            }
            else return -1.0;

        }
    }
}

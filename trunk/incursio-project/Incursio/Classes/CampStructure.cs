using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Managers;
using Incursio.Commands;

namespace Incursio.Classes
{
    public class CampStructure : Structure
    {
        //constants
        public const int LIGHT_INFANTRY_BUILD_TIME = 5;
        public const int HEAVY_INFANTRY_BUILD_TIME = 10;
        public const int ARCHER_BUILD_TIME = 7;
        public const int GUARD_TOWER_BUILD_TIME = 15;
        public const int ITEM_UPGRADE_BUILD_TIME = 90;

        public const int COST_LIGHT_INFANTRY = 45;
        public const int COST_HEAVY_INFANTRY = 90;
        public const int COST_ARCHER = 60;
        public const int COST_GUARD_TOWER = 200;
        public const int COST_UPGRADE = 200;

        int newUnitPlacementX = 10;
        int newUnitPlacementY = 120;    //little bit of hard coding, but can't really help it here 
        int income = 8;

        Coordinate destination;
        Coordinate newStructureCoords;

        String currentlyBuildingThis = "IDLE"; //this is for the HUD to display what it's building
        String currentBuildForObjectFactory = "IDLE"; //and this is for the object factory

        public CampStructure() : base(){
            //TODO: Set camp properties
            this.maxHealth = 350;
            this.health = 350;
            this.sightRange = 500;
            this.setType(State.EntityName.Camp);
            this.map = Incursio.getInstance().currentMap;

            setDefaultDestination();

            this.isConstructor = true;
        }

        public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
        {
            this.updateOccupancy(true);
            base.Update(gameTime, ref myRef);
        }

        public override void build(BaseGameEntity toBeBuilt)
        {
            if (buildProject != null)
            {

            }
            else
            {
                Player owningPlayer;
                if (this.owner == State.PlayerId.HUMAN)
                    owningPlayer = PlayerManager.getInstance().humanPlayer;
                else
                    owningPlayer = PlayerManager.getInstance().computerPlayer;

                if (toBeBuilt.getType() == State.EntityName.LightInfantry)
                {
                    //if we have enough resources, build it
                    if (owningPlayer.MONETARY_UNIT >= COST_LIGHT_INFANTRY)
                    {
                        currentlyBuildingThis = "Light Infantry";
                        currentBuildForObjectFactory = "Incursio.Classes.LightInfantryUnit";
                        this.timeBuilt = 0;
                        this.timeRequired = LIGHT_INFANTRY_BUILD_TIME * 60;
                        this.buildProject = toBeBuilt;
                        this.currentState = State.StructureState.Building;
                        owningPlayer.MONETARY_UNIT = owningPlayer.MONETARY_UNIT - COST_LIGHT_INFANTRY;
                    }
                    else
                    {
                        //send message to player
                        owningPlayer.dispatchEvent(
                            new GameEvent(
                                State.EventType.NOT_ENOUGH_RESOURCES,
                                "Not Enough Resources",
                                this.location
                            )
                        );
                        //MessageManager.getInstance().addMessage("Not enough resources");
                    }
                    
                }
                else if (toBeBuilt.getType() == State.EntityName.Archer)
                {
                    //if we have enough resources, build it
                    if (owningPlayer.MONETARY_UNIT >= COST_ARCHER)
                    {
                        currentlyBuildingThis = "Archer";
                        currentBuildForObjectFactory = "Incursio.Classes.ArcherUnit";
                        this.timeBuilt = 0;
                        this.timeRequired = ARCHER_BUILD_TIME * 60;
                        this.buildProject = toBeBuilt;
                        this.currentState = State.StructureState.Building;
                        owningPlayer.MONETARY_UNIT = owningPlayer.MONETARY_UNIT - COST_ARCHER;
                    }
                    else
                    {
                        //send message to player
                        owningPlayer.dispatchEvent(
                            new GameEvent(
                                State.EventType.NOT_ENOUGH_RESOURCES,
                                "Not Enough Resources",
                                this.location
                            )
                        );
                        //MessageManager.getInstance().addMessage("Not enough resources");
                    }
                    
                }
                else if (toBeBuilt.getType() == State.EntityName.HeavyInfantry)
                {
                    //if we have enough resources, build it
                    if (owningPlayer.MONETARY_UNIT >= COST_HEAVY_INFANTRY)
                    {
                        currentlyBuildingThis = "Heavy Infantry";
                        currentBuildForObjectFactory = "Incursio.Classes.HeavyInfantryUnit";
                        this.timeBuilt = 0;
                        this.timeRequired = HEAVY_INFANTRY_BUILD_TIME * 60;
                        this.buildProject = toBeBuilt;
                        this.currentState = State.StructureState.Building;
                        owningPlayer.MONETARY_UNIT = owningPlayer.MONETARY_UNIT - COST_HEAVY_INFANTRY;
                    }
                    else
                    {
                        //send message to player
                        owningPlayer.dispatchEvent(
                            new GameEvent(
                                State.EventType.NOT_ENOUGH_RESOURCES,
                                "Not Enough Resources",
                                this.location
                            )
                        );
                        //MessageManager.getInstance().addMessage("Not enough resources");
                    }
  
                }
                else if (toBeBuilt.getType() == State.EntityName.GuardTower)
                {
                    //if we have enough resources, build it
                    if (owningPlayer.MONETARY_UNIT >= COST_GUARD_TOWER)
                    {
                        currentlyBuildingThis = "Guard Tower";
                        currentBuildForObjectFactory = "Incursio.Classes.GuardTowerStructure";
                        this.timeBuilt = 0;
                        this.timeRequired = GUARD_TOWER_BUILD_TIME * 60;
                        this.buildProject = toBeBuilt;
                        this.currentState = State.StructureState.Building;
                        owningPlayer.MONETARY_UNIT = owningPlayer.MONETARY_UNIT - COST_GUARD_TOWER;
                    }
                    else
                    {
                        //send message to player
                        owningPlayer.dispatchEvent(
                            new GameEvent(
                                State.EventType.NOT_ENOUGH_RESOURCES,
                                "Not Enough Resources",
                                this.location
                            )
                        );
                        //MessageManager.getInstance().addMessage("Not enough resources");
                    }
                    
                }
            } 
        }

        public override void updateResourceTick()
        {
            //give the owner money
            if (timeForResource >= RESOURCE_TICK * 60)
            {
                timeForResource = 0;
                if (this.owner == State.PlayerId.HUMAN)
                {
                    PlayerManager.getInstance().humanPlayer.MONETARY_UNIT = PlayerManager.getInstance().humanPlayer.MONETARY_UNIT + income;
                }
                else
                {
                    PlayerManager.getInstance().computerPlayer.MONETARY_UNIT = PlayerManager.getInstance().computerPlayer.MONETARY_UNIT + income;
                }
            }
            else
            {
                timeForResource++;
            }
        }

        public override void buildTick()
        {
            if (this.timeBuilt >= this.timeRequired)
            {
                if (buildProject.getType() != State.EntityName.GuardTower)
                {
                    Unit temp = ( EntityManager.getInstance().createNewEntity(currentBuildForObjectFactory, this.owner) as Unit);
                    temp.setLocation(this.location);
                    temp.issueSingleOrder(new MoveCommand(this.destination));
                    timeBuilt = 0;
                    timeRequired = 0;
                    this.currentState = State.StructureState.Idle;
                    this.currentBuildForObjectFactory = "IDLE";
                    this.currentlyBuildingThis = "IDLE";
                    this.buildProject = null;

                    PlayerManager.getInstance().notifyPlayer(
                        this.owner,
                        new GameEvent(State.EventType.CREATION_COMPLETE,
                        /*SOUND,*/
                            "Construction Complete",
                            this.location
                        )
                    );
                }
                else
                {
                    GuardTowerStructure temp = (EntityManager.getInstance().createNewEntity(currentBuildForObjectFactory, this.owner) as GuardTowerStructure);
                    temp.setLocation(newStructureCoords);
                    timeBuilt = 0;
                    timeRequired = 0;
                    this.currentState = State.StructureState.Idle;
                    this.currentBuildForObjectFactory = "IDLE";
                    this.currentlyBuildingThis = "IDLE";
                    this.buildProject = null;
                    
                    PlayerManager.getInstance().notifyPlayer(
                        this.owner,
                        new GameEvent(State.EventType.CREATION_COMPLETE, 
                            /*SOUND,*/ 
                            "Construction Complete", 
                            this.location
                        )
                    );
                }
            }
            else
            {
                this.timeBuilt++;
            }
        }

        public String getCurrentlyBuilding()
        {
            return currentlyBuildingThis;
        }

        public override void setDestination(Coordinate dest)
        {
            this.destination = dest;
        }

        public void setNewStructureCoords(Coordinate coords)
        {
            this.newStructureCoords = coords;
        }

        public override void updateOccupancy(bool occupied)
        {
            //hardcode blagh
            int xStart = location.x - 32;
            int yStart = location.y - (int)(32 * 0.80);
            int xEnd = location.x;// +32;
            int yEnd = location.y + (int)(32 * 0.20);

            if (xStart < 0 || xEnd < 0 || yStart < 0 || yEnd < 0)
                return;

            map.setSingleCellOccupancy(xStart, yStart, 0);
            map.setSingleCellOccupancy(xStart, yEnd, 0);
            map.setSingleCellOccupancy(xEnd, yStart, 0);
            map.setSingleCellOccupancy(xEnd, yEnd, 0);
        }

        public override void setLocation(Coordinate coords)
        {
            updateOccupancy(false);

            base.setLocation(coords);

            updateOccupancy(true);

            setDefaultDestination();
        }

        public override void updateBounds()
        {
            Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.campTextureComputer;

            this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
                location.x - myRef.Width / 2,
                (int)(location.y - myRef.Height * 0.80),
                myRef.Width,
                myRef.Height
            );
        }

        public double getPercentDone()
        {
            if (this.isBuilding())
            {
                return (float)timeBuilt / timeRequired;
            }
            else return -1.0;
            
        }

        private void setDefaultDestination(){
            this.destination = new Coordinate(this.location.x + newUnitPlacementX, this.location.y + newUnitPlacementY);
        }

        /// <summary> 
        /// For camps, we don't want to lose our queue.
        /// If we are issued a 'single' order (which by default empties order queue),
        /// we want to execute this order immediately and then continue our build queue.
        /// </summary>
        /// <param name="order">Order to be issued</param>
        public override void issueSingleOrder(BaseCommand order)
        {
            base.issueImmediateOrder(order);
        }

    }
}

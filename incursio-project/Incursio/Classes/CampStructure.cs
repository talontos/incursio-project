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

        public override void build(BaseGameEntity toBeBuilt)
        {
            if (buildProject != null)
            {

            }
            else
            {
                if (toBeBuilt.getType() == State.EntityName.LightInfantry)
                {
                    currentlyBuildingThis = "Light Infantry";
                    currentBuildForObjectFactory = "Incursio.Classes.LightInfantryUnit";
                    this.timeBuilt = 0;
                    this.timeRequired = LIGHT_INFANTRY_BUILD_TIME * 60;
                    this.buildProject = toBeBuilt;
                    this.currentState = State.StructureState.Building;
                }
                else if (toBeBuilt.getType() == State.EntityName.Archer)
                {
                    currentlyBuildingThis = "Archer";
                    currentBuildForObjectFactory = "Incursio.Classes.ArcherUnit";
                    this.timeBuilt = 0;
                    this.timeRequired = ARCHER_BUILD_TIME * 60;
                    this.buildProject = toBeBuilt;
                    this.currentState = State.StructureState.Building;
                }
                else if (toBeBuilt.getType() == State.EntityName.HeavyInfantry)
                {
                    currentlyBuildingThis = "Heavy Infantry";
                    currentBuildForObjectFactory = "Incursio.Classes.HeavyInfantryUnit";
                    this.timeBuilt = 0;
                    this.timeRequired = HEAVY_INFANTRY_BUILD_TIME * 60;
                    this.buildProject = toBeBuilt;
                    this.currentState = State.StructureState.Building;
                }
                else if (toBeBuilt.getType() == State.EntityName.GuardTower)
                {
                    currentlyBuildingThis = "Guard Tower";
                    currentBuildForObjectFactory = "Incursio.Classes.GuardTowerStructure";
                    this.timeBuilt = 0;
                    this.timeRequired = GUARD_TOWER_BUILD_TIME * 60;
                    this.buildProject = toBeBuilt;
                    this.currentState = State.StructureState.Building;
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

        public override void setLocation(Coordinate coords)
        {
            //hardcode blargh
            int xStart = coords.x - 32;
            int yStart = coords.y - (int)(64 * 0.80);
            int xEnd = coords.x + 32;
            int yEnd = coords.y + (int)(64 * 0.20);

            map.setSingleCellOccupancy(xStart, yStart, false);
            map.setSingleCellOccupancy(xStart, yEnd, false);
            map.setSingleCellOccupancy(xEnd, yStart, false);
            map.setSingleCellOccupancy(xEnd, yEnd, false);

            base.setLocation(coords);

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

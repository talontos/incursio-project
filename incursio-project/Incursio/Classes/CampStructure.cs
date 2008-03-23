using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Managers;

namespace Incursio.Classes
{
    public class CampStructure : Structure
    {
        //constants
        const int LIGHT_INFANTRY_BUILD_TIME = 5;
        const int HEAVY_INFANTRY_BUILD_TIME = 10;
        const int ARCHER_BUILD_TIME = 7;
        const int GUARD_TOWER_BUILD_TIME = 90;
        const int ITEM_UPGRADE_BUILD_TIME = 90;

        int newUnitPlacementX = 10;
        int newUnitPlacementY = 120;    //little bit of hard coding, but can't really help it here 

        String currentlyBuildingThis = "IDLE"; //this is for the HUD to display what it's building
        String currentBuildForObjectFactory = "IDLE"; //and this is for the object factory

        public CampStructure() : base(){
            //TODO: Set camp properties
            this.maxHealth = 350;
            this.health = 350;
            this.sightRange = 500;
            this.setType(State.EntityName.Camp);
            this.map = Incursio.getInstance().currentMap;
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
                    currentBuildForObjectFactory = "Incursio.Classes.GuardTower";
                    this.timeBuilt = 0;
                    this.timeRequired = GUARD_TOWER_BUILD_TIME * 60;
                    this.buildProject = toBeBuilt;
                    this.currentState = State.StructureState.Building;
                }
            } 
        }

        public override void buildTick()
        {
            if (this.timeBuilt >= this.timeRequired)
            {
                if (buildProject.getType() != State.EntityName.GuardTower)
                {
                    Unit temp = ( EntityManager.getInstance().createNewEntity(currentBuildForObjectFactory, this.owner) as Unit);
                    temp.setLocation(new Coordinate(this.location.x + newUnitPlacementX, this.location.y + newUnitPlacementY));
                    timeBuilt = 0;
                    timeRequired = 0;
                    this.currentState = State.StructureState.Idle;

                    this.buildProject = null;
                }
                else
                {

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
        }
    }
}

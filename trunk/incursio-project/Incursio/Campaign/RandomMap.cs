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


using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Campaign
{
    public class RandomMap : CampaignMap
    {
        int mapWidth = 2048;
        int mapHeight = 1024;

        public RandomMap() : base(){
            this.level = State.CampaignLevel.ONE;

            this.setMapDimensions(mapWidth, mapHeight, 1024, 768);
        }

        public override void initializeMap()
        {
            base.initializeMap();
            //This map right now will represent our test environment

            EntityManager entityManager = EntityManager.getInstance();

            //create camps in random locations
            CampStructure playerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", PlayerManager.getInstance().currentPlayerId);
            playerCamp.setLocation( new Coordinate(Incursio.rand.Next(mapWidth / 2), Incursio.rand.Next(mapHeight - 100)));

            CampStructure computerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", PlayerManager.getInstance().computerPlayerId);
            computerCamp.setLocation( new Coordinate(Incursio.rand.Next(mapWidth / 2, mapWidth), Incursio.rand.Next(mapHeight - 100)));


            //place random units around the map
            int numLight = Incursio.rand.Next(10);
            int numHeavy = Incursio.rand.Next(10);
            int numArch = Incursio.rand.Next(10);
            int numTower = Incursio.rand.Next(10);
            int numPoints = Incursio.rand.Next(5);
            
            //lights
            for(int i = 0; i < numLight; i++){
                LightInfantryUnit infUnitp = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", PlayerManager.getInstance().currentPlayerId);
                infUnitp.setLocation(new Coordinate(Incursio.rand.Next(mapWidth), Incursio.rand.Next(mapHeight)));
                LightInfantryUnit infUnitc = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", PlayerManager.getInstance().computerPlayerId);
                infUnitc.setLocation(new Coordinate(Incursio.rand.Next(mapWidth), Incursio.rand.Next(mapHeight)));
            }

            //heavys
            for (int i = 0; i < numHeavy; i++)
            {
                HeavyInfantryUnit infUnitp = (HeavyInfantryUnit)entityManager.createNewEntity("Incursio.Classes.HeavyInfantryUnit", PlayerManager.getInstance().currentPlayerId);
                infUnitp.setLocation(new Coordinate(Incursio.rand.Next(mapWidth), Incursio.rand.Next(mapHeight)));
                HeavyInfantryUnit infUnitc = (HeavyInfantryUnit)entityManager.createNewEntity("Incursio.Classes.HeavyInfantryUnit", PlayerManager.getInstance().computerPlayerId);
                infUnitc.setLocation(new Coordinate(Incursio.rand.Next(mapWidth), Incursio.rand.Next(mapHeight)));
            }

            //archers
            for (int i = 0; i < numArch; i++)
            {
                ArcherUnit archUnitp = (ArcherUnit)entityManager.createNewEntity("Incursio.Classes.ArcherUnit", PlayerManager.getInstance().currentPlayerId);
                archUnitp.setLocation(new Coordinate(Incursio.rand.Next(mapWidth), Incursio.rand.Next(mapHeight)));
                ArcherUnit archUnitc = (ArcherUnit)entityManager.createNewEntity("Incursio.Classes.ArcherUnit", PlayerManager.getInstance().computerPlayerId);
                archUnitc.setLocation(new Coordinate(Incursio.rand.Next(mapWidth), Incursio.rand.Next(mapHeight)));
            }

            //towers
            for (int i = 0; i < numTower; i++)
            {
                GuardTowerStructure playerTowerp = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", PlayerManager.getInstance().currentPlayerId);
                playerTowerp.setLocation(new Coordinate(Incursio.rand.Next(50, mapWidth - 50), Incursio.rand.Next(50, mapHeight - 50)));

                GuardTowerStructure playerTowerc = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", PlayerManager.getInstance().computerPlayerId);
                playerTowerc.setLocation(new Coordinate(Incursio.rand.Next(50, mapWidth - 50), Incursio.rand.Next(50, mapHeight - 50)));
            }

            //control points
            for (int i = 0; i < numPoints; i++)
            {
                ControlPoint playerPoint = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", PlayerManager.getInstance().currentPlayerId);
                playerPoint.setLocation(new Coordinate(Incursio.rand.Next(50, mapWidth - 50), Incursio.rand.Next(50, mapHeight - 100)));

                ControlPoint computerPoint = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", PlayerManager.getInstance().computerPlayerId);
                computerPoint.setLocation(new Coordinate(Incursio.rand.Next(50, mapWidth - 50), Incursio.rand.Next(50, mapHeight - 100)));
            }
        }
    }
}

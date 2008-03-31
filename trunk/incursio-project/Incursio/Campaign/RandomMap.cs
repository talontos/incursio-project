using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Campaign
{
    public class RandomMap : CampaignMap
    {
        public override void initializeMap()
        {
            //This map right now will represent our test environment

            EntityManager entityManager = EntityManager.getInstance();

            this.level = State.CampaignLevel.ONE;

            int mapWidth = 2048;
            int mapHeight = 1024;
            this.setMapDimensions(mapWidth, mapHeight, 1024, 768);

            //create camps in random locations
            CampStructure playerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", State.PlayerId.HUMAN);
            playerCamp.setLocation( new Coordinate(Incursio.rand.Next(mapWidth / 2), Incursio.rand.Next(mapHeight - 100)));

            CampStructure computerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", State.PlayerId.COMPUTER);
            computerCamp.setLocation( new Coordinate(Incursio.rand.Next(mapWidth / 2, mapWidth), Incursio.rand.Next(mapHeight - 100)));

            //place random units around the map
            int numLight = Incursio.rand.Next(10);
            int numArch = Incursio.rand.Next(10);
            int numTower = Incursio.rand.Next(10);
            
            //lights
            for(int i = 0; i < numLight; i++){
                LightInfantryUnit infUnitp = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", State.PlayerId.HUMAN);
                infUnitp.setLocation(new Coordinate(Incursio.rand.Next(mapWidth), Incursio.rand.Next(mapHeight)));
                LightInfantryUnit infUnitc = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", State.PlayerId.COMPUTER);
                infUnitc.setLocation(new Coordinate(Incursio.rand.Next(mapWidth), Incursio.rand.Next(mapHeight)));
            }

            //archers
            for (int i = 0; i < numArch; i++)
            {
                ArcherUnit archUnitp = (ArcherUnit)entityManager.createNewEntity("Incursio.Classes.ArcherUnit", State.PlayerId.HUMAN);
                archUnitp.setLocation(new Coordinate(Incursio.rand.Next(mapWidth), Incursio.rand.Next(mapHeight)));
                ArcherUnit archUnitc = (ArcherUnit)entityManager.createNewEntity("Incursio.Classes.ArcherUnit", State.PlayerId.COMPUTER);
                archUnitc.setLocation(new Coordinate(Incursio.rand.Next(mapWidth), Incursio.rand.Next(mapHeight)));
            }

            //towers
            for (int i = 0; i < numTower; i++)
            {
                GuardTowerStructure playerTowerp = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.HUMAN);
                playerTowerp.setLocation(new Coordinate(Incursio.rand.Next(50, mapWidth - 50), Incursio.rand.Next(50, mapHeight - 50)));

                GuardTowerStructure playerTowerc = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.COMPUTER);
                playerTowerc.setLocation(new Coordinate(Incursio.rand.Next(50, mapWidth - 50), Incursio.rand.Next(50, mapHeight - 50)));
            }
        }
    }
}

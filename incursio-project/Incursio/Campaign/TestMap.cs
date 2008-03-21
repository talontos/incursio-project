using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Campaign
{
    public class TestMap : CampaignMap
    {
        public override void initializeMap()
        {
            //TODO: CHANGE!
            //This map right now will represent our test environment

            EntityManager entityManager = EntityManager.getInstance();

            this.level = State.CampaignLevel.ONE;
            this.setMapDimensions(2048, 1024, 1024, 768);

            //testing unit creation/placement/moving///
            LightInfantryUnit infUnit1 = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", State.PlayerId.HUMAN);
            LightInfantryUnit infUnit2 = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", State.PlayerId.HUMAN);
            LightInfantryUnit infUnit3 = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", State.PlayerId.COMPUTER);

            ArcherUnit archUnit1 = (ArcherUnit)entityManager.createNewEntity("Incursio.Classes.ArcherUnit", State.PlayerId.HUMAN);
            ArcherUnit archUnit2 = (ArcherUnit)entityManager.createNewEntity("Incursio.Classes.ArcherUnit", State.PlayerId.COMPUTER);

            CampStructure playerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", State.PlayerId.HUMAN);
            CampStructure computerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", State.PlayerId.COMPUTER);

            GuardTowerStructure playerTower1 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.HUMAN);

            //infUnit1.setLocation(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));
            infUnit1.setLocation(new Coordinate(240, 380));
            infUnit2.setLocation(new Coordinate(200, 400));
            //archUnit2.setLocation(new Coordinate(820, 195));
            archUnit2.setLocation(new Coordinate(300, 200));
            infUnit3.setLocation(new Coordinate(800, 220)); //for ease of testing
            archUnit1.setLocation(new Coordinate(160, 395));
            playerCamp.setLocation(new Coordinate(100, 400));
            computerCamp.setLocation(new Coordinate(900, 200));
            playerTower1.setLocation(new Coordinate(150, 340));
            //infUnit3.setLocation(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));

            infUnit1.setHealth(80);
            infUnit2.setHealth(80);
            infUnit3.setHealth(50);
            archUnit1.setHealth(90);
            archUnit2.setHealth(90);
            playerCamp.setHealth(350);
            computerCamp.setHealth(350);
            playerTower1.setHealth(350);
        }
    }
}
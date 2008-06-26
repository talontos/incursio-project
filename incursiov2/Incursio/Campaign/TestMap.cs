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
using Incursio.Entities;

namespace Incursio.Campaign
{
    public class TestMap : CampaignMap
    {
        public TestMap()
            : base()
        {
            this.level = State.CampaignLevel.ONE;
            //this.setMapDimensions(2048, 1024, 1024, 768);
            this.setMapDimensions(2048, 2048, 1024, 768);
        }

        public override GameResult inspectWinConditions()
        {
            //return base.inspectWinConditions();

            //neverending game
            return new GameResult();
        }

        public override void initializeMap()
        {
            //This map right now will represent our test environment

            EntityManager entityManager = EntityManager.getInstance();

            base.initializeMap();

            //

            //let's make one of each...

            List<BaseGameEntityConfiguration> all = ObjectFactory.getInstance().entities;

            int x = 50, y = 50, player = PlayerManager.getInstance().currentPlayerId;

            for(int i = 0; i < all.Count; i++){
                BaseGameEntity e = EntityManager.getInstance().createNewEntity(i, player);
                e.setLocation(new Coordinate(x, y));

                x += 50;
                y += 50;
            }

            BaseGameEntity w = EntityManager.getInstance().createNewEntity(1, PlayerManager.getInstance().computerPlayerId);
            w.setLocation(new Coordinate(500, 500));
            


            /*
            //testing unit creation/placement/moving///
            LightInfantryUnit infUnit1 = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", PlayerManager.getInstance().currentPlayerId);
            LightInfantryUnit infUnit2 = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", PlayerManager.getInstance().currentPlayerId);
            LightInfantryUnit infUnit3 = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", PlayerManager.getInstance().computerPlayerId);

            HeavyInfantryUnit heavyUnit1 = (HeavyInfantryUnit)entityManager.createNewEntity("Incursio.Classes.HeavyInfantryUnit", PlayerManager.getInstance().currentPlayerId);
            HeavyInfantryUnit heavyUnit2 = (HeavyInfantryUnit)entityManager.createNewEntity("Incursio.Classes.HeavyInfantryUnit", PlayerManager.getInstance().computerPlayerId);

            ArcherUnit archUnit1 = (ArcherUnit)entityManager.createNewEntity("Incursio.Classes.ArcherUnit", PlayerManager.getInstance().currentPlayerId);
            //ArcherUnit archUnit2 = (ArcherUnit)entityManager.createNewEntity("Incursio.Classes.ArcherUnit", PlayerManager.getInstance().computerPlayerId);

            CampStructure playerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", PlayerManager.getInstance().currentPlayerId);
            CampStructure computerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", PlayerManager.getInstance().computerPlayerId);

            GuardTowerStructure playerTower1 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", PlayerManager.getInstance().currentPlayerId);

            ControlPoint point1 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", PlayerManager.getInstance().computerPlayerId);

            //infUnit1.setLocation(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));
            infUnit1.setLocation(new Coordinate(16,16));
            infUnit2.setLocation(new Coordinate(200, 400));
            heavyUnit1.setLocation(new Coordinate(220, 440));
            heavyUnit2.setLocation(new Coordinate(400, 440));
            //archUnit2.setLocation(new Coordinate(820, 195));
            //archUnit2.setLocation(new Coordinate(700, 200));
            infUnit3.setLocation(new Coordinate(800, 220)); //for ease of testing
            archUnit1.setLocation(new Coordinate(160, 395));
            playerCamp.setLocation(new Coordinate(100, 400));
            computerCamp.setLocation(new Coordinate(900, 200));
            point1.setLocation(new Coordinate(300, 600));
            playerTower1.setLocation(new Coordinate(150, 340));
            //infUnit3.setLocation(new Coordinate(rand.Next(0, 1024), rand.Next(0, 768)));

            heavyUnit2.setHealth(100);
            infUnit1.setHealth(80);
            infUnit2.setHealth(80);
            infUnit3.setHealth(200);
            archUnit1.setHealth(90);
            //archUnit2.setHealth(90);
            playerCamp.setHealth(350);
            computerCamp.setHealth(350);
            playerTower1.setHealth(350);
            
            */
        }
    }
}

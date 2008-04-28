using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;
using Incursio.Classes.Terrain;

namespace Incursio.Campaign
{
    public class Map1 : CampaignMap
    {
        public Map1()
            : base()
        {
            this.level = State.CampaignLevel.THREE;
            
            this.setMapDimensions(2048, 2048, 1024, 768);
        }

        public override void initializeMap()
        {
            //This map right now will represent our test environment

            EntityManager entityManager = EntityManager.getInstance();


            base.initializeMap();

            //testing unit creation/placement/moving///
            Hero playerHero = (Hero)entityManager.createNewEntity("Incursio.Classes.Hero", State.PlayerId.HUMAN);
            Hero compHero = (Hero)entityManager.createNewEntity("Incursio.Classes.Hero", State.PlayerId.COMPUTER);

            CampStructure playerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", State.PlayerId.HUMAN);
            CampStructure computerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", State.PlayerId.COMPUTER);

            //LightInfantryUnit infUnit = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", State.PlayerId.COMPUTER);
            HeavyInfantryUnit heavyUnit = (HeavyInfantryUnit)entityManager.createNewEntity("Incursio.Classes.HeavyInfantryUnit", State.PlayerId.HUMAN);

            ControlPoint cp1 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.COMPUTER);
            ControlPoint cp2 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.COMPUTER);
            ControlPoint cp3 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.HUMAN);
            ControlPoint cp4 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.HUMAN);

            GuardTowerStructure gth1 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.HUMAN);
            GuardTowerStructure gth2 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.HUMAN);
            GuardTowerStructure gth3 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.HUMAN);

            GuardTowerStructure gt1 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.COMPUTER);
            GuardTowerStructure gt2 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.COMPUTER);
            //GuardTowerStructure gt3 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.COMPUTER);
            GuardTowerStructure gt4 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.COMPUTER);
            GuardTowerStructure gt5 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.COMPUTER);
            GuardTowerStructure gt6 = (GuardTowerStructure)entityManager.createNewEntity("Incursio.Classes.GuardTowerStructure", State.PlayerId.COMPUTER);

            gth1.setLocation(new Coordinate(75, 700));
            gth2.setLocation(new Coordinate(700, 100));
            gth3.setLocation(new Coordinate(600, 500));

            gt1.setLocation(new Coordinate(900, 900));
            gt2.setLocation(new Coordinate(300, 1500));
            //gt3.setLocation(new Coordinate(1400, 400));
            gt4.setLocation(new Coordinate(1500, 1300));
            gt5.setLocation(new Coordinate(1300, 1600));
            gt6.setLocation(new Coordinate(1775, 1200));

            //infUnit.setLocation(new Coordinate(700, 100));
            heavyUnit.setLocation(new Coordinate(300, 100));
            playerHero.setLocation(new Coordinate(150, 500));
            compHero.setLocation(new Coordinate(1600, 1600));
            compHero.setHero_Badass();

            playerCamp.setLocation(new Coordinate(100, 400));
            computerCamp.setLocation(new Coordinate(1700, 1700));

            cp1.setLocation(new Coordinate(1000,1000));
            cp2.setLocation(new Coordinate(300, 1700));
            cp3.setLocation(new Coordinate(1700, 300));
            cp4.setLocation(new Coordinate(500, 400));

            playerCamp.setHealth(350);
            computerCamp.setHealth(350);
        }

        public override void loadTerrain()
        {
            //load grass
            base.loadTerrain();

            //load whatever for this map

            //um, trees
            /*
            Tree[] trees = {new Tree(20, 2),
                            new Tree(10, 50)};

            for(int i = 0; i < trees.Length; i++){
                trees[i].addToMap(this);
            }
             * */
        }
    }
}

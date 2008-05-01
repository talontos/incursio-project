using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;
using Incursio.Classes.Terrain;

namespace Incursio.Campaign
{
    public class Inland : CampaignMap
    {
        public Inland()
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
            
            Tree[] trees = {new Tree(16, 2),
                            new Tree(15, 3),
                            new Tree(10, 58),
                            new Tree(32, 34),
                            new Tree(43, 16),
                            new Tree(56, 59),
                            new Tree(08, 56),
            };

            //LETS TRY TO DRAW SOME WATER
            Water[] water = {new Water(0, 0, State.WaterType.OpenWater),
                new Water(0, 1, State.WaterType.OpenWater),
                new Water(0, 2, State.WaterType.OpenWater),
                new Water(0, 3, State.WaterType.OpenWater),
                new Water(0, 4, State.WaterType.OpenWater),
                new Water(0, 5, State.WaterType.OpenWater),
                new Water(1, 0, State.WaterType.OpenWater),
                new Water(1, 1, State.WaterType.OpenWater),
                new Water(1, 2, State.WaterType.OpenWater),
                new Water(1, 3, State.WaterType.OpenWater),
                new Water(1, 4, State.WaterType.OpenWater),
                new Water(2, 0, State.WaterType.OpenWater),
                new Water(2, 1, State.WaterType.OpenWater),
                new Water(2, 2, State.WaterType.OpenWater),
                new Water(2, 3, State.WaterType.OpenWater),
                new Water(3, 0, State.WaterType.OpenWater),
                new Water(3, 1, State.WaterType.OpenWater),
                new Water(0, 6, State.WaterType.ShoreDown),
                new Water(1, 6, State.WaterType.ShoreLowerRight),
                new Water(1, 5, State.WaterType.ShoreOpenUpperLeft),
                new Water(2, 5, State.WaterType.ShoreLowerRight),
                new Water(2, 4, State.WaterType.ShoreOpenUpperLeft),
                new Water(3, 4, State.WaterType.ShoreLowerRight),
                new Water(3, 3, State.WaterType.ShoreRight),
                new Water(3, 2, State.WaterType.ShoreOpenUpperLeft),
                new Water(4, 2, State.WaterType.ShoreLowerRight),
                new Water(4, 1, State.WaterType.ShoreRight),
                new Water(4, 0, State.WaterType.ShoreRight)

            };

            Tree[] trees1 = {new Tree(18, 40, true),
                             new Tree(14, 38, true),
                             new Tree(16, 39, true),
                             new Tree(12, 41, true),
                             new Tree(10, 42, true),
                             new Tree(04, 40, true),
                             new Tree(08, 43, true),
                             new Tree(06, 42, true),
                             new Tree(02, 41, true),
                             new Tree(00, 41, true),

                             new Tree(15, 54, true),
                             new Tree(16, 56, true),
                             new Tree(14, 57, true),
                             new Tree(13, 59, true),
                             new Tree(12, 60, true),

                             new Tree(44, 1, true),
                             new Tree(43, 2, true),
                             new Tree(45, 3, true),
                             new Tree(43, 5, true),
                             new Tree(41, 7, true),
                             new Tree(39, 8, true),

                             new Tree(62, 23, true),
                             new Tree(60, 24, true),
                             new Tree(58, 22, true),
                             new Tree(57, 21, true),
                             new Tree(55, 19, true),
                
                             new Tree(39, 58, true),
                             new Tree(41, 57, true),
                             new Tree(45, 57, true),
                             new Tree(43, 58, true),
                             new Tree(47, 58, true),
                             new Tree(46, 59, true),
                             new Tree(44, 59, true),
                             new Tree(49, 60, true),
                             
                             new Tree(62, 32, true),
                             new Tree(60, 33, true),
                             new Tree(58, 32, true),
                             new Tree(57, 34, true),
                             
                             
            };

            Rock[] rocks = {new Rock(2, 8, 0),
                            new Rock(34, 11, 2),
                            new Rock(52, 21, 1),
                            new Rock(34, 56, 2),
                            new Rock(4, 49, 1),
                            new Rock(57, 59, 2),


            };

            for(int i = 0; i < trees.Length; i++){
                trees[i].addToMap(this);
            }

            for(int i = 0; i < rocks.Length; i++){
                this.addObjectEntity(rocks[i]);
            }

            for(int i = 0; i < trees1.Length; i++){
                this.addObjectEntity(trees1[i]);
            }

            for (int i = 0; i < water.Length; i++)
            {
                water[i].addToMap(this);
            }

            this.addObjectEntity(new Building(34, 23, 0));
            this.addObjectEntity(new Building(38, 29, 1));
            this.addObjectEntity(new Building(40, 30, 1));
            this.addObjectEntity(new Building(32, 26, 2));
            this.addObjectEntity(new Building(28, 38, 0));
            this.addObjectEntity(new Building(30, 40, 1));
            this.addObjectEntity(new Building(34, 39, 2));
            this.addObjectEntity(new Building(26, 31, 0));
            this.addObjectEntity(new Building(24, 28, 2));
            
        }
    }
}

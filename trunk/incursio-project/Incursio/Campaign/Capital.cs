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
using Incursio.Classes.Terrain;
using Incursio.Classes;

namespace Incursio.Campaign
{
    class Capital : CampaignMap
    {

        public Capital()
            : base()
        {
            this.level = State.CampaignLevel.THREE;
            
            this.setMapDimensions(2048, 2240, 1024, 768);
            
        }

        public override void initializeMap()
        {
            EntityManager entityManager = EntityManager.getInstance();

            base.initializeMap();

            BaseGameEntity playerHero = entityManager.createNewEntity("Incursio.Classes.Hero", PlayerManager.getInstance().currentPlayerId);
            playerHero.setLocation(new Coordinate(120, 1760));

            BaseGameEntity compHero = entityManager.createNewEntity("Incursio.Classes.Hero", PlayerManager.getInstance().computerPlayerId);
            compHero.setLocation(new Coordinate(1900, 160));

            BaseGameEntity playerCamp = entityManager.createNewEntity("Incursio.Classes.CampStructure", PlayerManager.getInstance().currentPlayerId);
            playerCamp.setLocation(new Coordinate(40, 1600));
            playerCamp.setHealth(350);

            BaseGameEntity computerCamp = entityManager.createNewEntity("Incursio.Classes.CampStructure", PlayerManager.getInstance().computerPlayerId);
            computerCamp.setLocation(new Coordinate(1940, 95));
            computerCamp.setHealth(350);

            BaseGameEntity cp1 = entityManager.createNewEntity("Incursio.Classes.ControlPoint", PlayerManager.getInstance().currentPlayerId);
            cp1.setLocation(new Coordinate(230, 1650));

            BaseGameEntity cp2 = entityManager.createNewEntity("Incursio.Classes.ControlPoint", PlayerManager.getInstance().computerPlayerId);
            cp2.setLocation(new Coordinate(1810, 120));

            BaseGameEntity cp3 = entityManager.createNewEntity("Incursio.Classes.ControlPoint", PlayerManager.getInstance().computerPlayerId);
            cp3.setLocation(new Coordinate(430, 289));

            BaseGameEntity cp4 = entityManager.createNewEntity("Incursio.Classes.ControlPoint", PlayerManager.getInstance().computerPlayerId);
            cp4.setLocation(new Coordinate(1810, 1340));

            BaseGameEntity cp5 = entityManager.createNewEntity("Incursio.Classes.ControlPoint", PlayerManager.getInstance().computerPlayerId);
            cp5.setLocation(new Coordinate(1400, 1000));

            //default camera position
            this.minViewableX = 0;
            this.maxViewableX = 32;
            this.minViewableY = 38;
            this.maxViewableY = 70;
        }

        public override void loadTerrain()
        {
            base.loadTerrain();

            Grass grass;
            for (int j = 0; j < 70; j++)
            {
                for (int i = 0; i < 64; i++)
                {
                    grass = new Grass(i, j);
                    this.addMapEntity(grass, i, j);
                    //grass doesn't need to set occupancy
                }
            }

            Tree[] trees = {new Tree(1, 1, true),
                            new Tree(3, 1, true),
                            new Tree(0, 1, true),
                            new Tree(2, 3, true),
                            new Tree(1, 3, true),
                            new Tree(1, 5, true),
                            new Tree(0, 7, true),
                    
                            new Tree(2, 6),
                            new Tree(3, 13),
                            new Tree(1, 21),
                            new Tree(1, 32),
                            new Tree(2, 28),
                            
                
                            new Tree(1, 40, true),
                            new Tree(0, 41, true),
                            new Tree(2, 41, true),
                            new Tree(4, 42, true),

                            new Tree(20, 55, true),
                            new Tree(21, 57, true),
                            new Tree(21, 59, true),
                            new Tree(23, 58, true),
                            new Tree(25, 59, true),

                            new Tree(62, 56, true),
                            new Tree(61, 56, true),
                            new Tree(59, 57, true),
                            new Tree(58, 58, true),

                            new Tree(42, 59, true),
                            new Tree(40, 58, true),
                            new Tree(38, 57, true),
                            new Tree(36, 58, true),
                            new Tree(34, 59, true),

                            new Tree(33, 55),
                            new Tree(30, 53),

            };

            //oh
            //my
            //god
            Building[] buildings = {new Building(6, 2, 5), new Building(8, 2, 3), new Building(10, 2, 4),
                                    new Building(6, 5, 5), new Building(8, 5, 3), new Building(10, 5, 4),
                                    new Building(6, 8, 5), new Building(8, 8, 3), new Building(10, 8, 4),
                                    new Building(6, 11, 5), new Building(8, 11, 3), new Building(10, 11, 4),
                                    new Building(6, 14, 5), new Building(8, 14, 3), new Building(10, 14, 4),
                                    new Building(6, 17, 5), new Building(8, 17, 3), new Building(10, 17, 4),
                                    new Building(6, 20, 5), new Building(8, 20, 3), new Building(10, 20, 4),
                                    new Building(6, 23, 5), new Building(8, 23, 3), new Building(10, 23, 4),
                                    new Building(6, 26, 5), new Building(8, 26, 3), new Building(10, 26, 4),
                                    new Building(6, 29, 5), new Building(8, 29, 3), new Building(10, 29, 4),
                                    new Building(6, 32, 5), new Building(8, 32, 3), new Building(10, 32, 4),
                                    new Building(8, 35, 5), new Building(10, 35, 3), new Building(12, 35, 4),

                                    new Building(20, 46, 5), new Building(22, 46, 3), new Building(24, 46, 4),
                                    new Building(22, 48, 5), new Building(24, 48, 3),
                                                             new Building(27, 48, 3),
                                                             new Building(30, 48, 3),
                                                             new Building(42, 46, 3), new Building(44, 46, 4),
                                                             new Building(39, 46, 3),
                                                             new Building(36, 47, 3),
                                                             new Building(33, 48, 3),

                                    new Building(48, 48, 5), new Building(50, 48, 3),
                                                             new Building(53, 48, 3),
                                                             new Building(56, 49, 3),
                                                             new Building(59, 50, 3),
                                                             new Building(62, 50, 3),
                                    
                                                             new Building(48, 14, 3), new Building(50, 14, 4),
                                                             new Building(45, 13, 3),
                                                             new Building(42, 12, 3),
                                                             new Building(39, 12, 3),
                                    new Building(34, 12, 5), new Building(36, 12, 3),
                                                             new Building(43, 14, 3),
                                                             new Building(40, 14, 3),
                                                             new Building(37, 14, 3),
                                    new Building(32, 14, 5), new Building(34, 14, 3),
                                                             new Building(50, 16, 3),
                                                             new Building(47, 16, 3),
                                                             new Building(44, 16, 3),
                                                             new Building(41, 16, 3),
                                                             new Building(38, 16, 3),
                                                             new Building(35, 16, 3),
                                                             new Building(32, 17, 3),
                                    new Building(27, 17, 5), new Building(29, 17, 3),
                                    new Building(24, 18, 5), new Building(26, 18, 3),
                                    new Building(24, 19, 5), new Building(26, 19, 3), new Building(28, 19, 4),                                   
                                    new Building(22, 22, 5), new Building(24, 22, 3), new Building(26, 22, 4),
                                    new Building(24, 25, 5), new Building(26, 25, 3), new Building(28, 25, 4), 
                                    new Building(51, 18, 5), new Building(53, 18, 3), new Building(55, 18, 4),
                                    new Building(52, 21, 5), new Building(54, 21, 3), new Building(56, 21, 4),
                                    new Building(53, 24, 5), new Building(55, 24, 3), new Building(57, 24, 4),
                                    new Building(53, 27, 5), new Building(55, 27, 3), new Building(57, 27, 4),
                                    new Building(53, 30, 5), new Building(55, 30, 3), new Building(57, 30, 4),
                                    new Building(53, 33, 5), new Building(55, 33, 3), new Building(57, 33, 4),
                                                             new Building(53, 36, 3), new Building(55, 36, 4),
                                                             new Building(50, 36, 3), 
                                                             new Building(47, 36, 3), 
                                                             new Building(44, 36, 3), 
                                    new Building(32, 32, 5), new Building(34, 32, 3), new Building(36, 32, 4),
                                    new Building(34, 34, 5), new Building(36, 34, 3), new Building(38, 34, 4),
                                    new Building(36, 36, 5), new Building(41, 36, 3), 
                                                             new Building(38, 36, 3), 
                        
                                    new Building(39, 18, 0),
                                    new Building(44, 26, 2),
                                    new Building(46, 26, 1),
                                    new Building(37, 23, 2),
                                    new Building(35, 23, 0),
                                    new Building(33, 24, 0),

            };

            Rock[] rocks = {new Rock(2, 18, 2),
                            new Rock(1, 23, 1),
                            new Rock(3, 15, 0),
                            new Rock(1, 11, 0),
                            new Rock(31, 56, 2),
                            new Rock(58, 54, 2),
                            new Rock(56, 56, 0),

            };

            Water[] water = {new Water(46, 20, State.WaterType.OpenWater),
                             new Water(45, 20, State.WaterType.OpenWater),
                             new Water(44, 20, State.WaterType.OpenWater),
                             new Water(46, 21, State.WaterType.OpenWater),
                             new Water(46, 22, State.WaterType.OpenWater),
                             new Water(45, 21, State.WaterType.OpenWater),
                             new Water(45, 22, State.WaterType.OpenWater),
                             new Water(44, 21, State.WaterType.OpenWater),
                             new Water(44, 22, State.WaterType.OpenWater),
                             new Water(43, 21, State.WaterType.OpenWater),
                             new Water(43, 22, State.WaterType.OpenWater),
                             new Water(43, 19, State.WaterType.ShoreUpperLeft),
                             new Water(43, 20, State.WaterType.ShoreOpenLowerRight),
                             new Water(42, 20, State.WaterType.ShoreUpperLeft),
                             new Water(42, 21, State.WaterType.ShoreLeft),
                             new Water(42, 22, State.WaterType.ShoreLeft),
                             new Water(42, 23, State.WaterType.ShoreLowerLeft),
                             new Water(43, 23, State.WaterType.ShoreDown),
                             new Water(44, 23, State.WaterType.ShoreDown),
                             new Water(45, 23, State.WaterType.ShoreDown),
                             new Water(46, 23, State.WaterType.ShoreDown),
                             new Water(47, 23, State.WaterType.ShoreLowerRight),
                             new Water(47, 22, State.WaterType.ShoreRight),
                             new Water(47, 21, State.WaterType.ShoreRight),
                             new Water(47, 20, State.WaterType.ShoreRight),
                             new Water(47, 19, State.WaterType.ShoreUpperRight),
                             new Water(46, 19, State.WaterType.ShoreUp),
                             new Water(45, 19, State.WaterType.ShoreUp),
                             new Water(44, 19, State.WaterType.ShoreUp),

            };

            Road[] roads = {new Road(0, 51, State.RoadType.Horizontal),
                            new Road(1, 51, State.RoadType.Horizontal),
                            new Road(2, 51, State.RoadType.Horizontal),
                            new Road(3, 51, State.RoadType.Horizontal),
                            new Road(4, 51, State.RoadType.ElbowDownLeft),
                            new Road(4, 52, State.RoadType.Vertical),
                            new Road(4, 53, State.RoadType.Vertical),
                            new Road(4, 54, State.RoadType.ElbowUpRight),
                            new Road(5, 54, State.RoadType.Horizontal),
                            new Road(6, 54, State.RoadType.Horizontal),
                            new Road(7, 54, State.RoadType.Horizontal),
                            new Road(8, 54, State.RoadType.Horizontal),
                            new Road(9, 54, State.RoadType.Horizontal),
                            new Road(10, 54, State.RoadType.ElbowUpLeft),
                            new Road(10, 53, State.RoadType.Vertical),
                            new Road(10, 52, State.RoadType.Vertical),
                            new Road(10, 51, State.RoadType.Vertical),
                            new Road(10, 50, State.RoadType.Vertical),
                            new Road(10, 49, State.RoadType.Vertical),
                            new Road(10, 48, State.RoadType.Vertical),
                            new Road(10, 47, State.RoadType.Vertical),
                            new Road(10, 46, State.RoadType.Vertical),
                            new Road(10, 45, State.RoadType.Vertical),
                            new Road(10, 44, State.RoadType.ElbowDownRight),
                            new Road(11, 44, State.RoadType.Horizontal),
                            new Road(12, 44, State.RoadType.Horizontal),
                            new Road(13, 44, State.RoadType.Horizontal),
                            new Road(14, 44, State.RoadType.ElbowUpLeft),
                            new Road(14, 43, State.RoadType.Vertical),
                            new Road(14, 42, State.RoadType.Vertical),
                            new Road(14, 41, State.RoadType.Vertical),
                            new Road(14, 40, State.RoadType.ElbowDownRight),
                            new Road(15, 40, State.RoadType.Horizontal),
                            new Road(16, 40, State.RoadType.Horizontal),
                            new Road(17, 40, State.RoadType.ElbowUpLeft),
                            new Road(17, 39, State.RoadType.Vertical),
                            new Road(17, 38, State.RoadType.Vertical),
                            new Road(17, 37, State.RoadType.Vertical),
                            new Road(17, 36, State.RoadType.Vertical),
                            new Road(17, 35, State.RoadType.Vertical),
                            new Road(17, 34, State.RoadType.Vertical),
                            new Road(17, 33, State.RoadType.Vertical),
                            new Road(17, 32, State.RoadType.Vertical),
                            new Road(17, 31, State.RoadType.Vertical),
                            new Road(17, 30, State.RoadType.Vertical),
                            new Road(17, 29, State.RoadType.Vertical),
                            new Road(17, 28, State.RoadType.Vertical),
                            new Road(17, 27, State.RoadType.Vertical),
                            new Road(17, 26, State.RoadType.Vertical),
                            new Road(17, 25, State.RoadType.Vertical),
                            new Road(17, 24, State.RoadType.Vertical),
                            new Road(17, 23, State.RoadType.Vertical),
                            new Road(17, 22, State.RoadType.Vertical),
                            new Road(17, 21, State.RoadType.Vertical),
                            new Road(17, 20, State.RoadType.Vertical),
                            new Road(17, 19, State.RoadType.Vertical),
                            new Road(17, 18, State.RoadType.Vertical),
                            new Road(17, 17, State.RoadType.Vertical),
                            new Road(17, 16, State.RoadType.Vertical),
                            new Road(17, 15, State.RoadType.Vertical),
                            new Road(17, 14, State.RoadType.Vertical),
                            new Road(17, 13, State.RoadType.Vertical),
                            new Road(17, 12, State.RoadType.Vertical),
                            new Road(17, 11, State.RoadType.Vertical),
                            new Road(17, 10, State.RoadType.Vertical),
                            new Road(17, 09, State.RoadType.Vertical),
                            new Road(17, 08, State.RoadType.Vertical),
                            new Road(17, 07, State.RoadType.Vertical),
                            new Road(17, 06, State.RoadType.ElbowDownRight),
                            new Road(18, 06, State.RoadType.Horizontal),
                            new Road(19, 06, State.RoadType.Horizontal),
                            new Road(20, 06, State.RoadType.Horizontal),
                            new Road(21, 06, State.RoadType.Horizontal),
                            new Road(22, 06, State.RoadType.Horizontal),
                            new Road(23, 06, State.RoadType.Horizontal),
                            new Road(24, 06, State.RoadType.Horizontal),
                            new Road(25, 06, State.RoadType.Horizontal),
                            new Road(26, 06, State.RoadType.Horizontal),
                            new Road(27, 06, State.RoadType.Horizontal),
                            new Road(28, 06, State.RoadType.Horizontal),
                            new Road(29, 06, State.RoadType.Horizontal),
                            new Road(30, 06, State.RoadType.Horizontal),
                            new Road(31, 06, State.RoadType.Horizontal),
                            new Road(32, 06, State.RoadType.Horizontal),
                            new Road(33, 06, State.RoadType.Horizontal),
                            new Road(34, 06, State.RoadType.Horizontal),
                            new Road(35, 06, State.RoadType.Horizontal),
                            new Road(36, 06, State.RoadType.Horizontal),
                            new Road(37, 06, State.RoadType.Horizontal),
                            new Road(38, 06, State.RoadType.Horizontal),
                            new Road(39, 06, State.RoadType.Horizontal),
                            new Road(40, 06, State.RoadType.Horizontal),
                            new Road(41, 06, State.RoadType.Horizontal),
                            new Road(42, 06, State.RoadType.Horizontal),
                            new Road(43, 06, State.RoadType.Horizontal),
                            new Road(44, 06, State.RoadType.Horizontal),
                            new Road(45, 06, State.RoadType.Horizontal),
                            new Road(46, 06, State.RoadType.Horizontal),
                            new Road(47, 06, State.RoadType.Horizontal),
                            new Road(48, 06, State.RoadType.Horizontal),
                            new Road(49, 06, State.RoadType.Horizontal),
                            new Road(50, 06, State.RoadType.Horizontal),
                            new Road(51, 06, State.RoadType.Horizontal),
                            new Road(52, 06, State.RoadType.Horizontal),
                            new Road(53, 06, State.RoadType.Horizontal),
                            new Road(54, 06, State.RoadType.Horizontal),
                            new Road(55, 06, State.RoadType.Horizontal),
                            new Road(56, 06, State.RoadType.Horizontal),
                            new Road(57, 06, State.RoadType.Horizontal),
                            new Road(58, 06, State.RoadType.Horizontal),
                            new Road(59, 06, State.RoadType.Horizontal),
                            new Road(60, 06, State.RoadType.ElbowDownLeft),
                            new Road(60, 07, State.RoadType.Vertical),
                            new Road(60, 08, State.RoadType.Vertical),
                            new Road(60, 09, State.RoadType.Vertical),
                            new Road(60, 10, State.RoadType.Vertical),
                            new Road(60, 11, State.RoadType.Vertical),
                            new Road(60, 12, State.RoadType.Vertical),
                            new Road(60, 13, State.RoadType.Vertical),
                            new Road(60, 14, State.RoadType.Vertical),
                            new Road(60, 15, State.RoadType.Vertical),
                            new Road(60, 16, State.RoadType.Vertical),
                            new Road(60, 17, State.RoadType.Vertical),
                            new Road(60, 18, State.RoadType.Vertical),
                            new Road(60, 19, State.RoadType.Vertical),
                            new Road(60, 20, State.RoadType.Vertical),
                            new Road(60, 21, State.RoadType.Vertical),
                            new Road(60, 22, State.RoadType.Vertical),
                            new Road(60, 23, State.RoadType.Vertical),
                            new Road(60, 24, State.RoadType.Vertical),
                            new Road(60, 25, State.RoadType.Vertical),
                            new Road(60, 26, State.RoadType.Vertical),
                            new Road(60, 27, State.RoadType.Vertical),
                            new Road(60, 28, State.RoadType.Vertical),
                            new Road(60, 29, State.RoadType.Vertical),
                            new Road(60, 30, State.RoadType.Vertical),
                            new Road(60, 31, State.RoadType.Vertical),
                            new Road(60, 32, State.RoadType.Vertical),
                            new Road(60, 33, State.RoadType.Vertical),
                            new Road(60, 34, State.RoadType.Vertical),
                            new Road(60, 35, State.RoadType.Vertical),
                            new Road(60, 36, State.RoadType.Vertical),
                            new Road(60, 37, State.RoadType.Vertical),
                            new Road(60, 38, State.RoadType.Vertical),
                            new Road(60, 39, State.RoadType.Vertical),
                            new Road(60, 40, State.RoadType.Vertical),
                            new Road(60, 41, State.RoadType.Vertical),
                            new Road(60, 42, State.RoadType.Vertical),
                            new Road(60, 43, State.RoadType.ElbowUpLeft),
                            new Road(59, 43, State.RoadType.Horizontal),
                            new Road(58, 43, State.RoadType.Horizontal),
                            new Road(57, 43, State.RoadType.Horizontal),
                            new Road(56, 43, State.RoadType.Horizontal),
                            new Road(55, 43, State.RoadType.Horizontal),
                            new Road(54, 43, State.RoadType.Horizontal),
                            new Road(53, 43, State.RoadType.Horizontal),
                            new Road(52, 43, State.RoadType.Horizontal),
                            new Road(51, 43, State.RoadType.Horizontal),
                            new Road(50, 43, State.RoadType.Horizontal),
                            new Road(49, 43, State.RoadType.Horizontal),
                            new Road(48, 43, State.RoadType.Horizontal),
                            new Road(47, 43, State.RoadType.Horizontal),
                            new Road(46, 43, State.RoadType.ElbowDownRight),
                            new Road(46, 44, State.RoadType.Vertical),
                            new Road(46, 45, State.RoadType.Vertical),
                            new Road(46, 46, State.RoadType.Vertical),
                            new Road(46, 47, State.RoadType.Vertical),
                            new Road(46, 48, State.RoadType.Vertical),
                            new Road(46, 49, State.RoadType.Vertical),
                            new Road(46, 50, State.RoadType.Vertical),
                            new Road(46, 51, State.RoadType.Vertical),
                            new Road(46, 52, State.RoadType.Vertical),
                            new Road(46, 53, State.RoadType.Vertical),
                            new Road(46, 54, State.RoadType.Vertical),
                            new Road(46, 55, State.RoadType.Vertical),
                            new Road(46, 56, State.RoadType.Vertical),
                            new Road(46, 57, State.RoadType.Vertical),
                            new Road(46, 58, State.RoadType.Vertical),

            };

            //add trees
            for (int i = 0; i < trees.Length; i++)
            {
                this.addObjectEntity(trees[i]);
            }

            //add buildings
            for (int i = 0; i < buildings.Length; i++)
            {
                this.addObjectEntity(buildings[i]);
            }

            //add water
            for (int i = 0; i < water.Length; i++)
            {
                this.addMapEntity(water[i], water[i].location.x, water[i].location.y);
            }

            //add water
            for (int i = 0; i < roads.Length; i++)
            {
                this.addMapEntity(roads[i], roads[i].location.x, roads[i].location.y);
            }

            //add rocks
            for (int i = 0; i < rocks.Length; i++)
            {
                this.addObjectEntity(rocks[i]);
            }
        }

    }
}

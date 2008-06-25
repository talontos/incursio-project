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
using Incursio.World.Terrain;
using Incursio.Entities;

namespace Incursio.Campaign
{
    class Port : CampaignMap
    {
        public Port()
            : base()
        {
            this.level = State.CampaignLevel.ONE;
            
            this.setMapDimensions(2048, 4096, 1024, 768);
            
        }

        public override void initializeMap()
        {
            EntityManager entityManager = EntityManager.getInstance();

            base.initializeMap();

            BaseGameEntity playerHero = entityManager.createNewEntity(heroId, PlayerManager.getInstance().currentPlayerId);
            playerHero.setLocation(new Coordinate(1750, 3250));

            BaseGameEntity compHero = entityManager.createNewEntity(heroId, PlayerManager.getInstance().computerPlayerId);
            compHero.setLocation(new Coordinate(300, 100));

            BaseGameEntity playerCamp = entityManager.createNewEntity(campId, PlayerManager.getInstance().currentPlayerId);
            playerCamp.setLocation(new Coordinate(1810, 3250));
            playerCamp.setHealth(350);

            BaseGameEntity computerCamp = entityManager.createNewEntity(campId, PlayerManager.getInstance().computerPlayerId);
            computerCamp.setLocation(new Coordinate(230, 120));
            computerCamp.setHealth(350);

            BaseGameEntity cp1 = entityManager.createNewEntity(cpId, PlayerManager.getInstance().computerPlayerId);
            cp1.setLocation(new Coordinate(400, 200));

            BaseGameEntity cp2 = entityManager.createNewEntity(cpId, PlayerManager.getInstance().currentPlayerId);
            cp2.setLocation(new Coordinate(1600, 3350));

            BaseGameEntity cp3 = entityManager.createNewEntity(cpId, PlayerManager.getInstance().computerPlayerId);
            cp3.setLocation(new Coordinate(1250, 2300));

            BaseGameEntity cp4 = entityManager.createNewEntity(cpId, PlayerManager.getInstance().computerPlayerId);
            cp4.setLocation(new Coordinate(300, 3200));

            BaseGameEntity cp5 = entityManager.createNewEntity(cpId, PlayerManager.getInstance().computerPlayerId);
            cp5.setLocation(new Coordinate(650, 1020));

            BaseGameEntity cp6 = entityManager.createNewEntity(cpId, PlayerManager.getInstance().computerPlayerId);
            cp6.setLocation(new Coordinate(1400, 120));
            
            
            //set default camera position
            this.minViewableX = 32;
            this.maxViewableX = 64;
            this.minViewableY = 96;
            this.maxViewableY = 128;
        }

        public override void loadTerrain()
        {
            //Cover map with grass
            Grass grass;
            for (int j = 0; j < 108; j++)
            {
                for (int i = 0; i < 64; i++)
                {
                    grass = new Grass(i, j);
                    this.addMapEntity(grass, i, j);
                    //grass doesn't need to set occupancy
                }
            }

            Water water;
            for (int i = 0; i < 64; i++)
            {
                water = new Water(i, 107, State.WaterType.ShoreUp);
                this.addMapEntity(water, i, 107);
            }



            //oh
            //my
            //god
            Water[] water2 = {new Water(0, 107, State.WaterType.OpenWater), new Water(0, 106, State.WaterType.ShoreUp), 
                             new Water(1, 107, State.WaterType.OpenWater), new Water(1, 106, State.WaterType.ShoreUp),
                             new Water(2, 107, State.WaterType.OpenWater), new Water(2, 106, State.WaterType.ShoreUp),
                                                                           new Water(3, 106, State.WaterType.ShoreUpperRight),
                                                                           new Water(3, 107, State.WaterType.ShoreOpenLowerLeft),
                             new Water(10, 107, State.WaterType.ShoreOpenLowerRight), new Water(10, 106, State.WaterType.ShoreUpperLeft), 
                             new Water(11, 107, State.WaterType.OpenWater), new Water(11, 106, State.WaterType.ShoreUp),
                             new Water(12, 107, State.WaterType.OpenWater), new Water(12, 106, State.WaterType.ShoreOpenLowerRight),
                             new Water(13, 107, State.WaterType.OpenWater), new Water(12, 105, State.WaterType.ShoreUpperLeft),
                             new Water(14, 107, State.WaterType.OpenWater),
                             new Water(15, 107, State.WaterType.OpenWater),
                             new Water(16, 107, State.WaterType.OpenWater),
                             new Water(13, 106, State.WaterType.OpenWater), new Water(13, 105, State.WaterType.ShoreOpenLowerRight),
                             new Water(14, 106, State.WaterType.OpenWater), new Water(13, 104, State.WaterType.ShoreUpperLeft),
                             new Water(14, 105, State.WaterType.OpenWater), new Water(14, 104, State.WaterType.ShoreUp),
                             new Water(15, 106, State.WaterType.ShoreOpenLowerLeft), new Water(15, 104, State.WaterType.ShoreUpperRight),
                             new Water(15, 107, State.WaterType.OpenWater), new Water(16, 106, State.WaterType.ShoreUpperRight),
                             new Water(15, 105, State.WaterType.ShoreRight), new Water(16, 107, State.WaterType.ShoreOpenLowerLeft),
                             new Water(30, 107, State.WaterType.ShoreOpenLowerRight), new Water(30, 106, State.WaterType.ShoreUpperLeft),
                             new Water(31, 107, State.WaterType.OpenWater), new Water(31, 106, State.WaterType.ShoreUp),
                             new Water(32, 107, State.WaterType.OpenWater), new Water(32, 106, State.WaterType.ShoreOpenLowerRight),
                             new Water(33, 107, State.WaterType.OpenWater), new Water(32, 105, State.WaterType.ShoreUpperLeft),
                             new Water(34, 107, State.WaterType.OpenWater), new Water(33, 105, State.WaterType.ShoreUp),
                             new Water(35, 107, State.WaterType.OpenWater), new Water(34, 105, State.WaterType.ShoreUp),
                             new Water(36, 107, State.WaterType.OpenWater), new Water(35, 105, State.WaterType.ShoreUp),
                             new Water(37, 107, State.WaterType.OpenWater), new Water(36, 105, State.WaterType.ShoreUpperRight),
                             new Water(38, 107, State.WaterType.ShoreUp), new Water(36, 106, State.WaterType.ShoreOpenLowerLeft),
                             new Water(39, 107, State.WaterType.ShoreUp), new Water(37, 106, State.WaterType.ShoreUpperRight),
                             new Water(40, 107, State.WaterType.ShoreUp), new Water(37, 107, State.WaterType.ShoreOpenLowerLeft),
                             new Water(33, 106, State.WaterType.OpenWater), 
                             new Water(34, 106, State.WaterType.OpenWater), 
                             new Water(35, 106, State.WaterType.OpenWater),
                             new Water(36, 107, State.WaterType.OpenWater),
                             new Water(45, 107, State.WaterType.OpenWater), new Water(45, 107, State.WaterType.ShoreOpenLowerRight),
                             new Water(46, 107, State.WaterType.OpenWater), new Water(45, 106, State.WaterType.ShoreUpperLeft),
                             new Water(47, 107, State.WaterType.OpenWater), new Water(46, 106, State.WaterType.ShoreUp),
                             new Water(48, 107, State.WaterType.OpenWater),
                             new Water(47, 106, State.WaterType.ShoreUp), new Water(48, 106, State.WaterType.ShoreUp),
                             new Water(53, 107, State.WaterType.ShoreOpenLowerRight), new Water(45, 106, State.WaterType.ShoreUpperLeft),
                             new Water(54, 107, State.WaterType.OpenWater), new Water(49, 106, State.WaterType.ShoreUpperRight),
                             new Water(55, 107, State.WaterType.OpenWater), new Water(49, 107, State.WaterType.ShoreOpenLowerLeft),
                             new Water(56, 107, State.WaterType.OpenWater), new Water(53, 106, State.WaterType.ShoreUpperLeft),
                             new Water(57, 107, State.WaterType.OpenWater), new Water(54, 106, State.WaterType.ShoreUp),
                             new Water(58, 107, State.WaterType.OpenWater), 
                             new Water(59, 107, State.WaterType.OpenWater), new Water(55, 105, State.WaterType.ShoreUpperLeft),
                             new Water(60, 107, State.WaterType.OpenWater), new Water(56, 105, State.WaterType.ShoreUp),
                             new Water(61, 107, State.WaterType.OpenWater), new Water(57, 105, State.WaterType.ShoreUp),
                             new Water(62, 107, State.WaterType.OpenWater), 
                             new Water(63, 107, State.WaterType.OpenWater), new Water(58, 104, State.WaterType.ShoreUpperLeft),
                             new Water(55, 106, State.WaterType.ShoreOpenLowerRight), new Water(59, 104, State.WaterType.ShoreUp),
                             new Water(56, 106, State.WaterType.OpenWater), new Water(60, 104, State.WaterType.ShoreUp),
                             new Water(57, 106, State.WaterType.OpenWater), new Water(61, 104, State.WaterType.ShoreUp),
                             new Water(58, 106, State.WaterType.OpenWater), 
                             new Water(59, 106, State.WaterType.OpenWater), 
                             new Water(60, 106, State.WaterType.OpenWater), new Water(63, 103, State.WaterType.ShoreUpperLeft),
                             new Water(61, 106, State.WaterType.OpenWater), 
                             new Water(62, 106, State.WaterType.OpenWater),
                             new Water(63, 106, State.WaterType.OpenWater),
                             new Water(58, 105, State.WaterType.ShoreOpenLowerRight),
                             new Water(59, 105, State.WaterType.OpenWater),
                             new Water(60, 105, State.WaterType.OpenWater),   
                             new Water(61, 105, State.WaterType.OpenWater),
                             new Water(62, 105, State.WaterType.OpenWater),
                             new Water(63, 105, State.WaterType.OpenWater),
                             new Water(62, 104, State.WaterType.ShoreUp),
                             new Water(63, 104, State.WaterType.ShoreOpenLowerRight),


            };


            Tree[] trees = {new Tree(63, 102),
                            new Tree(62, 103),
                            new Tree(62, 101),
                            new Tree(46, 89),
                            new Tree(47, 87),
                            new Tree(45, 53),
                            new Tree(15, 49),
                            new Tree(16, 48),
                            new Tree(20, 36),
                            new Tree(1, 5),
                            new Tree(35, 20),
                            new Tree(38, 25),
                            new Tree(63, 9),
                            new Tree(4, 90),
                            new Tree(7, 85),
                            new Tree(2, 9),
                            new Tree(59, 20),
                            new Tree(24, 43),
                            new Tree(25, 44),
                            new Tree(22, 40),
                            new Tree(26, 39),
                            new Tree(23, 41),
                            new Tree(3, 60),
                            new Tree(4, 59),
                            new Tree(2, 59),
                            new Tree(3, 56),
                            new Tree(60, 80, true),
                            new Tree(36, 89, true),
                            new Tree(34, 90, true),
                            new Tree(32, 89, true),
                            new Tree(31, 91, true),
                            new Tree(30, 93, true),
                            new Tree(31, 94, true),
                            new Tree(62, 88, true),
                            new Tree(58, 87, true),
                            new Tree(5, 23, true),
                            new Tree(7, 22, true),
                            new Tree(9, 24, true),
                            new Tree(4, 22, true),
                            new Tree(2, 21, true),
                            new Tree(1, 22, true),
                            new Tree(21, 40, true),
                            new Tree(19, 42, true),
                            new Tree(23, 43, true),
                            new Tree(25, 42, true),
                            new Tree(24, 44, true),
                            



            };

            Road[] roads = {new Road(56, 101, State.RoadType.Vertical),
                             new Road(56, 102, State.RoadType.ElbowUpLeft),
                             new Road(55, 102, State.RoadType.Horizontal),
                             new Road(54, 102, State.RoadType.Horizontal),
                             new Road(53, 102, State.RoadType.Horizontal),
                             new Road(52, 102, State.RoadType.Horizontal),
                             new Road(51, 102, State.RoadType.Horizontal),
                             new Road(50, 102, State.RoadType.ElbowDownRight),
                             
                             new Road(10, 102, State.RoadType.ElbowDownRight),
                             new Road(11, 102, State.RoadType.Horizontal),
                             new Road(12, 102, State.RoadType.Horizontal),
                             new Road(13, 102, State.RoadType.Horizontal),
                             new Road(14, 102, State.RoadType.ElbowUpLeft),
                             new Road(14, 101, State.RoadType.Vertical),
                             new Road(14, 100, State.RoadType.Vertical),

                             new Road(3, 0, State.RoadType.Vertical),
                             new Road(3, 1, State.RoadType.Vertical),
                             new Road(3, 2, State.RoadType.Vertical),
                             new Road(3, 3, State.RoadType.Vertical),
                             new Road(3, 4, State.RoadType.Vertical),
                             new Road(3, 5, State.RoadType.ElbowUpRight),
                             new Road(4, 5, State.RoadType.Horizontal),
                             new Road(5, 5, State.RoadType.Horizontal),
                             new Road(6, 5, State.RoadType.Horizontal),
                             new Road(7, 5, State.RoadType.ElbowDownLeft),
                             new Road(7, 6, State.RoadType.Vertical),
                             new Road(7, 7, State.RoadType.Vertical),

            };

            for(int i = 0; i < water2.Length; i++){
                this.addMapEntity(water2[i], water2[i].location.x, water2[i].location.y);
            }


            for (int j = 108; j < 128; j++)
            {
                for (int i = 0; i < 64; i++)
                {
                    water = new Water(i, j, State.WaterType.OpenWater);
                    this.addMapEntity(water, i, j);
                }
            }

            for(int i = 0; i < trees.Length; i++){
                this.addObjectEntity(trees[i]);
            }

            for(int i = 0; i < roads.Length; i++){
                this.addMapEntity(roads[i], roads[i].location.x, roads[i].location.y);
            }

            this.addObjectEntity(new BaseMapEntity(TextureBank.getInstance().terrain.terrain.dock.texture, false, 23, 109));
            this.addObjectEntity(new BaseMapEntity(TextureBank.getInstance().terrain.terrain.dock.texture, false, 26, 109));

            this.addObjectEntity(new Building(22, 105, 0));
            this.addObjectEntity(new Building(25, 105, 1));
            this.addObjectEntity(new Building(27, 106, 2));
            this.addObjectEntity(new Building(4, 13, 0));
            this.addObjectEntity(new Building(56, 8, 1));
            this.addObjectEntity(new Building(60, 11, 2));
            this.addObjectEntity(new Building(43, 75, 1));

            this.addObjectEntity(new Rock(50, 85, 0));
            this.addObjectEntity(new Rock(52, 86, 1));
            this.addObjectEntity(new Rock(48, 86, 2));
            this.addObjectEntity(new Rock(5, 102, 0));
            this.addObjectEntity(new Rock(18, 89, 1));
            this.addObjectEntity(new Rock(23, 5, 2));
            this.addObjectEntity(new Rock(59, 13, 1));
            this.addObjectEntity(new Rock(38, 20, 2));

        }
    }
}

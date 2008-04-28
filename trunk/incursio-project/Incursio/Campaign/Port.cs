using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;
using Incursio.Classes.Terrain;

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

            Hero playerHero = (Hero)entityManager.createNewEntity("Incursio.Classes.Hero", State.PlayerId.HUMAN);
            playerHero.setLocation(new Coordinate(1750, 3600));

            Hero compHero = (Hero)entityManager.createNewEntity("Incursio.Classes.Hero", State.PlayerId.COMPUTER);
            compHero.setLocation(new Coordinate(300, 100));

            CampStructure playerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", State.PlayerId.HUMAN);
            playerCamp.setLocation(new Coordinate(1810, 3600));
            playerCamp.setHealth(350);

            CampStructure computerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", State.PlayerId.COMPUTER);
            computerCamp.setLocation(new Coordinate(230, 120));
            computerCamp.setHealth(350);

            ControlPoint cp1 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.COMPUTER);
            cp1.setLocation(new Coordinate(400, 200));

            ControlPoint cp2 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.HUMAN);
            cp2.setLocation(new Coordinate(1600, 3450));


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
                water = new Water(i, 108, State.WaterType.ShoreUp);
                this.addMapEntity(water, i, 108);
            }

            for (int j = 109; j < 128; j++)
            {
                for (int i = 0; i < 64; i++)
                {
                    water = new Water(i, j, State.WaterType.OpenWater);
                    this.addMapEntity(water, i, j);
                }
            }
        }
    }
}

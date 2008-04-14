using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Campaign
{
    public class Map1 : CampaignMap
    {
        public Map1()
            : base()
        {
            this.level = State.CampaignLevel.THREE;
            //this.setMapDimensions(2048, 1024, 1024, 768);
            this.setMapDimensions(2048, 2048, 1024, 768);
        }

        public override void initializeMap()
        {
            //This map right now will represent our test environment

            EntityManager entityManager = EntityManager.getInstance();


            base.initializeMap();

            //testing unit creation/placement/moving///

            CampStructure playerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", State.PlayerId.HUMAN);
            CampStructure computerCamp = (CampStructure)entityManager.createNewEntity("Incursio.Classes.CampStructure", State.PlayerId.COMPUTER);

            LightInfantryUnit infUnit = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", State.PlayerId.COMPUTER);
            HeavyInfantryUnit heavyUnit = (HeavyInfantryUnit)entityManager.createNewEntity("Incursio.Classes.HeavyInfantryUnit", State.PlayerId.HUMAN);

            ControlPoint cp1 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.COMPUTER);
            ControlPoint cp2 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.COMPUTER);
            ControlPoint cp3 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.COMPUTER);
            ControlPoint cp4 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.HUMAN);


            infUnit.setLocation(new Coordinate(700, 100));
            heavyUnit.setLocation(new Coordinate(300, 100));
            playerCamp.setLocation(new Coordinate(100, 400));
            computerCamp.setLocation(new Coordinate(1700, 1700));

            cp1.setLocation(new Coordinate(1000,1000));
            cp2.setLocation(new Coordinate(300, 1700));
            cp3.setLocation(new Coordinate(1700, 300));
            cp4.setLocation(new Coordinate(500, 400));

            playerCamp.setHealth(350);
            computerCamp.setHealth(350);
        }
    }
}

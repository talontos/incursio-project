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
            this.level = State.CampaignLevel.ONE;
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

            ControlPoint cp1 = (ControlPoint)entityManager.createNewEntity("Incursio.Classes.ControlPoint", State.PlayerId.COMPUTER);

            playerCamp.setLocation(new Coordinate(100, 400));
            computerCamp.setLocation(new Coordinate(1700, 1700));

            cp1.setLocation(new Coordinate(1000,1000));

            playerCamp.setHealth(350);
            computerCamp.setHealth(350);
        }
    }
}
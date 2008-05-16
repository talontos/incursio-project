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

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;
using Incursio.Classes.Terrain;

namespace Incursio.Campaign
{
    class CreditsMap : CampaignMap
    {
        private int dirCounter = 0;
        private State.Direction curDir = State.Direction.West;

        private const string CREDITS = "";

        public CreditsMap()
            : base()
        {
            this.level = State.CampaignLevel.CREDITS;
            
            this.setMapDimensions(1024, 768, 1024, 768);
        }

        /// <summary>
        /// This serves as the map's update function.
        /// we will draw all credit/ect related things here
        /// </summary>
        /// <returns></returns>
        public override GameResult inspectWinConditions()
        {

            //TODO: Draw credits


            //update dancing light infantry
            if(dirCounter >= 60){
                curDir = curDir == State.Direction.West ? State.Direction.East : State.Direction.West;
                EntityManager.getInstance().getLivePlayerUnits(PlayerManager.getInstance().currentPlayerId).ForEach(delegate(BaseGameEntity e)
                {
                    (e as Unit).setDirectionState(curDir);
                });

                curDir = curDir == State.Direction.West ? State.Direction.East : State.Direction.West;
                EntityManager.getInstance().getLivePlayerUnits(PlayerManager.getInstance().computerPlayerId).ForEach(delegate(BaseGameEntity e)
                {
                    (e as Unit).setDirectionState(curDir);
                });

                curDir = curDir == State.Direction.West ? State.Direction.East : State.Direction.West;

                dirCounter = 0;
            }
            else{
                dirCounter++;
            }
            return new GameResult();
        }

        public override void initializeMap()
        {
            //This map right now will represent our test environment

            EntityManager entityManager = EntityManager.getInstance();

            base.initializeMap();

            //add light infantry
            LightInfantryUnit l;
            Coordinate location = new Coordinate();
            for(int i = 0; i < this.width; i++){
                l = (LightInfantryUnit) entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", PlayerManager.getInstance().currentPlayerId);
                translateMapCellToPixel(i, 17, out location.x, out location.y);
                l.setLocation(new Coordinate(location.x, location.y));
                l.setCurrentState(State.EntityState.Attacking);

                l = (LightInfantryUnit)entityManager.createNewEntity("Incursio.Classes.LightInfantryUnit", PlayerManager.getInstance().computerPlayerId);
                translateMapCellToPixel(i, 2, out location.x, out location.y);
                l.setLocation(new Coordinate(location.x, location.y));
                l.setCurrentState(State.EntityState.Attacking);
            }
        }
    }
}

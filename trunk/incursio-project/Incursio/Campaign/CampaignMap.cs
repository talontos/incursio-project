using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Campaign
{
    public class CampaignMap : MapBase
    {
        public State.CampaignLevel level;

        public override void initializeMap(/*PASS A FILE IN DEFINING MAP??*/)
        {
            base.initializeMap();
        }

        /// <summary>
        /// This function should be overridden by separate levels to check
        /// win conditions.
        /// </summary>
        /// <returns>Game state</returns>
        public override State.GameState inspectWinConditions(){
            EntityManager eMan = EntityManager.getInstance();

            if (eMan.getLivePlayerEntities(State.PlayerId.HUMAN).Count == 0         //No live entities
                //|| eMan.getLivePlayerHeros(State.PlayerId.HUMAN).Count == 0         //No live heros
               )
            {
                return State.GameState.Defeat;
            }

            if (   eMan.getLivePlayerEntities(State.PlayerId.COMPUTER).Count == 0      //No live entities
                //|| eMan.getLivePlayerHeros(State.PlayerId.COMPUTER).Count == 0         //No live heros
               )
            {
                return State.GameState.Victory;
            }

            //...

            return State.GameState.None;
        }

    }
}

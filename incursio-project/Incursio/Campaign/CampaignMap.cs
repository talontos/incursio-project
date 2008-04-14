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
        private bool noControlPoints = false;
        private bool noCamp = false;

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

            //if no control points are held, or the camp is destroyed
            if (eMan.getPlayerControlPoints(State.PlayerId.HUMAN) == 0)               //If no control points are held
            {
                return State.GameState.Defeat;
            }

            if (eMan.getPlayerControlPoints(State.PlayerId.COMPUTER) == 0)            //If no control points are held
            {
                return State.GameState.Victory;
            }

            if (eMan.isPlayerCampDestroyed(State.PlayerId.HUMAN))                     //If camp is destroyed
            {
                return State.GameState.Defeat;
            }

            if (eMan.isPlayerCampDestroyed(State.PlayerId.COMPUTER))                 //If camp is destroyed
            {
                return State.GameState.Victory;
            }

            //...

            return State.GameState.None;
        }

    }
}

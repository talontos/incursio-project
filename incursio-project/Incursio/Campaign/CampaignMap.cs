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
        public override GameResult inspectWinConditions(){
            EntityManager eMan = EntityManager.getInstance();
            GameResult result = new GameResult();

            //TODO: We can remove this one; others will override it
            if (eMan.getLivePlayerEntities(State.PlayerId.HUMAN).Count == 0)         //No live entities
            {
                result = new GameResult(State.GameState.Defeat, "Your Entire Army has Fallen!");
            }

            if (eMan.getLivePlayerHeros(State.PlayerId.HUMAN).Count == 0)         //No live heros
            {
                result = new GameResult(State.GameState.Defeat, "Your Hero has Fallen!");
            }

            //TODO: We can remove this one; others will override it
            if (eMan.getLivePlayerEntities(State.PlayerId.COMPUTER).Count == 0)         //No live entities
            {
                result = new GameResult(State.GameState.Defeat, "Your Enemy's Entire Army has Fallen!");
            }

            /*if (eMan.getLivePlayerHeros(State.PlayerId.COMPUTER).Count == 0)         //No live heros
            {
                result = new GameResult(State.GameState.Defeat, "Your Enemy's Hero has Fallen!");
            }*/

            //if no control points are held, or the camp is destroyed
            if (eMan.getPlayerTotalOwnedControlPoints(State.PlayerId.HUMAN) == 0)               //If no control points are held
            {
                result = new GameResult(State.GameState.Defeat, "You have Lost all Control Points!");
            }

            if (eMan.getPlayerTotalOwnedControlPoints(State.PlayerId.COMPUTER) == 0)            //If no control points are held
            {
                result = new GameResult(State.GameState.Victory, "You have Conquered all Control Points!");
            }

            if (eMan.isPlayerCampDestroyed(State.PlayerId.HUMAN))                     //If camp is destroyed
            {
                result = new GameResult(State.GameState.Defeat, "Your Camp has been Destroyed!");
            }

            if (eMan.isPlayerCampDestroyed(State.PlayerId.COMPUTER))                 //If camp is destroyed
            {
                result = new GameResult(State.GameState.Victory, "Your Enemy's Camp has been Destroyed!");
            }

            //...

            return result;
        }

    }
}

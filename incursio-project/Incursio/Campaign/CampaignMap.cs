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

        private int game_over_timer = 0;
        private GameResult gameOverResult = null;
        private bool messageSent = false;

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

            if (this.gameOverResult != null)
            {
                if( !this.messageSent){
                    MessageManager.getInstance().addMessage(
                        new GameEvent(State.EventType.GAME_OVER_MAN, null, "",/*get sound for two things*/ gameOverResult.result, new Coordinate(0,0))
                    );

                    messageSent = true;
                }
                //game is over; countdown to end-screen
                if(this.game_over_timer >= 250){
                    return this.gameOverResult;
                }
                else{
                    game_over_timer++;
                }
            }
            else{

                EntityManager eMan = EntityManager.getInstance();
                //GameResult result = new GameResult();

                //TODO: We can remove this one; others will override it
                if (eMan.getLivePlayerEntities(State.PlayerId.HUMAN).Count == 0)         //No live entities
                {
                    this.gameOverResult = new GameResult(State.GameState.Defeat, "Your Entire Army has Fallen!");
                }

                if (eMan.getLivePlayerHeros(State.PlayerId.HUMAN).Count == 0)         //No live heros
                {
                    this.gameOverResult = new GameResult(State.GameState.Defeat, "Your Hero has Fallen!");
                }

                //TODO: We can remove this one; others will override it
                if (eMan.getLivePlayerEntities(State.PlayerId.COMPUTER).Count == 0)         //No live entities
                {
                    this.gameOverResult = new GameResult(State.GameState.Victory, "Your Enemy's Entire Army has Fallen!");
                }

                if (eMan.getLivePlayerHeros(State.PlayerId.COMPUTER).Count == 0)         //No live heros
                {
                    this.gameOverResult = new GameResult(State.GameState.Victory, "Your Enemy's Hero has Fallen!");
                }

                //if no control points are held, or the camp is destroyed
                if (eMan.getPlayerTotalOwnedControlPoints(State.PlayerId.HUMAN) == 0)               //If no control points are held
                {
                    this.gameOverResult = new GameResult(State.GameState.Defeat, "You have Lost all Control Points!");
                }

                if (eMan.getPlayerTotalOwnedControlPoints(State.PlayerId.COMPUTER) == 0)            //If no control points are held
                {
                    this.gameOverResult = new GameResult(State.GameState.Victory, "You have Conquered all Control Points!");
                }

                if (eMan.isPlayerCampDestroyed(State.PlayerId.HUMAN))                     //If camp is destroyed
                {
                    this.gameOverResult = new GameResult(State.GameState.Defeat, "Your Camp has been Destroyed!");
                }

                if (eMan.isPlayerCampDestroyed(State.PlayerId.COMPUTER))                 //If camp is destroyed
                {
                    this.gameOverResult = new GameResult(State.GameState.Victory, "Your Enemy's Camp has been Destroyed!");
                }

                //...
            }
                
            return new GameResult();
        }

    }
}

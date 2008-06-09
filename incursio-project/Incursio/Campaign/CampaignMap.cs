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

namespace Incursio.Campaign
{
    public class CampaignMap : MapBase
    {
        public State.CampaignLevel level;

        private int game_over_timer = 0;
        private GameResult gameOverResult = null;
        private bool messageSent = false;

        protected int heroId = 0, campId = 0, cpId = 0;

        public override void initializeMap()
        {   
            base.initializeMap();

            ObjectFactory.getInstance().setSpecialEntityIds(out heroId, out campId, out cpId);  
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
                        new GameEvent(State.EventType.GAME_OVER_MAN, null, gameOverResult.sound, gameOverResult.result, new Coordinate(0,0))
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
                if (eMan.getLivePlayerEntities(PlayerManager.getInstance().currentPlayerId).Count == 0)         //No live entities
                {
                    this.gameOverResult = new GameResult(State.GameState.Defeat, "Your Entire Army has Fallen!", SoundManager.getInstance().AudioCollection.messages.heroFallen);
                }

                if (eMan.getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId).Count == 0)         //No live heros
                {
                    this.gameOverResult = new GameResult(State.GameState.Defeat, "Your Hero has Fallen!", SoundManager.getInstance().AudioCollection.messages.heroFallen);
                }

                //TODO: We can remove this one; others will override it
                if (eMan.getLivePlayerEntities(PlayerManager.getInstance().computerPlayerId).Count == 0)         //No live entities
                {
                    this.gameOverResult = new GameResult(State.GameState.Victory, "Your Enemy's Entire Army has Fallen!", "");
                }

                if (eMan.getLivePlayerHeros(PlayerManager.getInstance().computerPlayerId).Count == 0)         //No live heros
                {
                    this.gameOverResult = new GameResult(State.GameState.Victory, "Your Enemy's Hero has Fallen!", "");
                }

                //if no control points are held, or the camp is destroyed
                if (eMan.getPlayerTotalOwnedControlPoints(PlayerManager.getInstance().currentPlayerId) == 0)               //If no control points are held
                {
                    this.gameOverResult = new GameResult(State.GameState.Defeat, "You have Lost all Control Points!", "");
                }

                if (eMan.getPlayerTotalOwnedControlPoints(PlayerManager.getInstance().computerPlayerId) == 0)            //If no control points are held
                {
                    this.gameOverResult = new GameResult(State.GameState.Victory, "You have Conquered all Control Points!", "");
                }

                if (eMan.isPlayerCampDestroyed(PlayerManager.getInstance().currentPlayerId))                     //If camp is destroyed
                {
                    this.gameOverResult = new GameResult(State.GameState.Defeat, "Your Camp has been Destroyed!", SoundManager.getInstance().AudioCollection.messages.campDestroyed);
                }

                if (eMan.isPlayerCampDestroyed(PlayerManager.getInstance().computerPlayerId))                 //If camp is destroyed
                {
                    this.gameOverResult = new GameResult(State.GameState.Victory, "Your Enemy's Camp has been Destroyed!", "");
                }

                //...
            }
                
            return new GameResult();
        }

    }
}

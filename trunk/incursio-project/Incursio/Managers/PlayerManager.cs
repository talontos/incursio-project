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
using Microsoft.Xna.Framework;
using Incursio.Utils;

namespace Incursio.Managers
{
    class PlayerManager
    {
        //TEMPRORARY
        public Player computerPlayer{
            get{return this.getPlayerById(computerPlayerId);}
        }

        //TEMPRORARY
        public Player humanPlayer{
            get { return this.getPlayerById(currentPlayerId); }
        }

        //TEMPORARY
        public int currentPlayerId = 0;
        public int computerPlayerId = 1;

        public int MAX_PLAYERS = 2;

        private List<Player> players;

        private static PlayerManager instance;

        public static PlayerManager getInstance(){
            if (instance == null){
                instance = new PlayerManager();
                instance.initializePlayerManager();
            }

            return instance;
        }

        public void reinitializeInstance()
        {
            instance = new PlayerManager();
            instance.initializePlayerManager();
        }

        public void updatePlayers(GameTime gameTime){

            players.ForEach(delegate(Player p)
            {
                p.update(gameTime);
            });

        }

        public void initializePlayerManager(){
            this.players = new List<Player>(this.MAX_PLAYERS);

            this.addNewPlayer(true);
            this.addNewPlayer(false);

            this.humanPlayer.MONETARY_UNIT = 1000;
        }

        /// <summary>
        /// if player list is not full, create and add a new player
        /// </summary>
        /// <returns></returns>
        public bool addNewPlayer(bool isComputer){
            if(players.Count < this.MAX_PLAYERS){
                if(isComputer)
                    this.insertPlayer(new AIPlayer(new SimpleAI()));
                else{
                    this.insertPlayer(new Player());
                }

                return true;
            }

            return false;
        }

        private void insertPlayer(Player newPlayer){
            newPlayer.id = players.Count;
            players.Add(newPlayer);
        }

        public void notifyPlayer(int player, GameEvent e){
            //get player
            players.ForEach(delegate(Player p)
            {
                if(p.id == player){
                    p.dispatchEvent(e);
                }
            });
        }

        public Player getPlayerById(int id){
            return players[id];
        }


    }
}

/****************************************
 * Copyright � 2008, Team RobotNinja:
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
        public Player computerPlayer;
        public Player humanPlayer;

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
            //TODO: remove separate players; use list
            this.computerPlayer = new AIPlayer(new SimpleAI());
            this.computerPlayer.id = State.PlayerId.COMPUTER;

            this.humanPlayer = new Player();
            this.humanPlayer.id = State.PlayerId.HUMAN;
            this.humanPlayer.MONETARY_UNIT = 1000;

            this.players = new List<Player>();
            players.Add(computerPlayer);
            players.Add(humanPlayer);
        }

        /// <summary>
        /// if player list is not full, create and add a new player
        /// </summary>
        /// <returns></returns>
        public bool addNewPlayer(bool isComputer){
            if(players.Count < 8){
                if(isComputer)
                    players.Add(new AIPlayer(new SimpleAI()));
                else{
                    players.Add(new Player());
                }

                return true;
            }

            return false;
        }

        public void notifyPlayer(State.PlayerId player, GameEvent e){
            //get player
            players.ForEach(delegate(Player p)
            {
                if(p.id == player){
                    p.dispatchEvent(e);
                }
            });
        }

        public Player getPlayerById(State.PlayerId id){
            for(int i = 0; i < this.players.Count; i++){
                if (players[i].id == id)
                    return players[i];
            }

            return null;
        }


    }
}

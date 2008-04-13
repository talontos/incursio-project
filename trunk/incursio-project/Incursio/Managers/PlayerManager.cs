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

        public void updatePlayers(GameTime gameTime){

            players.ForEach(delegate(Player p)
            {
                p.update(gameTime);
            });

        }

        public void initializePlayerManager(){
            //TODO: remove separate players; use list
            this.computerPlayer = new AIPlayer(new BaseAI());
            this.computerPlayer.id = State.PlayerId.COMPUTER;

            this.humanPlayer = new Player();
            this.humanPlayer.id = State.PlayerId.HUMAN;
            this.humanPlayer.MONETARY_UNIT = 1000;

            this.players = new List<Player>();
            players.Add(computerPlayer);
            players.Add(humanPlayer);
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


    }
}

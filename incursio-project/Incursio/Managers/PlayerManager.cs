using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;

namespace Incursio.Managers
{
    class PlayerManager
    {
        public Player computerPlayer;
        public Player humanPlayer;

        private static PlayerManager instance;

        public static PlayerManager getInstance(){
            if (instance == null){
                instance = new PlayerManager();
                instance.initializePlayerManager();
            }

            return instance;
        }

        public void initializePlayerManager(){
            this.computerPlayer = new Player();
            this.computerPlayer.id = State.PlayerId.COMPUTER;

            this.humanPlayer = new Player();
            this.humanPlayer.id = State.PlayerId.HUMAN;
        }


    }
}

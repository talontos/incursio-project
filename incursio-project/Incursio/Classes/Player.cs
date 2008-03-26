using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Microsoft.Xna.Framework;

using Incursio.Utils;

namespace Incursio.Classes
{
    public class Player
    {
        public String name = "";
        public List<BaseGameEntity> selection;
        public int heroId;
        public long MONETARY_UNIT = 0;
        public int ownedControlPoints = 0;
        public Enum id;


        public Player(){
            this.name = "Player";
        }

        public Player(String playerName)
        {
            this.name = playerName;
        }

        public void update(GameTime gameTime){
            //update player-specifics
            //money
        }
    }
}
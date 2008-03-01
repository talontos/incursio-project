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
        public Hero hero;
        public long MONETARY_UNIT = 0;
        public int ownedControlPoints = 0;
        public Enum id;


        public Player(){
            this.name = "Player";
            Incursio.getInstance().factory.create(Hero.HERO_CLASS);
        }

        public Player(String playerName)
        {
            Incursio.getInstance().factory.create(Hero.HERO_CLASS);
            this.name = playerName;
        }

        public void update(GameTime gameTime){
            //update player-specifics

            //update units
        }
    }
}
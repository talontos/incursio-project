using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;

namespace Incursio.Classes
{
    class Player
    {
        private String name = "";
        private List<BaseGameEntity> units;
        private Hero hero;
        private long MONETARY_UNIT = 0;
        private int ownedControlPoints = 0;

        public Player(){
            this.name = "Player";
            this.units = new List<BaseGameEntity>();
            this.hero = new Hero();
        }
    }
}
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

        public Player(){
            this.name = "Player";
            this.units = new List<BaseGameEntity>();
        }
    }
}
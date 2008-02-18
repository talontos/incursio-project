using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;

namespace Incursio.Classes
{
    class Player
    {
        private String name = "";
        private List<Unit> units;
        private List<Structure> structures;
        private Hero hero;
        private long MONETARY_UNIT = 0;
        private int ownedControlPoints = 0;

        public Player(){
            this.name = "Player";
            this.units = new List<Unit>();
            this.structures = new List<Structure>();
            this.hero = new Hero();
        }

        public Player(String playerName)
        {
            this.units = new List<Unit>();
            this.structures = new List<Structure>();
            this.hero = new Hero();
            this.name = playerName;
        }
    }
}
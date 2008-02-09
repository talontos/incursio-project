using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Classes
{
    class Unit : BaseGameEntity
    {
        private long damage = 0;

        public Unit(){

        }

        public void move(Coordinate destination){

        }

        public void attack(){

        }

        //Getters & Setters
        public long getDamage(){
            return this.damage;
        }

        public void setDamage(long damage){
            this.damage = damage;
        }
    }
}

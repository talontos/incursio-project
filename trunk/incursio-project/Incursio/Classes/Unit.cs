using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Classes
{
    class Unit : BaseGameEntity
    {
        protected long damage = 0;
        protected long armor = 0;

        public Unit() : base(){

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

        public long getArmor()
        {
            return this.armor;
        }

        public void setArmor(long armor)
        {
            this.armor = armor;
        }
    }
}

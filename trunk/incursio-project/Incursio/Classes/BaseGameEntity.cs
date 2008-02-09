using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Classes
{
    class BaseGameEntity
    {
        private String type = "";    //unit type-name?
        private long health = 0;
        private int  sightRange = 0;
        private Player owner;
        private Enum currentState;
        private Coordinate location;

        public BaseGameEntity(){
            
        }

        //Getters/Setters//

        public String getType(){
            return this.type;
        }

        public void setType(String type){
            this.type = type;
        }

        public long getHealth(){
            return this.health;
        }

        public void setHealth(int health){
            this.health = health;
        }

        public Player getPlayer(){
            return this.owner;
        }

        public void setPlayer(Player owner){
            this.owner = owner;
        }

        public long getSightRange(){
            return this.sightRange;
        }

        public void setSightRange(long sightRange){
            this.sightRange = sightRange;
        }

        public long getDamage(){
            return this.damage;
        }
    }
}

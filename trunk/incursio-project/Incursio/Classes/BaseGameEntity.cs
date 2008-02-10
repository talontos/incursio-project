using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Classes
{
    class BaseGameEntity
    {
        protected String type = "";    //unit type-name?
        protected long health = 0;
        protected int  sightRange = 0;
        protected Player owner;
        protected Enum currentState;
        protected Coordinate location;

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

        public void setSightRange(int sightRange){
            this.sightRange = sightRange;
        }
    }
}

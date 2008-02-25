using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;

namespace Incursio.Classes
{
    public class BaseGameEntity
    {
        protected String type = "";    //unit type-name?
        protected long health = 0;
        protected int  sightRange = 0;
        protected Player owner;
        protected Enum currentState;
        protected Coordinate location;
        protected int keyId = -1;

        public BaseGameEntity(){
            
        }

        protected void Update(GameTime gameTime){

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

        public Coordinate getLocation()
        {
            return this.location;
        }

        public void setLocation(Coordinate coords)
        {
            this.location = coords;
        }

        public Enum getState()
        {
            return this.currentState;
        }

        public void setState(Enum state)
        {
            this.currentState = state;
        }

        public int getKeyId(){
            return this.keyId;
        }

        public void setKeyId(int key){
            this.keyId = key;
        }
    }
}

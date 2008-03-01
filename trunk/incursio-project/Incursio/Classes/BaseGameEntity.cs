using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Classes
{
    public class BaseGameEntity
    {
        //protected String type = "";    //unit type-name?
        public Texture2D texture;

        protected State.EntityName entityType;
        protected long health = 0;
        protected int  sightRange = 0;
        protected State.PlayerId owner;
        protected Coordinate location = new Coordinate(0,0);
        protected int keyId = -1;

        public BaseGameEntity(){
            
        }

        public virtual void Update(GameTime gameTime){

        }

        //Getters/Setters//

        /*public String getType(){
            return this.type;
        }*/

        /*public void setType(String type){
            this.type = type;
        }*/

        public void setType(State.EntityName name)
        {
            this.entityType = name;
        }

        public State.EntityName getType()
        {
            return this.entityType;
        }

        public long getHealth(){
            return this.health;
        }

        public void setHealth(int health){
            this.health = health;
        }

        public Enum getPlayer(){
            return this.owner;
        }

        public void setPlayer(State.PlayerId owner){
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

        public int getKeyId(){
            return this.keyId;
        }

        public void setKeyId(int key){
            this.keyId = key;
        }
    }
}

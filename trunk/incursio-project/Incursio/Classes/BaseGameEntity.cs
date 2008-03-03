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

        public State.EntityName entityType;
        public long maxHealth = 100;
        public long health = 100;
        public int  sightRange = 0;
        public State.PlayerId owner;
        public Coordinate location = new Coordinate(0,0);
        public int keyId = -1;
        public bool visible = false;

        public BaseGameEntity(){
            
        }

        public virtual void Update(GameTime gameTime){
            //TODO: draw unit here?
            //TODO: check if i'm clicked?
        }

        public virtual void Render(){

        }

        public virtual String getTextureName(){
            return null;
        }

        //Getters/Setters//

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

        public long getMaxHealth()
        {
            return this.maxHealth;
        }

        public void setMaxHealth(int newHealth)
        {
            this.maxHealth = newHealth;
        }

        public State.PlayerId getPlayer(){
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

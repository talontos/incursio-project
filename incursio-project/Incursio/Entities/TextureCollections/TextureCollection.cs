using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Entities.TextureCollections
{
    public class TextureCollection
    {
        public int id;
        public string name;

        public Texture2D portrait;

        public StillTextures still;
        public MovementTextures movement;
        public AttackTextures attacking;
        public DeathTextures death;
        public DamagedTextures damaged;

        public TextureCollection(int id, string name, string portraitName){
            this.id = id;
            this.name = name;

            if(portraitName != null && portraitName.Length > 0){
                //TODO: LOAD PORTRAIT
            }
        }

        public TextureSet addSetOfType(string type){
            switch(type){
                case "Still":       still = new StillTextures();        return still;
                case "Movement":    movement = new MovementTextures();  return movement;
                case "Attacking":   attacking = new AttackTextures();   return attacking;
                case "Death":       death = new DeathTextures();        return death;
                case "Damaged":     damaged = new DamagedTextures();    return damaged;
                default: return null;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Incursio.Managers;

namespace Incursio.Entities.TextureCollections
{
    public class TextureCollection
    {
        public int id;
        public string name;

        public Texture2D portrait;
        public Texture2D icon;

        public StillTextures still;
        public MovementTextures movement;
        public AttackTextures attacking;
        public DeathTextures death;
        public DamagedTextures damaged;
        public TerrainTextures terrain;
        public InterfaceTextures interfaceTextures;
        public ProjectileTextures projectiles;

        public TextureCollection(int id, string name, string portraitName, string iconName){
            this.id = id;
            this.name = name;

            if(portraitName != null && portraitName.Length > 0){
                //TODO: We will probably need a full path...
                portrait = Texture2D.FromFile(Incursio.getInstance().spriteBatch.GraphicsDevice,
                    //global::Incursio.Utils.EntityConfiguration.FileConfig.texturePath + portraitName);
                    ConfigurationManager.getInstance().contentDirectory + portraitName);
            }
            else
            {
                //TODO: GET A DEFAULT portrait
                portrait = global::Incursio.Managers.TextureBank.getInstance().defaultPortrait;
            }

            if (iconName != null && iconName.Length > 0)
            {
                //TODO: We will probably need a full path...
                icon = Texture2D.FromFile(Incursio.getInstance().spriteBatch.GraphicsDevice,
                    //global::Incursio.Utils.EntityConfiguration.FileConfig.texturePath + iconName);
                    ConfigurationManager.getInstance().contentDirectory + iconName);
            }
            else{
                //TODO: GET A DEFAULT ICON
                icon = global::Incursio.Managers.TextureBank.getInstance().defaultIcon;
            }
        }

        public TextureSet addSetOfType(string type){
            switch(type){
                case "Still":       still = new StillTextures();                 return still;
                case "Movement":    movement = new MovementTextures();           return movement;
                case "Attacking":   attacking = new AttackTextures();            return attacking;
                case "Death":       death = new DeathTextures();                 return death;
                case "Damaged":     damaged = new DamagedTextures();             return damaged;
                case "Terrain":     terrain = new TerrainTextures();             return terrain;
                case "Interface":   interfaceTextures = new InterfaceTextures(); return interfaceTextures;
                case "Projectile":  projectiles = new ProjectileTextures();      return projectiles;
                default: return null;
            }
        }
    }
}

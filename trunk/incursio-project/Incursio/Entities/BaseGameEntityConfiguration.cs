using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Entities.Components;
using Incursio.Entities.TextureCollections;

using Incursio.Entities.AudioCollections;
using Microsoft.Xna.Framework;

namespace Incursio.Entities
{
    /// <summary>
    /// This is a class used to house all information required to construct an instance of an entity
    /// </summary>
    public class BaseGameEntityConfiguration
    {
        public int classID;
        public string className;

        public int maxHealth = 100;
        public int health = 100;
        public int armor = 0;
        public int sightRange = 0;
        public int pointValue = 0;

        public Vector2 size = new Vector2(1, 1);

        public int costToBuild = 0;

        public bool isHero = false;
        public bool isMainBase = false;
        public bool isControlPoint = false;
        public bool isTurret = false;

        public bool isStructure = false;
        public bool invulnerable = false;

        public List<ComponentConfiguration> components = new List<ComponentConfiguration>();

        public BaseGameEntityConfiguration(int id, string name, params string[] args){
            this.classID = id;
            this.className = name;

            //set all other properties that are given
            for(int i = 0; (i + 1) < args.Length; i += 2){
                switch(args[i].ToUpper()){
                    case "MAXHEALTH":       maxHealth       = int.Parse(args[i + 1]); break;
                    case "HEALTH":          health          = int.Parse(args[i + 1]); break;
                    case "ARMOR":           armor           = int.Parse(args[i + 1]); break;
                    case "SIGHTRANGE":      sightRange      = int.Parse(args[i + 1]); break;
                    case "POINTVALUE":      pointValue      = int.Parse(args[i + 1]); break;
                    case "COSTTOBUILD":     costToBuild     = int.Parse(args[i + 1]); break;
                    case "ISHERO":          isHero          = bool.Parse(args[i + 1]); break;
                    case "ISMAINBASE":      isMainBase      = bool.Parse(args[i + 1]); break;
                    case "ISCONTROLPOINT":  isControlPoint  = bool.Parse(args[i + 1]); break;
                    case "ISTURRET":        isTurret        = bool.Parse(args[i + 1]); break;
                    case "ISSTRUCTURE":     isStructure     = bool.Parse(args[i + 1]); break;
                    case "WIDTH":           size.X          = float.Parse(args[i + 1]); break;
                    case "HEIGHT":          size.Y          = float.Parse(args[i + 1]); break;
                    case "INVULNERABLE":
                    case "INVINCIBLE":      invulnerable    = bool.Parse(args[i + 1]); break;
                    default: break; //TODO: display some sort of parse error
                }
            }
        }

        public void addComponentConfiguration(string componentType, params string[] args){
            ComponentConfiguration cc = new ComponentConfiguration(componentType);

            for (int i = 0; (i + 1) < args.Length; i += 2){
                cc.addAttribute(new KeyValuePair<string, string>(args[i], args[i + 1]));
            }

            this.components.Add(cc);
        }

        public BaseGameEntity buildEntity(){
            BaseGameEntity e = new BaseGameEntity();

            e.entityName     = this.className;
            e.maxHealth      = this.maxHealth;
            e.health         = this.health;
            e.armor          = this.armor;
            e.sightRange     = this.sightRange;
            e.pointValue     = this.pointValue;

            e.size           = this.size;

            e.isHero         = this.isHero;
            e.isMainBase     = this.isMainBase;
            e.isControlPoint = this.isControlPoint;
            e.isTurret       = this.isTurret;
            e.isStructure    = this.isStructure;

            e.invulnerable   = this.invulnerable;

            this.components.ForEach(delegate(ComponentConfiguration cc)
            {
                cc.addToEntity(e);
            });

            return e;
        }

    }
}

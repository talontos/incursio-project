using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Entities.Components;

namespace Incursio.Entities
{
    /// <summary>
    /// This is a class used to house all information required to construct an instance of an entity
    /// </summary>
    public class BaseGameEntityConfiguration
    {
        public int classID;
        public string className;

        public int keyId = -1;
        public int maxHealth = 100;
        public int health = 100;
        public int armor = 0;
        public int sightRange = 0;
        public Coordinate location = new Coordinate(0, 0);
        public int pointValue = 0;
        public int playerId = -1;

        public List<ComponentConfiguration> components = new List<ComponentConfiguration>();

        public BaseGameEntityConfiguration(int id, string name, params string[] args){
            this.classID = id;
            this.className = name;

            //set all other properties that are given
            for(int i = 0; (i + 1) < args.Length; i += 2){
                switch(args[i]){
                    case "maxHealth":   maxHealth   = args[i + 1]; break;
                    case "health":      health      = args[i + 1]; break;
                    case "armor":       armor       = args[i + 1]; break;
                    case "sightRange":  sightRange  = args[i + 1]; break;
                    case "pointValue":  pointValue  = args[i + 1]; break;
                    case "playerId":    playerId    = args[i + 1]; break;
                    default: break;
                }
            }
        }

        private void addComponentConfiguration(string componentType, params string[] args){
            ComponentConfiguration cc = new ComponentConfiguration(componentType);

            for (int i = 0; (i + 1) < args.Length; i += 2){
                cc.addAttribute(new KeyValuePair<string, object>(args[i], args[i + 1]));
            }
        }

        public void setEntityAttributes(out BaseGameEntity entity){

        }

    }
}

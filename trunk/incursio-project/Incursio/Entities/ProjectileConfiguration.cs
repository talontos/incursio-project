using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Entities.Projectiles;

namespace Incursio.Entities
{
    public class ProjectileConfiguration
    {
        public int id;
        public string name;
        public int damage;
        public int speed;
        public ComponentConfiguration renderComponentConfiguration;

        public ProjectileConfiguration(int id, string name, params string[] args)
        {
            this.id = id;
            this.name = name;

            //set all other properties that are given
            for(int i = 0; (i + 1) < args.Length; i += 2){
                switch(args[i].ToUpper()){
                    case "DAMAGE":      damage       = int.Parse(args[i + 1]); break;
                    case "SPEED":       speed        = int.Parse(args[i + 1]); break;

                    case "TEXTURESET": 
                        renderComponentConfiguration = new ComponentConfiguration("RenderComponent");
                        renderComponentConfiguration.addAttribute(new KeyValuePair<string,string>("collectionName",args[i + 1]));
                        break;
                }
            }
        }

        public BaseProjectile buildProjectile(){
            return new BaseProjectile(this);
        }

    }
}

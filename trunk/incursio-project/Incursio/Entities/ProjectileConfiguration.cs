using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities
{
    public class ProjectileConfiguration
    {
        //TODO: MAKE BETTER...
        public string name;
        public int damage;
        public int speed;
        public string textureName;

        public ProjectileConfiguration(int id, string name, params string[] args)
        {
            //set all other properties that are given
            for(int i = 0; (i + 1) < args.Length; i += 2){
                switch(args[i]){
                    case "name":       name         = args[i + 1];            break;
                    case "damage":     damage       = int.Parse(args[i + 1]); break;
                    case "speed":      speed        = int.Parse(args[i + 1]); break;
                    case "texture":    textureName  = args[i + 1];            break;
                    default: break;
                }
            }
        }

    }
}

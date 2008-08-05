/****************************************
 * Copyright © 2008, Team RobotNinja:
 * 
 *     - Henry Armstrong
 *     - Andy Burras
 *     - Mitch Martin
 * 
 * All Rights Reserved
 ***************************************/

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
using Incursio.Entities.Projectiles;
using Incursio.Entities;

namespace Incursio.Managers
{
    /// <summary>
    /// Class with static subclasses for use to store Texture references to avoid constant string-matching
    /// </summary>
    public class ProjectileBank
    {
        private static ProjectileBank instance;

        public List<ProjectileConfiguration> projectileConfigurations;

        private ProjectileBank(){
            this.projectileConfigurations = new List<ProjectileConfiguration>();
        }

        public static ProjectileBank getInstance(){
            if(instance == null)
                instance = new ProjectileBank();

            return instance;
        }

        public BaseProjectile getProjectileByName(string name){
            for (int i = 0; i < this.projectileConfigurations.Count; i++){
                if (projectileConfigurations[i].name.Equals(name))
                    return projectileConfigurations[i].buildProjectile();
            }

            return null;
        }
    }
}

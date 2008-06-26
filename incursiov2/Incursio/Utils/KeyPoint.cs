/****************************************
 * Copyright © 2008, Team RobotNinja:
 * 
 *     - Henry Armstrong
 *     - Andy Burras
 *     - Mitch Martin
 *     - Xuan Yu
 * 
 * All Rights Reserved
 ***************************************/

using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Managers;
using Incursio.Entities;

namespace Incursio.Utils
{
    /// <summary>
    /// This class represents an important point on a map; generally a camp or control point
    /// </summary>
    public class KeyPoint
    {
        public int owner{
            get { 
                return (this.structure != null ? this.structure.owner : PlayerManager.getInstance().currentPlayerId);
            }
        }

        private int prevOwner;

        public int priority = -1;

        public BaseGameEntity structure;

        private int preferredNumTowers = 1;
        private int preferredNumUnits = 2;

        public int numUnitsOrdered = 0;
        public int numGuardTowersOrdered = 0;

        public int numUnits;
        public int numGuardTowers = 0;

        public bool pointSecure = false;

        public KeyPoint(){

        }

        public KeyPoint(BaseGameEntity structure){
            this.structure = structure;
            this.prevOwner = this.owner;

            //TODO:
            /*
            if(this.structure){
                //this.setDefensePreferences(3, 5);
            }
            */
        }

        public void setDefensePreferences(int numTowers, int numUnits){
            this.preferredNumTowers = numTowers;
            this.preferredNumUnits = numUnits;
        }

        public DefenseReport Update(){
            //analyze defenses
            List<BaseGameEntity> friendlies, enemies;
            this.numUnits = 0;
            this.numGuardTowers = 0;
            this.pointSecure = true;

            if(this.owner != prevOwner){
                //owner changed, reset values
                this.numGuardTowersOrdered = 0;
                this.numUnitsOrdered = 0;
                this.prevOwner = this.owner;
            }

            DefenseReport report = new DefenseReport(this.structure.location, this);

            EntityManager.getInstance().getAllEntitiesInRange(this.owner, structure.location, 
                structure.sightRange, out friendlies, out enemies);

            friendlies.ForEach(delegate(BaseGameEntity e)
            {
                if (e.movementComponent != null)
                    numUnits++;
                else if (e.isTurret)
                    numGuardTowers++;

            });

            if (numGuardTowers < this.preferredNumTowers)
            {
                if( (numGuardTowers + numGuardTowersOrdered) < this.preferredNumTowers){
                    //BUILD SOME TOWERS!
                    report.setNumTowers(this.preferredNumTowers - numGuardTowers - numGuardTowersOrdered);
                    this.numGuardTowersOrdered += report.numTowersToBuild;
                }
                this.pointSecure = false;
            }

            if(numUnits < this.preferredNumUnits)
            {
                if ((numUnits + numUnitsOrdered) < this.preferredNumUnits){
                    //GET SOME MEN OVER HERE!
                    report.setNumUnits(this.preferredNumUnits - numUnits - numUnitsOrdered);
                    this.numUnitsOrdered += report.numUnitsToBuild;
                }
                this.pointSecure = false;
            }

            report.secure = this.pointSecure;
            return report;
        }

    }
}

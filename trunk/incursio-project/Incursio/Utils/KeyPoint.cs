using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;
using Incursio.Managers;

namespace Incursio.Utils
{
    public class KeyPoint
    {
        public State.PlayerId owner{
            get { 
                return (this.structure != null ? this.structure.owner : State.PlayerId.HUMAN);
            }
        }

        public Structure structure;

        private int preferredNumTowers = 1;
        private int preferredNumUnits = 2;

        public int numUnitsOrdered = 0;
        public int numGuardTowersOrdered = 0;

        public int numUnits;
        public int numGuardTowers = 0;

        public bool pointSecure = false;

        public KeyPoint(Structure structure){
            this.structure = structure;

            if(this.structure is CampStructure){
                //this.setDefensePreferences(3, 5);
            }
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

            DefenseReport report = new DefenseReport(this.structure.location, this);

            EntityManager.getInstance().getAllEntitiesInRange(this.owner, structure.location, 
                structure.sightRange, out friendlies, out enemies);

            friendlies.ForEach(delegate(BaseGameEntity e)
            {
                if (e is Unit)
                    numUnits++;
                else if (e is GuardTowerStructure)
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

            //if (numUnitsOrdered > 0 || numGuardTowersOrdered > 0)
            //    this.pointSecure = false;

            report.secure = this.pointSecure;
            return report;
        }

    }
}

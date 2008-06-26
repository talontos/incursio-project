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

namespace Incursio.Utils
{
    public class DefenseReport
    {
        public bool secure = true;
        public int numUnitsToBuild = 0;
        public int numTowersToBuild = 0;
        public Coordinate location;
        public KeyPoint keyPoint;

        public DefenseReport(Coordinate loc, KeyPoint k){
            this.location = loc;
            this.keyPoint = k;
        }

        public DefenseReport(int units, int towers, Coordinate loc, KeyPoint k){
            this.secure = false;
            this.numTowersToBuild = towers;
            this.numUnitsToBuild = units;
            this.location = loc;
            this.keyPoint = k;
        }

        public void setNumTowers(int towers){
            this.secure = false;
            this.numTowersToBuild = towers;
        }

        public void setNumUnits(int units){
            this.secure = false;
            this.numUnitsToBuild = units;
        }
    }
}

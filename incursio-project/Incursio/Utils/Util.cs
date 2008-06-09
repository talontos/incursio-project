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
    public class Util
    {
        public static int UnitStopMoveRange = 32;
        public static int RandomNumberSeed = 100;

        public static string selectRandomString(ref List<string> list){
            return list[Incursio.rand.Next(0, list.Count)];
        }

        public static int selectRandomInt(ref List<int> list){
            return list[Incursio.rand.Next(0, list.Count)];
        }
    }
}

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

//TODO: remove this class and use Point or Vector2
//  this is redundant
namespace Incursio.Utils
{
    public class Coordinate
    {
        public int x = 0;
        public int y = 0;

        public Coordinate(){}

        public Coordinate(int x, int y){
            this.x = x;
            this.y = y;
        }

        public Microsoft.Xna.Framework.Point toPoint(){
            return new Microsoft.Xna.Framework.Point(x,y);
        }

        public Microsoft.Xna.Framework.Vector2 toVector2(){
            return new Microsoft.Xna.Framework.Vector2(x, y);
        }
    }
}

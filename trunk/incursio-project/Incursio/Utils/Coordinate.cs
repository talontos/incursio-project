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
using Microsoft.Xna.Framework;

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

        public Coordinate(Vector2 v){
            this.x = (int)v.X;
            this.y = (int)v.Y;
        }

        public Point toPoint(){
            return new Microsoft.Xna.Framework.Point(x,y);
        }

        public Vector2 toVector2(){
            return new Microsoft.Xna.Framework.Vector2(x, y);
        }
    }
}

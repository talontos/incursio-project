using System;
using System.Collections.Generic;
using System.Text;

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
    }
}

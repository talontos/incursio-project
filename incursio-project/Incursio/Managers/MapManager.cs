using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Managers
{
    class MapManager
    {
        private static MapManager instance;

        private MapManager(){
        
        }

        public static MapManager getInstance(){
            if(instance == null)
                instance = new MapManager();

            return instance;
        }
    }
}

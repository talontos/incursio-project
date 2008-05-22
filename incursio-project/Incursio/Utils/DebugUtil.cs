using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;
using Incursio.Interface;

namespace Incursio.Utils
{
    public class DebugUtil
    {
        public static void matchCommand(string command){
            try{
                //TODO: SPLIT THIS BETTWER....
                command = command.Replace(" ", "").ToUpper();
                string[] list = command.Split(':');

                switch(list[0]){
                    case "DRAWOCCUPANCY":
                    case "SHOWOCCUPANCY":
                        MapManager.getInstance().DRAW_OCCUPANCY_GRID = !MapManager.getInstance().DRAW_OCCUPANCY_GRID;
                        break;

                    case "CREATEENTITY":
                        if(list.Length > 1){
                            EntityManager.getInstance().createNewEntity(int.Parse(list[1]), 
                                PlayerManager.getInstance().currentPlayerId).setLocation(new Coordinate(Cursor.getInstance().getPos()));
                        }
                        break;

                    case "ADDMONEY":
                        break;

                    case "REMOVEMONEY":
                        break;

                    case "KILL":
                        break;

                    case "ENDGAME":
                        break;

                    default:
                        break;
                }
            }
            finally{}
        }
    }
}

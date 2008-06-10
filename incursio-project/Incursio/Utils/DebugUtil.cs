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
                //space-separated commands/params
                command = command.ToUpper();
                string[] list = command.Split(' ');

                switch(list[0]){
                    case "PLAYBACKGROUNDMUSIC":
                        SoundManager.getInstance().PLAY_BG_MUSIC = !SoundManager.getInstance().PLAY_BG_MUSIC;
                        break;

                    case "DRAWOCCUPANCY":
                    case "SHOWOCCUPANCY":
                        MapManager.getInstance().DRAW_OCCUPANCY_GRID = !MapManager.getInstance().DRAW_OCCUPANCY_GRID;
                        break;

                    case "SHOWENTITIES":
                    case "DRAWENTITIES":
                    case "SHOWIDS":
                    case "DRAWIDS":
                    case "DRAWENTITYGRID":
                    case "SHOWENTITYGRID":
                        MapManager.getInstance().DRAW_ENTITY_GRID = !MapManager.getInstance().DRAW_ENTITY_GRID;
                        break;

                    case "TOGGLEGRIDS":
                    case "SHOWGRIDS":
                        MapManager.getInstance().DRAW_OCCUPANCY_GRID = !MapManager.getInstance().DRAW_OCCUPANCY_GRID;
                        MapManager.getInstance().DRAW_ENTITY_GRID = !MapManager.getInstance().DRAW_ENTITY_GRID;
                        break;

                    //TODO: ADD FUNCTIONALITY TO MAKE BY NAME
                    case "MAKEENTITY":
                    case "CREATEENTITY":
                        int player = PlayerManager.getInstance().currentPlayerId;

                        if (list.Length > 2)
                            player = int.Parse(list[2]);

                        if(list.Length > 1){
                            EntityManager.getInstance().createNewEntity(int.Parse(list[1]),
                                player).setLocation(new Coordinate(Cursor.getInstance().getPos()));
                        }
                        break;

                    case "ADDMONEY":
                        PlayerManager.getInstance().getPlayerById(
                            (list.Length > 2 ? int.Parse(list[2]) : PlayerManager.getInstance().currentPlayerId)
                        ).MONETARY_UNIT += (list.Length > 1 ? int.Parse(list[1]) : 1000);

                        break;

                    case "REMOVEMONEY":
                        PlayerManager.getInstance().getPlayerById(
                            (list.Length > 2 ? int.Parse(list[2]) : PlayerManager.getInstance().currentPlayerId)
                        ).MONETARY_UNIT -= (list.Length > 1 ? int.Parse(list[1]) : 1000);

                        break;

                    case "ECHO":
                        MessageManager.getInstance().addMessage(new GameEvent(State.EventType.CHAT_MESSAGE, null, "", command.Substring(4), new Coordinate(0, 0)));
                        break;

                    case "ENDGAME":
                        if (list.Length > 1)
                        {
                            switch(list[1]){
                                case "VICTORY":
                                    Incursio.getInstance().currentState = State.GameState.Victory;
                                    break;

                                case "DEFEAT":
                                    Incursio.getInstance().currentState = State.GameState.Defeat;
                                    break;

                                default:
                                    break;
                            }
                        }
                        break;

                    case "ENTITYSETTINGS":
                        //TODO: print out entity names + IDs
                        break;

                    default:
                        break;
                }
            }
            catch(Exception e){
                Console.WriteLine(e.StackTrace);
            }
            finally{}
        }
    }
}

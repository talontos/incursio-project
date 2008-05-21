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

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Campaign;
using Microsoft.Xna.Framework;
using Incursio.Utils.PathFinding;
using Incursio.Classes.PathFinding;
using Incursio.Utils;

namespace Incursio.Managers
{
    public class MapManager
    {
        private static MapManager instance;

        public static int TILE_WIDTH = 32;
        public static int TILE_HEIGHT = 32;

        public MapBase currentMap;
        public List<KeyPoint> keyPoints;

        public PathFinder pathFinder;

        private MapManager(){
            keyPoints = new List<KeyPoint>();
        }

        public static MapManager getInstance(){
            if(instance == null)
                instance = new MapManager();

            return instance;
        }

        public MapBase setCurrentLevel(State.CampaignLevel level){
            
            switch(level){
                //TODO: Credits 'map'?
                /*case State.CampaignLevel.CREDITS:
                    currentMap = new CreditsMap();
                    break;
                */

                //TODO: ADD BACK IN
                /*
                case State.CampaignLevel.ONE:
                    currentMap = new TestMap();// new Port();  
                    break;

                case State.CampaignLevel.TWO:   
                    currentMap = new Inland();  
                    break;

                case State.CampaignLevel.THREE: 
                    currentMap = new Capital();  
                    break;
                */
            }

            //TEMPORARY
            currentMap = new TestMap();

            this.pathFinder = new PathFinder(currentMap.occupancyGrid);
            MovableObject.Initalize(currentMap.width, currentMap.TILE_WIDTH);
            Incursio.getInstance().currentMap = this.currentMap;

            return this.currentMap;
        }

        public void initializeCurrentMap(){
            currentMap.initializeMap();

            //create the key points
            List<BaseGameEntity> all = EntityManager.getInstance().getAllEntities();

            this.keyPoints = new List<KeyPoint>();
            all.ForEach(delegate(BaseGameEntity e)
            {
                if(e.isMainBase || e.isControlPoint){
                    this.keyPoints.Add(new KeyPoint(e));
                }
            });
        }

        public void UpdateCampaign(GameTime gameTime){
            GameResult winState = currentMap.inspectWinConditions();


            if (winState.resultState != State.GameState.None)
                Incursio.getInstance().GameResult = winState;
        }
    }
}

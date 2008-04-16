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

        public PathFinder pathFinder;

        private MapManager(){
        
        }

        public static MapManager getInstance(){
            if(instance == null)
                instance = new MapManager();

            return instance;
        }

        public MapBase setCurrentLevel(State.CampaignLevel level){
            //TODO: Define Different levels
            switch(level){
                case State.CampaignLevel.ONE:   
                    currentMap = new Map1();  
                    break;

                case State.CampaignLevel.TWO:   
                    currentMap = new RandomMap();  
                    break;

                case State.CampaignLevel.THREE: 
                    currentMap = new TestMap();  
                    break;
            }

            this.pathFinder = new PathFinder(currentMap.occupancyGrid);
            MovableObject.Initalize(currentMap.width, currentMap.TILE_WIDTH);

            return this.currentMap;
        }

        public void initializeCurrentMap(){
            currentMap.initializeMap();
        }

        public void UpdateCampaign(GameTime gameTime){
            GameResult winState = currentMap.inspectWinConditions();


            if (winState.resultState != State.GameState.None)
                Incursio.getInstance().GameResult = winState;
        }

        public void showLocation(Coordinate location){
            this.currentMap.showLocation(location);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Campaign;
using Microsoft.Xna.Framework;

namespace Incursio.Managers
{
    public class MapManager
    {
        private static MapManager instance;

        public MapBase currentMap;

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
                case State.CampaignLevel.ONE:   currentMap = new TestMap();  break;
                case State.CampaignLevel.TWO:   currentMap = new RandomMap();  break;
                case State.CampaignLevel.THREE: currentMap = new TestMap();  break;
            }

            return this.currentMap;
        }

        public void initializeCurrentMap(){
            currentMap.initializeMap();
        }

        public void UpdateCampaign(GameTime gameTime){
            State.GameState winState = currentMap.inspectWinConditions();

            if (winState != State.GameState.None)
                Incursio.getInstance().currentState = winState;
        }
    }
}

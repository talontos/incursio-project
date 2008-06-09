using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Managers
{
    public class ConfigurationManager
    {
        public string contentDirectory = "Content";
        public string entityConfigurationFileName = "Configuration/EntityConfiguration.xml";
        public string audioConfigurationFileName = "Configuration/AudioConfiguration.xml";
        public string textureConfigurationFileName = "Configuration/TextureConfiguration.xml";

        private static ConfigurationManager instance;

        public static ConfigurationManager getInstance(){
            if (instance == null)
                instance = new ConfigurationManager();

            return instance;
        }

        private ConfigurationManager(){

        }

        //TODO: string-checking for filenames?
        public void setConfigurationSetting(string setting, string value){
            switch(setting.ToUpper()){
                case "CONTENTDIRECTORY":
                    this.contentDirectory = value;
                    break;

                case "ENTITYCONFIGURATIONFILENAME":
                    this.entityConfigurationFileName = value;
                    break;

                case "AUDIOCONFIGURATIONFILENAME":
                    this.audioConfigurationFileName = value;
                    break;

                case "TEXTURECONFIGURATIONFILENAME":
                    this.textureConfigurationFileName = value;
                    break;

                case "PLAYBACKGROUNDMUSIC":
                    SoundManager.getInstance().PLAY_BG_MUSIC = bool.Parse(value);
                    break;

                case "DRAWENTITYGRID":
                case "SHOWENTITYGRID":
                    MapManager.getInstance().DRAW_ENTITY_GRID = bool.Parse(value);
                    break;

                case "DRAWOCCUPANCYGRID":
                case "SHOWOCCUPANCYGRID":
                    MapManager.getInstance().DRAW_OCCUPANCY_GRID = bool.Parse(value);
                    break;
            }
        }

    }
}

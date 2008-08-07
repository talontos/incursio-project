using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Utils;
using Incursio.Entities;
using Incursio.Entities.TextureCollections;
using Incursio.Entities.AudioCollections;

namespace Incursio.Managers
{
    public class ConfigurationManager
    {
        public static string ENTITY_CONFIGURATION_FILE_NAME = "EntityConfiguration.xml";
        public static string PROJECTILE_CONFIGURATION_FILE_NAME = "ProjectileConfiguration.xml";
        public static string AUDIO_CONFIGURATION_FILE_NAME = "AudioConfiguration.xml";
        public static string TEXTURE_CONFIGURATION_FILE_NAME = "TextureConfiguration.xml";

        public string contentDirectory = "../../../Content/";
        public string audioDirectory = "../../../Content/Audio/";
       
        public string currentConfigurationSchemeDirectory = "Incursio/";

        public string entityConfigurationFile;
        public string projectileConfigurationFile;
        public string audioConfigurationFile;
        public string textureConfigurationFile;

        private static ConfigurationManager instance;

        public static ConfigurationManager getInstance(){
            if (instance == null)
                instance = new ConfigurationManager();

            return instance;
        }

        private ConfigurationManager(){
            buildConfigurationTree();
        }

        private void buildConfigurationTree(){
            entityConfigurationFile = "Configuration/" + currentConfigurationSchemeDirectory + ENTITY_CONFIGURATION_FILE_NAME;
            projectileConfigurationFile = "Configuration/" + currentConfigurationSchemeDirectory + PROJECTILE_CONFIGURATION_FILE_NAME;
            audioConfigurationFile = "Configuration/" + currentConfigurationSchemeDirectory + AUDIO_CONFIGURATION_FILE_NAME;
            textureConfigurationFile = "Configuration/" + currentConfigurationSchemeDirectory + TEXTURE_CONFIGURATION_FILE_NAME;
        }

        //TODO: string-checking for filenames?
        public void setConfigurationSetting(string setting, string value){
            switch(setting.ToUpper()){
                case "CONTENTDIRECTORY":
                    if (!value.EndsWith("/"))
                        value = value + "/";
                    this.contentDirectory = value;
                    break;

                case "AUDIODIRECTORY":
                    if (!value.EndsWith("/"))
                        value = value + "/";
                    this.audioDirectory = value;
                    break;

                case "AUDIOENABLED":
                    SoundManager.AUDIO_ENABLED = bool.Parse(value);
                    break;

                case "CONFIGURATIONSCHEME":
                    if (!value.EndsWith("/"))
                        value = value + "/";
                    this.currentConfigurationSchemeDirectory = value;
                    this.buildConfigurationTree();
                    break;

                case "ENTITYCONFIGURATIONFILENAME":
                    this.entityConfigurationFile = value;
                    break;

                case "PROJECTILECONFIGURATIONFILENAME":
                    this.projectileConfigurationFile = value;
                    break;

                case "AUDIOCONFIGURATIONFILENAME":
                    this.audioConfigurationFile = value;
                    break;

                case "TEXTURECONFIGURATIONFILENAME":
                    this.textureConfigurationFile = value;
                    break;

                case "ENTITIESAUTOGUARD":
                    EntityManager.getInstance().ENTITIES_AUTO_GUARD = bool.Parse(value);
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

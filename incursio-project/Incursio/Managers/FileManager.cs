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
using System.IO;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;
using System.Xml;
using Incursio.Entities;
using Incursio.Entities.TextureCollections;
using Incursio.Entities.Components;

namespace Incursio.Managers
{
    public class FileManager
    {
        private static FileManager instance;

        private FileManager()
        {
            
        }

        public static FileManager getInstance(){
            if(instance == null)
                instance = new FileManager();
            
            return instance;
        }

        public void loadGameConfiguration()
        {
            //This is temporary now.  We'll be switching to XML configurations.
            //file is game.cfg.wtf
            try
            {
                //open up a reader
                TextReader tr = new StreamReader("game.cfg.wtf");

                //read in whole file as string
                string raw = tr.ReadToEnd();
                string[] rawSplit = raw.Split('\n');
                string[,] configuration = new string[rawSplit.Length, 2];
                string[] item;

                for(int i = 0; i < rawSplit.Length; i++){
                    try{
                        item = rawSplit[i].Split(':');
                        configuration[i, 0] = item[0];
                        configuration[i, 1] = item[1];
                    }
                    catch(Exception exx){}
                }

                //now, read in the settings
                for(int i = 0; i < configuration.Length/2; i++){
                    try{
                        //SOUND////////
                        if(configuration[i, 0].Equals("playBackgroundMusic")){
                            SoundManager.getInstance().PLAY_BG_MUSIC = Boolean.Parse(configuration[i, 1]);
                        }

                        else if (configuration[i, 0].Equals("audioPath")){
                            EntityConfiguration.FileConfig.audioPath = configuration[i, 1].Trim();
                        }

                        //ENTITY COST///
                        else if(configuration[i, 0].Equals("archerCost")){
                            EntityConfiguration.EntityPrices.COST_ARCHER = int.Parse(configuration[i, 1]);
                        }
                        else if (configuration[i, 0].Equals("lightInfantryCost"))
                        {
                            EntityConfiguration.EntityPrices.COST_LIGHT_INFANTRY = int.Parse(configuration[i, 1]);
                        }
                        else if (configuration[i, 0].Equals("heavyInfantryCost"))
                        {
                            EntityConfiguration.EntityPrices.COST_HEAVY_INFANTRY = int.Parse(configuration[i, 1]);
                        }
                        else if (configuration[i, 0].Equals("towerCost"))
                        {
                            EntityConfiguration.EntityPrices.COST_GUARD_TOWER = int.Parse(configuration[i, 1]);
                        }

                        //ENTITY STATS//

                        //ARCHER//
                        else if(configuration[i, 0].Equals("archerStats"))
                        {
                            //archerStats:<armor=1 damage=20 moveSpeed=150 attackSpeed=3 sightRange=12 attackRange=10 maxHealth=100 health=100>
                        }

                    }
                    catch(Exception ex){

                    }
                }

                //close the reader
                tr.Close();
            }
            catch (FileLoadException e)
            {
                Console.WriteLine("File Load Exception Found");
                Console.WriteLine(e);
            }

            //----------------------------------------//
            
            //Load XML Configurations
            //TODO: Store config locations somewhere...
            this.ReadConfigurationFile("../../../Configuration/TextureConfiguration.xml");
            this.ReadConfigurationFile("../../../Configuration/AudioConfiguration.xml");
            this.ReadConfigurationFile("../../../Configuration/EntityConfiguration.xml");
            
        }

        public void saveCurrentGame(String fileName)
        {
            //BIG TODO: REDO THIS!!
            try
            {
                ////Open up a writer
                TextWriter tw = new StreamWriter(fileName);
                
                ////write hero info and stats
                /*
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId)[0].name);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId)[0].level);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId)[0].experiencePoints);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId)[0].pointsToNextLevel);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId)[0].damage);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId)[0].armor);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(PlayerManager.getInstance().currentPlayerId)[0].maxHealth);
                tw.WriteLine(DateTime.Now);
                */
                ////Close the writer
                tw.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine("IO exception found");
                Console.WriteLine(e);
            }
        }

        public void loadGame(String fileName)
        {
            try
            {
                //open up a reader
                TextReader tr = new StreamReader(fileName);
                Hero hero = new Hero();

                //read the file and set hero info and stats
                hero.name = tr.ReadLine();
                hero.level = Convert.ToInt32(tr.ReadLine());
                hero.experiencePoints = Convert.ToInt32(tr.ReadLine());
                hero.pointsToNextLevel = Convert.ToInt32(tr.ReadLine());
                hero.damage = Convert.ToInt32(tr.ReadLine());
                hero.armor = Convert.ToInt32(tr.ReadLine());
                hero.maxHealth = Convert.ToInt32(tr.ReadLine());
                hero.health = hero.maxHealth;

                Incursio.getInstance().setHero(hero);
                //close the reader
                tr.Close();
            }
            catch (FileLoadException e)
            {
                Console.WriteLine("File Load Exception Found");
                Console.WriteLine(e);
            }
        }

        private void ReadConfigurationFile(string sFileName)
        {
            //Iterators
            int audioIterator = -1;
            int entityIterator = -1;
            int textureIterator = -1;

            //Lists for object factory
            List<BaseGameEntityConfiguration> entityList = new List<BaseGameEntityConfiguration>();
            List<TextureCollection> textureList = new List<TextureCollection>();
            
            try
            {
                //Loading the XML document
                XmlDocument doc = new XmlDocument();
                doc.Load(sFileName);

                //Flags
                bool readingAudio = false;
                bool readingEntities = false;
                bool readingTextures = false;
                

                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    if (node.Name == "AudioCollection")
                    {
                        readingAudio = true;
                        readingEntities = false;
                        readingTextures = false;
                    }
                    else if (node.Name == "Entity")
                    {
                        readingAudio = false;
                        readingEntities = true;
                        readingTextures = false;
                    }
                    else if (node.Name == "TextureCollection")
                    {
                        readingAudio = false;
                        readingEntities = false;
                        readingTextures = true;
                    }

                    if (readingAudio)
                    {
                        //Parsing audio code goes here

                        /******SOUND CONFIGURATION PARSING************************
                         * each audio collection node will have the node name 'AudioCollection', and will have one attribute, 'name'
                         * 
                         * For each of these AudioCollection nodes, a AudioCollection object can be instantiated using its name (& id)
                         * 
                         * Each AudioCollection node will consist of sub-nodes of (possible) types:
                         *      VoiceCollection
                         *      AttackCollection
                         *      AmbientCollection
                         *      MessageCollection
                         * 
                         * These can be added simpley by calling the method addSetOfType(string type) on the instantiated AudioCollection.
                         *      type is the same as the sub-node name (listed above)
                         * 
                         * addSetOfType will instantiate a AudioCollection's AudioSet of that type and return it so that we can add audio filenames.
                         * 
                         * Each of these sub-nodes can consist of various sub-sub-nodes.  They represent various types of sounds.
                         *      To assist initializing their properties, they all inherit from AudioSet, which provides
                         *      a method addSound(string type, string fileName).  
                         * 
                         * addSound will allow each set to sort the given data into different sound types.
                         * 
                         * Once the completed list of AudioCollections is created, they can be mapped to EntityConfigurations
                         * 
                         * The SoundBank will look for a particular AudioCollection, named "GameAudio."  This sets ambient music and message
                         *      sounds, and includes default filenames.  Technically this set can be omitted from the XML file.  If
                         *      no GameAudio AudioCollection exists, SoundBank can create a new one with the defaults.
                         *************************************************************************************************************************/

                    }
                    else if (readingEntities)
                    {
                        //Iterating the entityIterator for an accurate ID
                        entityIterator++;

                        //Getting specific attributes
                        XmlAttribute name = node.Attributes["name"];
                        string nameToSet = name.Value;
                        node.Attributes.Remove(name);

                        string[] entityAttributes = new string[node.Attributes.Count * 2];
                        
                        //Getting the number of attributes for the entity
                        int numOfAttributes = node.Attributes.Count;

                        //Looping over these attributes and adding them to the string array
                        for (int i = 0; i < numOfAttributes; i++)
                        {
                            XmlAttribute newAttribute = node.Attributes[i];
                            entityAttributes[i * 2] = newAttribute.Name;
                            entityAttributes[(i * 2) + 1] = newAttribute.Value;
                        }

                        //Building a new BaseGameEntityConfiguration
                        BaseGameEntityConfiguration entity = new BaseGameEntityConfiguration(entityIterator, nameToSet, entityAttributes);
                        
                        foreach (XmlNode entityNode in node.ChildNodes)
                        {
                            //Getting specific attributes
                            XmlAttribute compType = entityNode.Attributes["type"];
                            string typeToSet = compType.Value;
                            entityNode.Attributes.Remove(compType);

                            string[] componentAttributes = new string[entityNode.Attributes.Count * 2];

                            //Getting the number of attributes for the entity
                            int numOfAtt = entityNode.Attributes.Count;

                            //Looping over these attributes and adding them to the string array
                            for (int j = 0; j < numOfAtt; j++)
                            {
                                XmlAttribute newAttr = entityNode.Attributes[j];
                                componentAttributes[j * 2] = newAttr.Name;
                                componentAttributes[(j * 2) + 1] = newAttr.Value;
                            }
                            
                            //adding the component
                            entity.addComponentConfiguration(typeToSet, componentAttributes);
                        }

                        //adding the entity to the list
                        entityList.Add(entity);
                        
                    }
                    else if (readingTextures)
                    {
                        /******TEXTURE CONFIGURATION PARSING**************************************************************************************
                         * each texture node will have the node name 'TextureCollection', and will have one attribute, 'name'
                         * 
                         * For each of these TextureCollection nodes, a TextureCollection object can be instantiated using its name (& id)
                         * 
                         * Each TC node will consist of sub-nodes of (possible) types:
                         *      Still
                         *      Movement
                         *      Death
                         *      Damaged
                         *      Attacking
                         * 
                         * These can be added simpley by calling the method addSetOfType(string type) on the instantiated TextureCollection.
                         *      type is the same as the sub-node name (listed above)
                         * 
                         * addSetOfType will instantiate a TextureCollection's TextureSet of that type and return it so that we can add textures.
                         * 
                         * Each of these sub-nodes can consist of various sub-sub-nodes.  Generally they represent the 4 cardinal directions, but
                         *      this is not always so.  To assist initializing their properties, they all inherit from TextureSet, which provides
                         *      a method addTexture(string name, int fw, int fh), where fw and fh represent frameWidth and frameHeight, respectively.
                         * 
                         * addTexture will allow each set to parse all of their attribute data themselves.
                         * 
                         * Once the completed list of TextureCollections is created, we can add it to the TextureBank (textureCollections)
                         *************************************************************************************************************************/

                        //Iterating the entityIterator for an accurate ID
                        textureIterator++;

                        //Getting specific attributes
                        XmlAttribute name = node.Attributes["name"];
                        XmlAttribute portrait = node.Attributes["portrait"];
                        XmlAttribute icon = node.Attributes["icon"];
                        
                        string nameToSet = name.Value;
                        string portToSet = portrait.Value;
                        string iconToSet = icon.Value;
                        
                        node.Attributes.Remove(name);
                        node.Attributes.Remove(portrait);
                        node.Attributes.Remove(icon);

                        //Building a new TextureCollection
                        TextureCollection texture = new TextureCollection(textureIterator, nameToSet, portToSet, iconToSet);

                        foreach (XmlNode texNode in node.ChildNodes)
                        {
                            TextureSet set = texture.addSetOfType(texNode.Name);

                            if(set != null){
                                string nodeName, width, height;
                                XmlAttribute att;

                                foreach(XmlNode subNode in texNode.ChildNodes){
                                    try{
                                        nodeName = subNode.Attributes["file"].Value;

                                        att = subNode.Attributes["frameWidth"];
                                        width = (att == null ? "0" : att.Value.Length == 0 ? "0" : att.Value);

                                        att = subNode.Attributes["frameHeight"];
                                        height = (att == null ? "0" : att.Value.Length == 0 ? "0" : att.Value);

                                        set.addTexture(subNode.Name, nodeName, int.Parse(width), int.Parse(height));
                                    }
                                    catch(Exception e){
                                        Console.WriteLine("Skipping Texture Configuration #" + textureIterator + " because of:");
                                        Console.WriteLine(e.StackTrace);
                                    }
                                }
                            }

                        }

                        //adding the texture to the list
                        textureList.Add(texture);
                    }
                    else
                    {
                        //ERROR!!! this shoud not happen
                        Console.WriteLine("Unrecognizable format or value in parsing XML.");
                    }

                }
                //TODO: Set the AudioCollection "GameAudio" to SoundManager's AudioCollection property.
                //      if no GameAudio collection exists, pass a null to SoundManager.

                //TODO: MAP ENTITIES TO THEIR TEXTURES & AUDIO
                    //this could be done already if we store the list throughout the game..

                //TODO: ADD TEXTURES TO BANK
                    //do we really need to store these configurations if the entities have them already?
                    //perhaps when we load terrain textures from XML...but not entities

                //Adding the entity list to the object factory
                if(readingEntities)
                    ObjectFactory.getInstance().entities = entityList;
                else if(readingAudio){

                }
                else if (readingTextures)
                {
                    TextureBank.getInstance().textureCollections = textureList;
                }
            }
            catch (FileLoadException e)
            {
                Console.WriteLine("File Load Exception Found");
                Console.WriteLine(e);
            }
        }
    }
}

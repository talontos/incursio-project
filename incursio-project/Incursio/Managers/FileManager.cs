/****************************************
 * Copyright � 2008, Team RobotNinja:
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


using Incursio.Managers;
using Incursio.Utils;
using System.Xml;
using Incursio.Entities;
using Incursio.Entities.TextureCollections;
using Incursio.Entities.Components;
using Incursio.Entities.AudioCollections;
using Incursio.World;

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
            //Load XML Configurations
            this.ReadConfigurationFile("GameConfiguration.xml");
            this.ReadConfigurationFile(ConfigurationManager.getInstance().textureConfigurationFile);
            this.ReadConfigurationFile(ConfigurationManager.getInstance().audioConfigurationFile);
            this.ReadConfigurationFile(ConfigurationManager.getInstance().projectileConfigurationFile);
            this.ReadConfigurationFile(ConfigurationManager.getInstance().entityConfigurationFile);
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

        public void saveMap(){
            try{
                //TODO: DO IT
            }
            catch(IOException e){
                Console.WriteLine("IO exception jo0 c4n7 do 7h47!");
                Console.WriteLine(e);
            }
        }

        public void loadGame(String fileName)
        {
            try
            {
                //TODO: REIMPLEMENT THIS
                /*
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
                */
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
            int projectileIterator = -1;

            //Lists for object factory
            List<AudioCollection> audioList = new List<AudioCollection>();
            List<BaseGameEntityConfiguration> entityList = new List<BaseGameEntityConfiguration>();
            List<TextureCollection> textureList = new List<TextureCollection>();
            List<ProjectileConfiguration> projectileList = new List<ProjectileConfiguration>();
            
            try
            {
                //Loading the XML document
                XmlDocument doc = new XmlDocument();
                doc.Load(sFileName);

                //Flags
                bool readingAudio = false;
                bool readingEntities = false;
                bool readingProjectiles = false;
                bool readingTextures = false;
                bool readingGameConfig = false;

                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    readingAudio = (node.Name == "AudioCollection");
                    readingEntities = (node.Name == "Entity");
                    readingProjectiles = (node.Name == "Projectile");
                    readingTextures = (node.Name == "TextureCollection");
                    readingGameConfig = (node.Name == "Setting");

                    if(readingGameConfig){
                        #region GAME_CONFIG
                        
                        //Parsing the settings from xml
                        foreach (XmlAttribute att in node.Attributes)
                        {
                            ConfigurationManager.getInstance().setConfigurationSetting(att.Name, att.Value);
                        }

                        readingGameConfig = false;
                        #endregion
                    }
                    else if (readingAudio)
                    {
                        #region AUDIO
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

                        //Incrementing the interator to get an accurate id
                        audioIterator++;

                        //Getting specific attributes
                        XmlAttribute name = node.Attributes["name"];
                        string nameToSet = name.Value;
                        node.Attributes.Remove(name);

                        XmlAttribute fileNodeNameAtt;

                        //Building the new blank collection
                        AudioCollection newCollection = new AudioCollection(audioIterator, nameToSet);

                        //Parsing the new sets in the collection from xml
                        foreach (XmlNode audioChildNode in node.ChildNodes)
                        {
                            //Getting the type of collection to add
                            string type = audioChildNode.Name;

                            AudioSet newSet = newCollection.addSetOfType(type);

                            if (newSet != null)
                            {
                                foreach (XmlNode typeNode in audioChildNode.ChildNodes)
                                {
                                    string nodeType = typeNode.Name;

                                    foreach (XmlNode fileNode in typeNode.ChildNodes)
                                    {
                                        fileNodeNameAtt = fileNode.Attributes["name"];

                                        if(fileNodeNameAtt != null)
                                            newSet.addSound(nodeType, fileNodeNameAtt.Value);
                                    }
                                }
                            }
                        }

                        //Add the audio collection to the audio list
                        audioList.Add(newCollection);

                        //Set the flag back to false
                        readingAudio = false;
                        #endregion
                    }
                    else if (readingEntities)
                    {
                        #region ENTITIES
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
                        
                        //get component data
                        foreach (XmlNode entityNode in node.ChildNodes)
                        {
                            //Getting specific attributes
                            XmlAttribute compType = entityNode.Attributes["type"];
                            string typeToSet = compType.Value;
                            entityNode.Attributes.Remove(compType);

                            List<string> componentAttributes = new List<string>(entityNode.Attributes.Count * 2);

                            //Getting the number of attributes for the entity
                            int numOfAtt = entityNode.Attributes.Count;

                            //Looping over these attributes and adding them to the string array
                            for (int j = 0; j < numOfAtt; j++)
                            {
                                XmlAttribute newAttr = entityNode.Attributes[j];
                                componentAttributes.Add(newAttr.Name);
                                componentAttributes.Add(newAttr.Value);
                            }

                            //TODO: expand to allow for more than one type of attribute for subnodes!
                            if(entityNode.HasChildNodes){
                                numOfAtt = entityNode.ChildNodes.Count;
                                int numOfCompAtt = 0;
                                XmlNode compNode;
                                XmlAttribute compAtt;

                                for (int j = 0; j < numOfAtt; j++){
                                    compNode = entityNode.ChildNodes[j];
                                    numOfCompAtt = compNode.Attributes.Count;

                                    for (int c = 0; c < numOfCompAtt; c++){
                                        compAtt = compNode.Attributes[c];

                                        componentAttributes.Add(compNode.Name);
                                        componentAttributes.Add(compAtt.Value);
                                    }

                                }
                            }
                            
                            //adding the component
                            entity.addComponentConfiguration(typeToSet, componentAttributes.ToArray());
                        }

                        //adding the entity to the list
                        entityList.Add(entity);

                        //Setting the flag back to false
                        readingEntities = false;
                        #endregion
                    }
                    else if (readingTextures)
                    {
                        #region TEXTURES
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

                        //Setting the flag back to false
                        readingTextures = false;
                        #endregion
                    }
                    else if (readingProjectiles)
                    {
                        #region PROJECTILES

                        //Iterating the projectileIterator for an accurate ID
                        projectileIterator++;

                        //Getting specific attributes
                        XmlAttribute name = node.Attributes["name"];
                        string nameToSet = name.Value;
                        node.Attributes.Remove(name);

                        string[] projectileAttributes = new string[node.Attributes.Count * 2];

                        //Getting the number of attributes for the entity
                        int numOfAttributes = node.Attributes.Count;

                        //Looping over these attributes and adding them to the string array
                        for (int i = 0; i < numOfAttributes; i++)
                        {
                            XmlAttribute newAttribute = node.Attributes[i];
                            projectileAttributes[i * 2] = newAttribute.Name;
                            projectileAttributes[(i * 2) + 1] = newAttribute.Value;
                        }

                        //Building a new ProjectileConfiguration
                        ProjectileConfiguration projectile = new ProjectileConfiguration(projectileIterator, nameToSet, projectileAttributes);

                        //adding the projectile to the list
                        projectileList.Add(projectile);

                        //Setting the flag back to false
                        readingProjectiles= false;

                        #endregion
                    }
                    else
                    {
                        //ERROR!!! this shoud not happen
                        Console.WriteLine("Unrecognizable format or value in parsing XML.");
                    }

                }

                //Adding the entity list to the object factory
                if((entityList != null)&&(entityList.Count > 0)) 
                    ObjectFactory.getInstance().entities = entityList;

                //Adding the projectile list to the object factory
                if ((projectileList != null) && (projectileList.Count > 0))
                    ProjectileBank.getInstance().projectileConfigurations = projectileList;

                //Adding the audio list
                if ((audioList != null) && (audioList.Count > 0))
                    SoundCollection.getInstance().audioCollections = audioList;

                //Adding the texture list to the texture bank
                if ((textureList != null)&&(textureList.Count > 0))
                    TextureBank.getInstance().textureCollections = textureList;

                
            }
            catch (FileLoadException e)
            {
                Console.WriteLine("File Load Exception Found");
                Console.WriteLine(e);
            }
        }
    }
}

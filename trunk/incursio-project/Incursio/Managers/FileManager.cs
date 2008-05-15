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

            //Load XML Configurations
            this.ReadConfigurationFile("../../../Configuration/EntityConfiguration.xml");
            
        }

        public void saveCurrentGame(String fileName)
        {
            try
            {
                ////Open up a writer
                TextWriter tw = new StreamWriter(fileName);
                
                ////write hero info and stats
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].name);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].level);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].experiencePoints);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].pointsToNextLevel);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].damage);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].armor);
                tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].maxHealth);
                tw.WriteLine(DateTime.Now);
                
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
            int audioIterator = 0;
            int entityIterator = 0;
            int textureIterator = 0;

            //Lists for object factory
            List<BaseGameEntityConfiguration> entityList = new List<BaseGameEntityConfiguration>();
            
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
                    if (node.Name == "Audio")
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
                    else if (node.Name == "Texture")
                    {
                        readingAudio = false;
                        readingEntities = false;
                        readingTextures = true;
                    }

                    if (readingAudio)
                    {
                        //Parsing audio code goes here

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
                        //TODO: Parsing textures code goes here

                    }
                    else
                    {
                        //ERROR!!! this shoud not happen
                        Console.WriteLine("Unrecognizable format or value in parsing XML.");
                    }

                }
                //Adding the entity list to the object factory
                ObjectFactory.getInstance().entities = entityList;
            }
            catch (FileLoadException e)
            {
                Console.WriteLine("File Load Exception Found");
                Console.WriteLine(e);
            }

            BaseGameEntity en = ObjectFactory.getInstance().create(0, 0);
        }
    }
}

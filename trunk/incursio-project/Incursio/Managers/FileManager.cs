using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;

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
    }
}

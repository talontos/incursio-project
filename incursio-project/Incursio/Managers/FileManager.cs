using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Incursio.Classes;
using Incursio.Managers;

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
                    item = rawSplit[i].Split(':');
                    configuration[i, 0] = item[0];
                    configuration[i, 1] = item[1];
                }

                //now, read in the settings
                for(int i = 0; i < configuration.Length/2; i++){
                    try{
                        if(configuration[i, 0].Equals("playBackgroundMusic")){
                                SoundManager.getInstance().PLAY_BG_MUSIC = Boolean.Parse(configuration[i, 1]);
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

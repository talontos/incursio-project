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

        public void loadEntityConfiguration()
        {
            //Load entity configuration here
            //Not implemented in this version of the game
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

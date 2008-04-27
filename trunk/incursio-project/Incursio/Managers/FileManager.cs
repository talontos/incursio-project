using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using Incursio.Classes;
using Incursio.Managers;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Incursio.Managers
{
    public class FileManager
    {
        private static FileManager instance;

        private FileManager(){

        }

        public static FileManager getInstance(){
            if(instance == null)
                instance = new FileManager();
            
            return instance;
        }

        public void loadEntityConfiguration()
        {

        }

        public void saveCurrentGame()
        {
            try
            {
                    TextWriter tw = new StreamWriter("HeroSave.txt");
                    tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].name);
                    tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].level);
                    tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].experiencePoints);
                    tw.WriteLine(EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].pointsToNextLevel);
                    tw.WriteLine(DateTime.Now);
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
                //read file here
            }
            catch (FileLoadException e)
            {
                Console.WriteLine("File Load Exception Found");
                Console.WriteLine(e);
            }
        }
    }
}

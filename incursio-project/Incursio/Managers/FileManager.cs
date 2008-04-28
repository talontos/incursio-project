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

        public static void getInfo()
        {
            SaveGame saved = new SaveGame();

            saved.name = EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].name;
            saved.lvl = EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].level;
            saved.xp = EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].experiencePoints;
            saved.xpToGo = EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].pointsToNextLevel;
        }

        public void loadEntityConfiguration()
        {

        }

        public void saveCurrentGame(String fileName)
        {
            try
            {
                ////Open up a writer
                //TextWriter tw = new StreamWriter(fileName);
                ////write stuff to the file
                //tw.WriteLine(name);
                //tw.WriteLine(lvl);
                //tw.WriteLine(xp);
                //tw.WriteLine(xpToGo);
                //tw.WriteLine(DateTime.Now);
                ////Close the writer
                //tw.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine("IO exception found");
                Console.WriteLine(e);
            }
            catch (StorageDeviceNotConnectedException e)
            {
                Console.WriteLine("Storage device was not connected");
                Console.WriteLine(e);
            }
        }

        public void loadGame(String fileName)
        {
            try
            {
                //open up a reader
                TextReader tr = new StreamReader(fileName);
                //read the file and set the info
                EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].name = tr.ReadLine();
                EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].level = Convert.ToInt16(tr.ReadLine());
                EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].experiencePoints = Convert.ToInt32(tr.ReadLine());
                EntityManager.getInstance().getLivePlayerHeros(State.PlayerId.HUMAN)[0].pointsToNextLevel = Convert.ToInt32(tr.ReadLine());
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

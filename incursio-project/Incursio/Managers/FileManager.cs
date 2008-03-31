using System;
using System.Collections.Generic;
using System.Text;

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

        public void loadEntityConfiguration(){

        }

        public void saveCurrentGame(String fileName){

        }

        public void loadGame(String fileName){

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Incursio.Classes;
using Incursio.Managers;


namespace Incursio.Classes
{
    [Serializable]
    class SaveGame
    {
        public String name;
        public int lvl;
        public int xp;
        public int xpToGo;
    }
}

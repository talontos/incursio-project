using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities.AudioCollections
{
    public class AmbientCollection : AudioSet
    {
        public string main_menu = "Title.mp3";
        public string credits = "Thunderhorse.mp3";
        public string inPlay = "Film.mp3";
        public string victory = "Thunderhorse.mp3";
        public string defeat = "SadSong.mp3";

        public override void addSound(string type, string fileName)
        {
            switch (type)
            {
                case "main": main_menu = fileName; break;
                case "credits": credits = fileName; break;
                case "inPlay": inPlay = fileName; break;
                case "victory": victory = fileName; break;
                case "defeat": defeat = fileName; break;
                default: break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities.AudioCollections
{
    public class AudioCollection
    {
        public int id;
        public string name;

        public VoiceCollection voices;
        public AttackCollection attack;
        public AmbientCollection ambience;
        public MessageCollection messages;
        
        public AudioCollection(int id, string name){
            this.id = id;
            this.name = name;
        }

        public AudioSet addSetOfType(string type)
        {
            switch (type)
            {
                case "VoiceCollection": voices = new VoiceCollection(); return voices;
                case "AttackCollection": attack = new AttackCollection(); return attack;
                case "AmbientCollection": ambience = new AmbientCollection(); return ambience;
                case "MessageCollection": messages = new MessageCollection(); return messages;
                default: return null;
            }
        }
    }
}

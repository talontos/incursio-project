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
        public AmbientCollection ambiance;
        public MessageSoundCollection messages;

        public AudioCollection(int id, string name){
            this.id = id;
            this.name = name;
        }
    }
}

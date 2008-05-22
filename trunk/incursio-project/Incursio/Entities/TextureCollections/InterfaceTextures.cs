using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Entities.TextureCollections
{
    class InterfaceTextures : TextureSet
    {
        public GameTexture North;
        public GameTexture South;
        public GameTexture East;
        public GameTexture West;

        public override void addTexture(string type, string name, int frameWidth, int frameHeight)
        {
            base.addTexture(type, name, frameWidth, frameHeight);

            switch (type)
            {
                case "north": North = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "south": South = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "east": East = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "west": West = this.makeGameTexture(name, frameWidth, frameHeight); break;
                default: return;
            }
        }
    }
}

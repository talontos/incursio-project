using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Incursio.Utils;

namespace Incursio.Entities.TextureCollections
{
    public class StillTextures : TextureSet
    {
        public GameTexture North;
        public GameTexture South;
        public GameTexture East;
        public GameTexture West;
        public GameTexture Building;
        public GameTexture BeingBuilt;

        public override void addTexture(string type, string name, int frameWidth, int frameHeight)
        {
            base.addTexture(type, name, frameWidth, frameHeight);

            switch (type)
            {
                case "north": North = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "south": South = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "east": East = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "west": West = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "building": Building = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "beingBuilt": BeingBuilt = this.makeGameTexture(name, frameWidth, frameHeight); break;
                default: return;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Incursio.Utils;

namespace Incursio.Entities.TextureCollections
{
    public class DamagedTextures : TextureSet
    {
        public byte alphaChan = 255;
        public GameTexture damaged;
        public GameTexture destroyed;
        public GameTexture exploded;

        public override void addTexture(string type, string name, int frameWidth, int frameHeight)
        {
            base.addTexture(type, name, frameWidth, frameHeight);

            switch (type)
            {
                case "damaged": damaged = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "destroyed": destroyed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "exploded": exploded = this.makeGameTexture(name, frameWidth, frameHeight); break;
                default: return;
            }
        }
    }
}

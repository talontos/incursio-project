using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Entities.TextureCollections
{
    class InterfaceTextures : TextureSet
    {
        public GameTexture ButtonLeft;
        public GameTexture ButtonRight;
        public GameTexture ButtonBody;

        public override void addTexture(string type, string name, int frameWidth, int frameHeight)
        {
            base.addTexture(type, name, frameWidth, frameHeight);

            switch (type)
            {
                case "ButtonLeft":  ButtonLeft = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "ButtonRight": ButtonRight = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "ButtonBody":  ButtonBody = this.makeGameTexture(name, frameWidth, frameHeight); break;
                default: return;
            }
        }
    }
}

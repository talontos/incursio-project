using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Entities.TextureCollections
{
    public class TextureSet
    {
        public virtual void addTexture(string type, string name, int frameWidth, int frameHeight){

        }

        protected GameTexture makeGameTexture(string name, int fw, int fh){
            return new GameTexture(name, fw, fh);
        }
    }
}

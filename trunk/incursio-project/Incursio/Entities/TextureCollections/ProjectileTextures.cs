using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Entities.TextureCollections
{
    public class ProjectileTextures : TextureSet
    {
        public GameTexture Texture;

        public override void addTexture(string type, string name, int frameWidth, int frameHeight)
        {
            base.addTexture(type, name, frameWidth, frameHeight);

            Texture = this.makeGameTexture(name, frameWidth, frameHeight);
        }
    }
}

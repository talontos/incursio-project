using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Incursio.Utils;

namespace Incursio.Entities.TextureCollections
{
    public class DeathTextures : TextureSet
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
                case "North": North = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "South": South = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "East": East = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "West": West = this.makeGameTexture(name, frameWidth, frameHeight); break;
                default: return;
            }
        }
    }
}

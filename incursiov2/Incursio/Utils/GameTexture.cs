using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Utils
{
    public class GameTexture
    {
        public Texture2D texture;
        public int frameWidth = 0;
        public int frameHeight = 0;

        public GameTexture(Texture2D tex){
            this.texture = tex;
        }

        public GameTexture(string textureName){
            this.loadTexture(textureName);
            //TODO: Throw error if texture is null; image specified DNE
        }

        public GameTexture(string textureName, int frameWidth, int frameHeight) : this(textureName){
            this.frameWidth = (frameWidth == 0 ? texture.Width : frameWidth);
            this.frameHeight = (frameHeight == 0 ? texture.Height : frameHeight);
        }

        private void loadTexture(string name){
            this.texture = Texture2D.FromFile(Incursio.getInstance().spriteBatch.GraphicsDevice, 
                global::Incursio.Utils.EntityConfiguration.FileConfig.texturePath + name);
        }
    }
}

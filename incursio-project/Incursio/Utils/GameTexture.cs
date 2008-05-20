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
        }

        public GameTexture(string textureName, int frameWidth, int frameHeight) : this(textureName){
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
        }

        private void loadTexture(string name){
            //TODO: GET PATH FOR NAME
            this.texture = Texture2D.FromFile(Incursio.getInstance().spriteBatch.GraphicsDevice, 
            global::Incursio.Utils.EntityConfiguration.FileConfig.texturePath + name);
        }
    }
}

/****************************************
 * Copyright © 2008, Team RobotNinja:
 * 
 *     - Henry Armstrong
 *     - Andy Burras
 *     - Mitch Martin
 *     - Xuan Yu
 * 
 * All Rights Reserved
 ***************************************/

using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Managers
{
    class TextureManager
    {
        private static TextureManager instance;

        /// <summary>
        /// Loads a texture definition file, loading all appropriate textures into Content
        /// </summary>
        /// <param name="Content">Game's content manager</param>
        /// <returns></returns>
        public static TextureManager initializeTextureManager(ContentManager Content){
            instance = new TextureManager(Content);
            return instance;
        }

        public static TextureManager getInstance(){
            if(instance == null){
                //ERROR! TextureManager is not initialized!!!
            }
            return instance;
        }

        private TextureManager(ContentManager Content){
            TextureBank.getInstance().defaultPortrait = Content.Load<Texture2D>(@"lightInfantryPortrait");
            TextureBank.getInstance().defaultIcon = Content.Load<Texture2D>(@"InfantryIcon");
        }


    }
}

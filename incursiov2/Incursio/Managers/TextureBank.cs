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

using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Managers
{
    /// <summary>
    /// Class with static subclasses for use to store Texture references to avoid constant string-matching
    /// </summary>
    public class TextureBank
    {
        private static TextureBank instance;

        public Texture2D defaultPortrait;
        public Texture2D defaultIcon;

        public List<global::Incursio.Entities.TextureCollections.TextureCollection> textureCollections;

        private global::Incursio.Entities.TextureCollections.TextureCollection _terrain;
        private global::Incursio.Entities.TextureCollections.TextureCollection _interfaceTextures;

        public global::Incursio.Entities.TextureCollections.TextureCollection terrain{
            get { if (_terrain == null) _terrain = this.getCollectionByName("Terrain"); return _terrain; }
            set { _terrain = value; }
        }

        public global::Incursio.Entities.TextureCollections.TextureCollection InterfaceTextures{
            get { if (_interfaceTextures == null) _interfaceTextures = this.getCollectionByName("Interface"); return _interfaceTextures; }
            set { _interfaceTextures = value; }
        }

        private TextureBank(){
            this.textureCollections = new List<global::Incursio.Entities.TextureCollections.TextureCollection>();
        }

        public static TextureBank getInstance(){
            if(instance == null)
                instance = new TextureBank();

            return instance;
        }

        public global::Incursio.Entities.TextureCollections.TextureCollection getCollectionByName(string name){
            for(int i = 0; i < this.textureCollections.Count; i++){
                if (textureCollections[i].name.Equals(name))
                    return textureCollections[i];
            }

            return null;
        }
    }
}

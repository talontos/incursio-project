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
            //TODO: load texture definition file


            //TODO: Parse def'n file, and load Content into Content
            //  then store references in TextureBank

            //Load overlay textures
            TextureBank.EntityTextures.selectedUnitOverlayTexture = Content.Load<Texture2D>(@"selectedUnitOverlay");
            TextureBank.EntityTextures.healthRatioTexture = Content.Load<Texture2D>(@"healthBar");
            
            //Load Unit Textures
            TextureBank.EntityTextures.lightInfantryEast = Content.Load<Texture2D>(@"infantry_right");
            TextureBank.EntityTextures.lightInfantryWest = Content.Load<Texture2D>(@"infantry_left");
            TextureBank.EntityTextures.lightInfantrySouth = Content.Load<Texture2D>(@"infantry_still");
            TextureBank.EntityTextures.lightInfantryNorth = Content.Load<Texture2D>(@"infantry_back");
            TextureBank.EntityTextures.lightInfantryDead = Content.Load<Texture2D>(@"infantry_dead");

            TextureBank.EntityTextures.archerEast = Content.Load<Texture2D>(@"archer_right");
            TextureBank.EntityTextures.archerWest = Content.Load<Texture2D>(@"archer_left");
            TextureBank.EntityTextures.archerSouth = Content.Load<Texture2D>(@"archer_Still");
            TextureBank.EntityTextures.archerNorth = Content.Load<Texture2D>(@"archer_Back");
            TextureBank.EntityTextures.archerDead = Content.Load<Texture2D>(@"Archer_dead");

            //TODO: get hero textures
            //TextureBank.EntityTextures.heroEast = Content.Load<Texture2D>(@"");
            //TextureBank.EntityTextures.heroWest = Content.Load<Texture2D>(@"");
            //TextureBank.EntityTextures.heroSouth = Content.Load<Texture2D>(@"");
            //TextureBank.EntityTextures.heroNorth = Content.Load<Texture2D>(@"");

            //Load structure textures
            TextureBank.EntityTextures.campTexturePlayer = Content.Load<Texture2D>(@"Fort_friendly");
            TextureBank.EntityTextures.campTextureComputer = Content.Load<Texture2D>(@"Fort_hostile");
            TextureBank.EntityTextures.campTextureComputerDamaged = Content.Load<Texture2D>(@"Fort_hostile_damaged");
            TextureBank.EntityTextures.campTextureComputerDestroyed = Content.Load<Texture2D>(@"Fort_hostile_destroyed");
            TextureBank.EntityTextures.campTextureComputerExploded = Content.Load<Texture2D>(@"Fort_hostile_exploded");

            TextureBank.EntityTextures.guardTowerTexturePlayer = Content.Load<Texture2D>(@"Tower_friendly");
            TextureBank.EntityTextures.guardTowerTextureComputer = Content.Load<Texture2D>(@"Tower_hostile");

            //Load Interface textures
            TextureBank.InterfaceTextures.headsUpDisplay = Content.Load<Texture2D>(@"utilityBarUnderlay");
            TextureBank.InterfaceTextures.resourceDisplay = Content.Load<Texture2D>(@"resourceBarUnderlay");
            TextureBank.InterfaceTextures.cursor = Content.Load<Texture2D>(@"cursor");
            TextureBank.InterfaceTextures.cursorPressed = Content.Load<Texture2D>(@"cursor_click");
            TextureBank.InterfaceTextures.lightInfantryPortrait = Content.Load<Texture2D>(@"lightInfantryPortrait");
            TextureBank.InterfaceTextures.lightInfantryIcon = Content.Load<Texture2D>(@"InfantryIcon");
            TextureBank.InterfaceTextures.archerPortrait = Content.Load<Texture2D>(@"archerPortrait");
            TextureBank.InterfaceTextures.archerIcon = Content.Load<Texture2D>(@"archerIcon");
            TextureBank.InterfaceTextures.basePortrait = Content.Load<Texture2D>(@"BasePortrait");
            TextureBank.InterfaceTextures.guardTowerPortrait = Content.Load<Texture2D>(@"TowerPortrait");

        }


    }
}
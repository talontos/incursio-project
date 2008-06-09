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
            //Load overlay textures
            TextureBank.EntityTextures.selectedUnitOverlayTexture = Content.Load<Texture2D>(@"selectedUnitOverlay");
            TextureBank.EntityTextures.healthRatioTexture = Content.Load<Texture2D>(@"healthBar");
            
            TextureBank.EntityTextures.arrow = Content.Load<Texture2D>(@"Arrow");

            //Load Interface textures
            TextureBank.InterfaceTextures.headsUpDisplay = Content.Load<Texture2D>(@"utilityBarUnderlay");
            TextureBank.InterfaceTextures.resourceDisplay = Content.Load<Texture2D>(@"resourceBarUnderlay");
            TextureBank.InterfaceTextures.moneyIcon = Content.Load<Texture2D>(@"moneyIcon");
            TextureBank.InterfaceTextures.cursor = Content.Load<Texture2D>(@"cursor");
            TextureBank.InterfaceTextures.cursorPressed = Content.Load<Texture2D>(@"cursor_click");
            
            TextureBank.InterfaceTextures.controlPointInterfaceIcon = Content.Load<Texture2D>(@"ControlPointInterfaceIcon");
            
            // load paused game menu components

            TextureBank.InterfaceTextures.gameMenuButton = Content.Load<Texture2D>(@"gameMenuButton");
            TextureBank.InterfaceTextures.gameMenuButtonPressed = Content.Load<Texture2D>(@"gameMenuButtonPressed");
            TextureBank.InterfaceTextures.resumeGameButton = Content.Load<Texture2D>(@"resumeButton");
            TextureBank.InterfaceTextures.resumeGameButtonPressed = Content.Load<Texture2D>(@"resumeButtonPressed");
            TextureBank.InterfaceTextures.exitGameToMenuButton = Content.Load<Texture2D>(@"exitGameButton");
            TextureBank.InterfaceTextures.exitGameToMenuButtonPressed = Content.Load<Texture2D>(@"exitGameButtonPressed");
            TextureBank.InterfaceTextures.newGameButton = Content.Load<Texture2D>(@"newGameButton");
            TextureBank.InterfaceTextures.newGameButtonPressed = Content.Load<Texture2D>(@"newGamePressed");
            TextureBank.InterfaceTextures.exitGameButton = Content.Load<Texture2D>(@"exitGameButton");
            TextureBank.InterfaceTextures.exitGameButtonPressed = Content.Load<Texture2D>(@"exitGameButtonPressed");
            TextureBank.InterfaceTextures.saveGameButton = Content.Load<Texture2D>(@"saveButton");
            TextureBank.InterfaceTextures.saveGameButtonPressed = Content.Load<Texture2D>(@"saveButtonPressed");
            TextureBank.InterfaceTextures.loadGameButton = Content.Load<Texture2D>(@"loadButton");
            TextureBank.InterfaceTextures.loadGameButtonPressed = Content.Load<Texture2D>(@"loadButtonPressed");
            TextureBank.InterfaceTextures.creditsButton_not_pressed = Content.Load<Texture2D>(@"Credits_button");
            TextureBank.InterfaceTextures.creditsButton_pressed = Content.Load<Texture2D>(@"Credit_button_pressed");


            TextureBank.InterfaceTextures.instructionsButton_pressed = Content.Load<Texture2D>(@"instruction_button_pressed");
            TextureBank.InterfaceTextures.instructionsButton_not_pressed = Content.Load<Texture2D>(@"instructionButton");
            TextureBank.InterfaceTextures.instructions = Content.Load<Texture2D>(@"instruction");

            TextureBank.InterfaceTextures.portMap_pressed = Content.Load<Texture2D>(@"port_map_button");
            TextureBank.InterfaceTextures.portMap_not_pressed = Content.Load<Texture2D>(@"port_map_button_pressed");
            TextureBank.InterfaceTextures.inlandMap_pressed = Content.Load<Texture2D>(@"inland_button");
            TextureBank.InterfaceTextures.inlandMap_not_pressed = Content.Load<Texture2D>(@"inland_button_pressed");
            TextureBank.InterfaceTextures.capitalMap_pressed = Content.Load<Texture2D>(@"capital_button");
            TextureBank.InterfaceTextures.capitalMap_not_pressed = Content.Load<Texture2D>(@"capital_button_press");

            TextureBank.InterfaceTextures.file1_not_pressed = Content.Load<Texture2D>(@"file1_button");
            TextureBank.InterfaceTextures.file1_pressed = Content.Load<Texture2D>(@"file1_button_pressed");
            TextureBank.InterfaceTextures.file2_not_pressed = Content.Load<Texture2D>(@"file2_button");
            TextureBank.InterfaceTextures.file2_pressed = Content.Load<Texture2D>(@"file2_button_pressed");
            TextureBank.InterfaceTextures.file3_not_pressed = Content.Load<Texture2D>(@"file3_button");
            TextureBank.InterfaceTextures.file3_pressed = Content.Load<Texture2D>(@"file3_button_pressed");

            TextureBank.InterfaceTextures.selectionRectangle = Content.Load<Texture2D>(@"DragSelection");

            // load background Images
            TextureBank.InterfaceTextures.mainMenuBackground = Content.Load<Texture2D>(@"Splash_Screen");
            TextureBank.InterfaceTextures.pauseMenuBackground = Content.Load<Texture2D>(@"Pause_Screen");
            TextureBank.InterfaceTextures.victoryMenuBackground = Content.Load<Texture2D>(@"win");
            TextureBank.InterfaceTextures.defeatMenuBackground = Content.Load<Texture2D>(@"Defeat");
            TextureBank.InterfaceTextures.creditsBackground = Content.Load<Texture2D>(@"Credits");
        }


    }
}

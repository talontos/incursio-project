using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Entities.TextureCollections
{
    public class InterfaceTextures : TextureSet
    {
        #region TEXTURES
        public GameTexture ButtonLeft;
        public GameTexture ButtonRight;
        public GameTexture ButtonBody;

        public GameTexture selectedUnitOverlayTexture;
        public GameTexture healthRatioTexture;

        public GameTexture arrow;

        public GameTexture headsUpDisplay;
        public GameTexture cursor;
        public GameTexture cursorPressed;
        public GameTexture cursorEvent;
        public GameTexture resourceDisplay;
        public GameTexture moneyIcon;
        public GameTexture controlPointInterfaceIcon;

        public GameTexture selectionRectangle;

        public GameTexture gameMenuButton_pressed;
        public GameTexture gameMenuButton_not_pressed;
        public GameTexture resumeGameButton_pressed;
        public GameTexture resumeGameButton_not_pressed;
        public GameTexture exitGameToMenuButton_pressed;
        public GameTexture exitGameToMenuButton_not_pressed;
        public GameTexture newGameButton_pressed;
        public GameTexture newGameButton_not_pressed;
        public GameTexture exitGameButton_pressed;
        public GameTexture exitGameButton_not_pressed;

        public GameTexture file1_pressed;
        public GameTexture file1_not_pressed;
        public GameTexture file2_pressed;
        public GameTexture file2_not_pressed;
        public GameTexture file3_pressed;
        public GameTexture file3_not_pressed;

        public GameTexture creditsButton_pressed;
        public GameTexture creditsButton_not_pressed;

        public GameTexture instructionsButton_pressed;
        public GameTexture instructionsButton_not_pressed;
        public GameTexture instructions;

        public GameTexture portMap_pressed;
        public GameTexture portMap_not_pressed;
        public GameTexture inlandMap_pressed;
        public GameTexture inlandMap_not_pressed;
        public GameTexture capitalMap_pressed;
        public GameTexture capitalMap_not_pressed;

        public GameTexture gameMenuButton;
        public GameTexture gameMenuButtonPressed;
        public GameTexture resumeGameButton;
        public GameTexture resumeGameButtonPressed;
        public GameTexture exitGameToMenuButton;
        public GameTexture exitGameToMenuButtonPressed;
        public GameTexture newGameButton;
        public GameTexture newGameButtonPressed;
        public GameTexture exitGameButton;
        public GameTexture exitGameButtonPressed;
        public GameTexture saveGameButton;
        public GameTexture saveGameButtonPressed;
        public GameTexture loadGameButton;
        public GameTexture loadGameButtonPressed;

        public GameTexture mainMenuBackground;
        public GameTexture pauseMenuBackground;
        public GameTexture victoryMenuBackground;
        public GameTexture defeatMenuBackground;
        public GameTexture creditsBackground;
        
        #endregion

        public override void addTexture(string type, string name, int frameWidth, int frameHeight)
        {
            base.addTexture(type, name, frameWidth, frameHeight);

            switch (type)
            {
                case "ButtonLeft":  ButtonLeft = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "ButtonRight": ButtonRight = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "ButtonBody":  ButtonBody = this.makeGameTexture(name, frameWidth, frameHeight); break;

                case "selectedUnitOverlayTexture": selectedUnitOverlayTexture = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "healthRatioTexture": healthRatioTexture = this.makeGameTexture(name, frameWidth, frameHeight); break;

                case "arrow": arrow = this.makeGameTexture(name, frameWidth, frameHeight); break;

                case "headsUpDisplay": headsUpDisplay = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "cursor": cursor = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "cursorPressed": cursorPressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "cursorEvent": cursorEvent = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "resourceDisplay": resourceDisplay = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "moneyIcon": moneyIcon = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "controlPointInterfaceIcon": this.controlPointInterfaceIcon= this.makeGameTexture(name, frameWidth, frameHeight); break;

                case "selectionRectangle": selectionRectangle = this.makeGameTexture(name, frameWidth, frameHeight); break;

                /*
                case "gameMenuButton_pressed": gameMenuButton_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "gameMenuButton_not_pressed": gameMenuButton_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "resumeGameButton_pressed": resumeGameButton_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "resumeGameButton_not_pressed": resumeGameButton_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "exitGameToMenuButton_pressed": exitGameToMenuButton_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "exitGameToMenuButton_not_pressed": exitGameToMenuButton_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "newGameButton_pressed": newGameButton_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "newGameButton_not_pressed": newGameButton_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "exitGameButton_pressed": exitGameButton_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "exitGameButton_not_pressed": exitGameButton_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                */

                case "file1_pressed": file1_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "file1_not_pressed": file1_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "file2_pressed": file2_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "file2_not_pressed": file2_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "file3_pressed": file3_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "file3_not_pressed": file3_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;

                case "creditsButton_pressed": creditsButton_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "creditsButton_not_pressed": creditsButton_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;

                case "instructionsButton_pressed": instructionsButton_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "instructionsButton_not_pressed": instructionsButton_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "instructions": instructions = this.makeGameTexture(name, frameWidth, frameHeight); break;

                case "portMap_pressed": portMap_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "portMap_not_pressed": portMap_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "inlandMap_pressed": inlandMap_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "inlandMap_not_pressed": inlandMap_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "capitalMap_pressed": capitalMap_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "capitalMap_not_pressed": capitalMap_not_pressed = this.makeGameTexture(name, frameWidth, frameHeight); break;

                case "gameMenuButton": gameMenuButton = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "gameMenuButtonPressed": gameMenuButtonPressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "resumeGameButton": resumeGameButton = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "resumeGameButtonPressed": resumeGameButtonPressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "exitGameToMenuButton": exitGameToMenuButton = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "exitGameToMenuButtonPressed": exitGameToMenuButtonPressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "newGameButton": newGameButton = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "newGameButtonPressed": newGameButtonPressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "exitGameButton": exitGameButton = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "exitGameButtonPressed": exitGameButtonPressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "saveGameButton": saveGameButton = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "saveGameButtonPressed": saveGameButtonPressed = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "loadGameButton": loadGameButton = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "loadGameButtonPressed": loadGameButtonPressed = this.makeGameTexture(name, frameWidth, frameHeight); break;

                case "mainMenuBackground": mainMenuBackground = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "pauseMenuBackground": pauseMenuBackground = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "victoryMenuBackground": victoryMenuBackground = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "defeatMenuBackground": defeatMenuBackground = this.makeGameTexture(name, frameWidth, frameHeight); break;
                case "creditsBackground": creditsBackground = this.makeGameTexture(name, frameWidth, frameHeight); break;

                default: return;
            }
        }
    }
}

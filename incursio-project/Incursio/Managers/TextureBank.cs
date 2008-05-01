using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Managers
{
    /// <summary>
    /// Class with 2 static subclasses.  For use to store Texture references to avoid constant string-matching
    /// </summary>
    public class TextureBank
    {
        public static class EntityTextures{

            public static Texture2D selectedUnitOverlayTexture;
            public static Texture2D healthRatioTexture;

            //Unit Textures//////////////
            public static Texture2D lightInfantryEast;
            public static Texture2D lightInfantryWest;
            public static Texture2D lightInfantrySouth;
            public static Texture2D lightInfantryNorth;
            public static Texture2D lightInfantryDead;
            public static Texture2D lightInfantryMovingEast;
            public static Texture2D lightInfantryMovingWest;
            public static Texture2D lightInfantryMovingSouth;
            public static Texture2D lightInfantryMovingNorth;
            public static Texture2D lightInfantryAttackingEast;
            public static Texture2D lightInfantryAttackingWest;
            public static Texture2D lightInfantryDeathEast;
            public static Texture2D lightInfantryDeathWest;

            public static Texture2D heavyInfantryEast;
            public static Texture2D heavyInfantryWest;
            public static Texture2D heavyInfantrySouth;
            public static Texture2D heavyInfantryNorth;
            public static Texture2D heavyInfantryMovingEast;
            public static Texture2D heavyInfantryMovingWest;
            public static Texture2D heavyInfantryMovingSouth;
            public static Texture2D heavyInfantryMovingNorth;
            public static Texture2D heavyInfantryAttackingEast;
            public static Texture2D heavyInfantryAttackingWest;
            public static Texture2D heavyInfantryDeath_East;
            public static Texture2D heavyInfantryDeath_West;

            public static Texture2D archerEast;
            public static Texture2D archerWest;
            public static Texture2D archerSouth;
            public static Texture2D archerNorth;
            public static Texture2D archerDead;
            public static Texture2D archerMovingWest;
            public static Texture2D archerMovingEast;
            public static Texture2D archerMovingSouth;
            public static Texture2D archerMovingNorth;
            public static Texture2D archerAttackingEast;
            public static Texture2D archerAttackingWest;
            public static Texture2D archerDeathEast;
            public static Texture2D archerDeathWest;

            public static Texture2D arrow;

            public static Texture2D heroEast;
            public static Texture2D heroWest;
            public static Texture2D heroSouth;
            public static Texture2D heroNorth;
            public static Texture2D heroMovingEast;
            public static Texture2D heroMovingWest;
            public static Texture2D heroMovingSouth;
            public static Texture2D heroMovingNorth;
            public static Texture2D heroAttackingEast;
            public static Texture2D heroAttackingWest;
            public static Texture2D heroDeathEast;
            public static Texture2D heroDeathWest;
            /////////////////////////////

            //Structure Textures/////////
            public static Texture2D campTexturePlayer;
            public static Texture2D campTexturePlayerBuilding;
            public static Texture2D campTextureComputer;
            public static Texture2D campTextureComputerDamaged;
            public static Texture2D campTextureComputerDestroyed;
            public static Texture2D campTextureComputerExploded;

            public static Texture2D guardTowerTexturePlayer;
            public static Texture2D guardTowerTextureComputer;

            public static Texture2D controlPointPlayer;
            public static Texture2D controlPointComputer;
            /////////////////////////////

        }

        public static class MapTiles{
            public static Texture2D grass1;
            //...

            public static Texture2D shore1;
            //...

            public static Texture2D openWater;
            public static Texture2D shoreDown;
            public static Texture2D shoreLeft;
            public static Texture2D shoreRight;
            public static Texture2D shoreUp;
            public static Texture2D shoreLowerLeftCorner;
            public static Texture2D shoreLowerRightCorner;
            public static Texture2D shoreUpperLeftCorner;
            public static Texture2D shoreUpperRightCorner;
            public static Texture2D shoreOpenLowerLeftCorner;
            public static Texture2D shoreOpenLowerRightCorner;
            public static Texture2D shoreOpenUpperLeftCorner;
            public static Texture2D shoreOpenUpperRightCorner;
            //...

            public static Texture2D tree1;
            public static Texture2D groupOfTrees;
            //...
            public static Texture2D roadHorizontal;
            public static Texture2D roadVertical;
            public static Texture2D roadElbowUpRight;
            public static Texture2D roadElbowUpLeft;
            public static Texture2D roadElbowDownRight;
            public static Texture2D roadElbowDownLeft;

            //...
            public static Texture2D rockSmall;
            public static Texture2D rockMedium;
            public static Texture2D rockBig;
            public static Texture2D dock;
            public static Texture2D building1;
            public static Texture2D building2;
            public static Texture2D building3;
            public static Texture2D buildingGroup;
            public static Texture2D buildingGroupEndRight;
            public static Texture2D buildingGroupEndLeft;

        }

        public static class InterfaceTextures
        {
            //Interface Textures/////////
            public static Texture2D headsUpDisplay;
            public static Texture2D cursor;
            public static Texture2D cursorPressed;
            public static Texture2D cursorEvent;
            public static Texture2D resourceDisplay;
            public static Texture2D moneyIcon;

            public static Texture2D selectionRectangle;

            public static Texture2D gameMenuButton_pressed;
            public static Texture2D gameMenuButton_not_pressed;
            public static Texture2D resumeGameButton_pressed;
            public static Texture2D resumeGameButton_not_pressed;
            public static Texture2D exitGameToMenuButton_pressed;
            public static Texture2D exitGameToMenuButton_not_pressed;
            public static Texture2D newGameButton_pressed;
            public static Texture2D newGameButton_not_pressed;
            public static Texture2D exitGameButton_pressed;
            public static Texture2D exitGameButton_not_pressed;

            public static Texture2D file1_pressed;
            public static Texture2D file1_not_pressed;
            public static Texture2D file2_pressed;
            public static Texture2D file2_not_pressed;
            public static Texture2D file3_pressed;
            public static Texture2D file3_not_pressed;

            public static Texture2D creditsButton_pressed;
            public static Texture2D creditsButton_not_pressed;

            public static Texture2D instructionsButton_pressed;
            public static Texture2D instructionsButton_not_pressed;
            public static Texture2D instructions;

            public static Texture2D portMap_pressed;
            public static Texture2D portMap_not_pressed;
            public static Texture2D inlandMap_pressed;
            public static Texture2D inlandMap_not_pressed;
            public static Texture2D capitalMap_pressed;
            public static Texture2D capitalMap_not_pressed;

            /////////////////////////////

            //Portraits/Icons////////////
            public static Texture2D gameMenuButton;
            public static Texture2D gameMenuButtonPressed;
            public static Texture2D resumeGameButton;
            public static Texture2D resumeGameButtonPressed;
            public static Texture2D exitGameToMenuButton;
            public static Texture2D exitGameToMenuButtonPressed;
            public static Texture2D newGameButton;
            public static Texture2D newGameButtonPressed;
            public static Texture2D exitGameButton;
            public static Texture2D exitGameButtonPressed;
            public static Texture2D saveGameButton;
            public static Texture2D saveGameButtonPressed;
            public static Texture2D loadGameButton;
            public static Texture2D loadGameButtonPressed;

            public static Texture2D archerPortrait;
            public static Texture2D archerIcon;
            public static Texture2D lightInfantryPortrait;
            public static Texture2D lightInfantryIcon;
            public static Texture2D heavyInfantryPortrait;
            public static Texture2D heavyInfantryIcon;
            public static Texture2D basePortrait;
            public static Texture2D guardTowerPortrait;
            public static Texture2D guardTowerIcon;
            public static Texture2D controlPointPortrait;
            public static Texture2D controlPointInterfaceIcon;
            public static Texture2D heroIcon;
            public static Texture2D heroPortrait;
            /////////////////////////////

            /////Background Images///////
            public static Texture2D mainMenuBackground;
            public static Texture2D pauseMenuBackground;
            public static Texture2D victoryMenuBackground;
            public static Texture2D defeatMenuBackground;

        }
    }
}

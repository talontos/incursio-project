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
            public static Texture2D lightInfantryMovingWest;
            public static Texture2D lightInfantryMovingEast;

            public static Texture2D heavyInfantryEast;
            public static Texture2D heavyInfantryWest;
            public static Texture2D heavyInfantrySouth;
            public static Texture2D heavyInfantryNorth;
            public static Texture2D heavyInfantryMovingEast;

            public static Texture2D archerEast;
            public static Texture2D archerWest;
            public static Texture2D archerSouth;
            public static Texture2D archerNorth;
            public static Texture2D archerDead;
            public static Texture2D archerMovingWest;
            public static Texture2D archerMovingEast;

            public static Texture2D heroEast;
            public static Texture2D heroWest;
            public static Texture2D heroSouth;
            public static Texture2D heroNorth;
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
            public static Texture2D grass;
            //...

            public static Texture2D shore1;
            //...

            public static Texture2D water1;
            //...

            public static Texture2D tree1;
            //...

            //...
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
            /////////////////////////////
        }
    }
}

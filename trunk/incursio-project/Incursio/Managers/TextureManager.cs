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
            TextureBank.EntityTextures.lightInfantryMovingEast = Content.Load<Texture2D>(@"Infantry_Moving_Right");
            TextureBank.EntityTextures.lightInfantryMovingWest = Content.Load<Texture2D>(@"Infantry_Moving_Left");
            TextureBank.EntityTextures.lightInfantryMovingNorth = Content.Load<Texture2D>(@"Infantry_Moving_North");
            TextureBank.EntityTextures.lightInfantryMovingSouth = Content.Load<Texture2D>(@"Infantry_Moving_South");
            TextureBank.EntityTextures.lightInfantryAttackingEast = Content.Load<Texture2D>(@"Infantry_Attacking_East");
            TextureBank.EntityTextures.lightInfantryAttackingWest = Content.Load<Texture2D>(@"Infantry_Attacking_West");
            TextureBank.EntityTextures.lightInfantryDeathEast = Content.Load<Texture2D>(@"LightInfantry_Death_Right");
            TextureBank.EntityTextures.lightInfantryDeathWest = Content.Load<Texture2D>(@"LightInfantry_Death_Left");

            TextureBank.EntityTextures.heavyInfantryEast = Content.Load<Texture2D>(@"HeavyInfantryEast");
            TextureBank.EntityTextures.heavyInfantryWest = Content.Load<Texture2D>(@"HeavyInfantryWest");
            TextureBank.EntityTextures.heavyInfantryNorth = Content.Load<Texture2D>(@"HeavyInfantryNorth");
            TextureBank.EntityTextures.heavyInfantrySouth = Content.Load<Texture2D>(@"HeavyInfantrySouth");
            TextureBank.EntityTextures.heavyInfantryMovingEast = Content.Load<Texture2D>(@"HeavyInfantry_Moving_Right");
            TextureBank.EntityTextures.heavyInfantryMovingWest = Content.Load<Texture2D>(@"HeavyInfantry_Moving_Left");
            TextureBank.EntityTextures.heavyInfantryMovingSouth = Content.Load<Texture2D>(@"HeavyInfantry_Moving_Down");
            TextureBank.EntityTextures.heavyInfantryMovingNorth = Content.Load<Texture2D>(@"HeavyInfantry_Moving_Up");
            TextureBank.EntityTextures.heavyInfantryAttackingEast = Content.Load<Texture2D>(@"HeavyInfantry_Attacking_East");
            TextureBank.EntityTextures.heavyInfantryAttackingWest = Content.Load<Texture2D>(@"HeavyInfantry_Attacking_West");
            TextureBank.EntityTextures.heavyInfantryDeath_East = Content.Load<Texture2D>(@"HeavyInfantry_Death");
            TextureBank.EntityTextures.heavyInfantryDeath_West = Content.Load<Texture2D>(@"HeavyInfantry_Death_Left");


            TextureBank.EntityTextures.archerEast = Content.Load<Texture2D>(@"archer_right");
            TextureBank.EntityTextures.archerWest = Content.Load<Texture2D>(@"archer_left");
            TextureBank.EntityTextures.archerSouth = Content.Load<Texture2D>(@"archer_Still");
            TextureBank.EntityTextures.archerNorth = Content.Load<Texture2D>(@"archer_Back");
            TextureBank.EntityTextures.archerDead = Content.Load<Texture2D>(@"Archer_dead");
            TextureBank.EntityTextures.archerMovingWest = Content.Load<Texture2D>(@"Archer_Moving_Left");
            TextureBank.EntityTextures.archerMovingEast = Content.Load<Texture2D>(@"Archer_Moving_Right");
            TextureBank.EntityTextures.archerMovingNorth = Content.Load<Texture2D>(@"Archer_Moving_Up");
            TextureBank.EntityTextures.archerMovingSouth = Content.Load<Texture2D>(@"Archer_Moving_Down");
            TextureBank.EntityTextures.archerAttackingEast = Content.Load<Texture2D>(@"Archer_Attacking_East");
            TextureBank.EntityTextures.archerAttackingWest = Content.Load<Texture2D>(@"Archer_Attacking_West");
            TextureBank.EntityTextures.archerDeathEast = Content.Load<Texture2D>(@"Archer_Death_Right");
            TextureBank.EntityTextures.archerDeathWest = Content.Load<Texture2D>(@"Archer_Death_Left");

            TextureBank.EntityTextures.arrow = Content.Load<Texture2D>(@"Arrow");
            
            TextureBank.EntityTextures.heroEast = Content.Load<Texture2D>(@"HeroEast");
            TextureBank.EntityTextures.heroWest = Content.Load<Texture2D>(@"HeroWest");
            TextureBank.EntityTextures.heroSouth = Content.Load<Texture2D>(@"HeroStill");
            TextureBank.EntityTextures.heroNorth = Content.Load<Texture2D>(@"Hero_North");
            TextureBank.EntityTextures.heroMovingEast = Content.Load<Texture2D>(@"Hero_Moving_Right");
            TextureBank.EntityTextures.heroMovingWest = Content.Load<Texture2D>(@"Hero_Moving_Left");
            TextureBank.EntityTextures.heroMovingSouth = Content.Load<Texture2D>(@"Hero_Moving_South");
            TextureBank.EntityTextures.heroMovingNorth = Content.Load<Texture2D>(@"Hero_Moving_North");
            TextureBank.EntityTextures.heroAttackingEast = Content.Load<Texture2D>(@"Hero_Attacking_Right");
            TextureBank.EntityTextures.heroAttackingWest = Content.Load<Texture2D>(@"Hero_Attacking_Left");
            TextureBank.EntityTextures.heroDeathEast = Content.Load<Texture2D>(@"Hero_Death_Right");
            TextureBank.EntityTextures.heroDeathWest = Content.Load<Texture2D>(@"Hero_Death_Left");

            //Load structure textures
            TextureBank.EntityTextures.campTexturePlayer = Content.Load<Texture2D>(@"Fort_friendly");
            TextureBank.EntityTextures.campTexturePlayerBuilding = Content.Load<Texture2D>(@"Fort_Friendly_Building");
            TextureBank.EntityTextures.campTextureComputer = Content.Load<Texture2D>(@"Fort_hostile");
            TextureBank.EntityTextures.campTextureComputerDamaged = Content.Load<Texture2D>(@"Fort_hostile_damaged");
            TextureBank.EntityTextures.campTextureComputerDestroyed = Content.Load<Texture2D>(@"Fort_hostile_destroyed");
            TextureBank.EntityTextures.campTextureComputerExploded = Content.Load<Texture2D>(@"Fort_hostile_exploded");

            TextureBank.EntityTextures.guardTowerTexturePlayer = Content.Load<Texture2D>(@"Tower_friendly");
            TextureBank.EntityTextures.guardTowerTextureComputer = Content.Load<Texture2D>(@"Tower_hostile");
            TextureBank.EntityTextures.guardTowerExploded = Content.Load<Texture2D>(@"Tower_exploded");

            TextureBank.EntityTextures.controlPointPlayer = Content.Load<Texture2D>(@"ControlPointFriendly");
            TextureBank.EntityTextures.controlPointComputer = Content.Load<Texture2D>(@"ControlPointEnemy");

            //Load Interface textures
            TextureBank.InterfaceTextures.headsUpDisplay = Content.Load<Texture2D>(@"utilityBarUnderlay");
            TextureBank.InterfaceTextures.resourceDisplay = Content.Load<Texture2D>(@"resourceBarUnderlay");
            TextureBank.InterfaceTextures.moneyIcon = Content.Load<Texture2D>(@"moneyIcon");
            TextureBank.InterfaceTextures.cursor = Content.Load<Texture2D>(@"cursor");
            TextureBank.InterfaceTextures.cursorPressed = Content.Load<Texture2D>(@"cursor_click");
            TextureBank.InterfaceTextures.lightInfantryPortrait = Content.Load<Texture2D>(@"lightInfantryPortrait");
            TextureBank.InterfaceTextures.lightInfantryIcon = Content.Load<Texture2D>(@"InfantryIcon");
            TextureBank.InterfaceTextures.archerPortrait = Content.Load<Texture2D>(@"archerPortrait");
            TextureBank.InterfaceTextures.archerIcon = Content.Load<Texture2D>(@"archerIcon");
            TextureBank.InterfaceTextures.basePortrait = Content.Load<Texture2D>(@"BasePortrait");
            TextureBank.InterfaceTextures.baseEnemyPortrait = Content.Load<Texture2D>(@"BasePortrait_hostile");
            TextureBank.InterfaceTextures.guardTowerPortrait = Content.Load<Texture2D>(@"TowerPortrait");
            TextureBank.InterfaceTextures.guardTowerIcon = Content.Load<Texture2D>(@"TowerIcon");
            TextureBank.InterfaceTextures.controlPointPortrait = Content.Load<Texture2D>(@"ControlPointIcon");
            TextureBank.InterfaceTextures.heavyInfantryIcon = Content.Load<Texture2D>(@"HeavyInfantryIcon");
            TextureBank.InterfaceTextures.heavyInfantryPortrait = Content.Load<Texture2D>(@"HeavyInfantryPortrait");
            TextureBank.InterfaceTextures.heroIcon = Content.Load<Texture2D>(@"HeroIcon");
            TextureBank.InterfaceTextures.heroPortrait = Content.Load<Texture2D>(@"HeroPortrait");
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
            
            //Load terrain textures
            TextureBank.MapTiles.grass1 = Content.Load<Texture2D>(@"GrassTile1");
            TextureBank.MapTiles.tree1 = Content.Load<Texture2D>(@"Tree1");
            TextureBank.MapTiles.groupOfTrees = Content.Load<Texture2D>(@"GroupTrees");

            TextureBank.MapTiles.openWater = Content.Load<Texture2D>(@"OpenWater");
            TextureBank.MapTiles.shoreDown = Content.Load<Texture2D>(@"RiverDown");
            TextureBank.MapTiles.shoreLeft = Content.Load<Texture2D>(@"RiverLeft");
            TextureBank.MapTiles.shoreRight = Content.Load<Texture2D>(@"RiverRight");
            TextureBank.MapTiles.shoreUp = Content.Load<Texture2D>(@"RiverUp");
            TextureBank.MapTiles.shoreLowerLeftCorner = Content.Load<Texture2D>(@"RiverLowerLeftCorner");
            TextureBank.MapTiles.shoreLowerRightCorner = Content.Load<Texture2D>(@"RiverLowerRightCorner");
            TextureBank.MapTiles.shoreOpenLowerLeftCorner = Content.Load<Texture2D>(@"RiverOpenLowerLeftCorner");
            TextureBank.MapTiles.shoreOpenLowerRightCorner = Content.Load<Texture2D>(@"RiverOpenLowerRightCorner");
            TextureBank.MapTiles.shoreUpperLeftCorner = Content.Load<Texture2D>(@"RiverUpperLeftCorner");
            TextureBank.MapTiles.shoreUpperRightCorner = Content.Load<Texture2D>(@"RiverUpperRightCorner");
            TextureBank.MapTiles.shoreOpenUpperLeftCorner = Content.Load<Texture2D>(@"RiverOpenUpperLeftCorner");
            TextureBank.MapTiles.shoreOpenUpperRightCorner = Content.Load<Texture2D>(@"RiverOpenUpperRightCorner");

            TextureBank.MapTiles.roadHorizontal = Content.Load<Texture2D>(@"RoadHorizontal");
            TextureBank.MapTiles.roadVertical = Content.Load<Texture2D>(@"RoadVertical");
            TextureBank.MapTiles.roadElbowDownLeft = Content.Load<Texture2D>(@"RoadHorizontalBendDown");
            TextureBank.MapTiles.roadElbowDownRight = Content.Load<Texture2D>(@"RoadVerticalBendRight2");
            TextureBank.MapTiles.roadElbowUpLeft = Content.Load<Texture2D>(@"RoadHorizontalBendUp");
            TextureBank.MapTiles.roadElbowUpRight = Content.Load<Texture2D>(@"RoadVerticalBendRight");

            TextureBank.MapTiles.building1 = Content.Load<Texture2D>(@"Building1");
            TextureBank.MapTiles.building2 = Content.Load<Texture2D>(@"Building2");
            TextureBank.MapTiles.building3 = Content.Load<Texture2D>(@"Building3");
            TextureBank.MapTiles.buildingGroup = Content.Load<Texture2D>(@"GroupBuildings");
            TextureBank.MapTiles.buildingGroupEndRight = Content.Load<Texture2D>(@"GroupBuildingsEndRight");
            TextureBank.MapTiles.buildingGroupEndLeft = Content.Load<Texture2D>(@"GroupBuildingsEndLeft");
            TextureBank.MapTiles.dock = Content.Load<Texture2D>(@"Dock");
            TextureBank.MapTiles.rockBig = Content.Load<Texture2D>(@"Rock3");
            TextureBank.MapTiles.rockMedium = Content.Load<Texture2D>(@"Rock2");
            TextureBank.MapTiles.rockSmall = Content.Load<Texture2D>(@"Rock1");
        }


    }
}

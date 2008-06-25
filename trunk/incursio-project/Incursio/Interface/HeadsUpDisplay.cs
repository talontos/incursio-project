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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Incursio.Interface;
using Incursio.Managers;
using Incursio.Entities;

namespace Incursio.Interface
{
  public class HeadsUpDisplay
    {
        /*
        Button lightInfantryButton;
        Button archerButton;
        Button towerButton;
        Button heavyInfantryButton;
        */

        BuildEntityPanel buildPanel;

        //helper variables for determining selection
        private int barX;
        private int numUnits;
        

        /// <summary>
        /// loadHeadsUpDisplay loads the HUD content into the game.  from here it can be displayed on the screen.
        /// note:  the HUD will only be displayed when the gamestate is in scenario play.
        /// </summary>

        public void loadHeadsUpDisplay()
        {
            barX = 0;
            numUnits = 0;

            buildPanel = new BuildEntityPanel();

            /*
            lightInfantryButton = new BuildLightInfantryButton();
            archerButton = new BuildArcherButton();
            towerButton = new BuildTowerButton();
            heavyInfantryButton = new BuildHeavyInfantryButton();
            */
        }

        public List<BaseGameEntity> update(Cursor cursor)
        {
            List<BaseGameEntity> selectedUnits = EntityManager.getInstance().getSelectedUnits();
            int numUnitsSelected = selectedUnits.Count;

            //To see what to Update
            if (numUnitsSelected > 0)
            {

                if (selectedUnits[0].factoryComponent != null && selectedUnits[0].getPlayer() == PlayerManager.getInstance().currentPlayerId)
                {
                    this.buildPanel.Update(cursor);
                    /*
                    lightInfantryButton.Update(cursor);
                    archerButton.Update(cursor);
                    towerButton.Update(cursor);
                    heavyInfantryButton.Update(cursor);
                    */
                }
            }

            //For a cursor press that is within the bounds of the HUD
            if (cursor.getMouseState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && this.isCursorWithin((int)cursor.getPos().X, (int)cursor.getPos().Y))
            {

                if (numUnitsSelected > 0)
                {
                    if (selectedUnits[0].factoryComponent != null && selectedUnits[0].getPlayer() == PlayerManager.getInstance().currentPlayerId)
                    {

                    }
                }
            }

            //For changes in Unit Selection (TO BE COMPLETED)
            if (numUnitsSelected > 1)
            {
                if (cursor.getIsLeftPressed() && cursor.getPos().X >= 383 && cursor.getPos().X <= 760 && cursor.getPos().Y >= 638 && cursor.getPos().Y <= 733)
                {
                    this.barX = (int)((cursor.getPos().X - 383) / 63);
                    if (cursor.getPos().Y - 686 > 0)
                    {
                        this.barX = this.barX + 6;
                    }

                    List<BaseGameEntity> temp = new List<BaseGameEntity>();
                    temp.Add(selectedUnits[this.barX]);
                    this.numUnits = 1;
                    this.barX = 0;
                    return temp;
                }
            }

            this.numUnits = numUnitsSelected;
            return selectedUnits;
        }

      public int getNumUnits()
      {
          return this.numUnits;
      }

      /// <summary>
      /// isCursorWithin determines if the cursor is presently in the bounds of the HUD, this will clarify mouse commands to their desired purpose
      /// </summary>
      /// <param name="cursor"></param>
      /// <returns></returns>
      public bool isCursorWithin(int x, int y)
      {
          //three rectangles since the texture is not quite rectangular
          Rectangle boundingBox1 = new Rectangle(0, 768 - TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Height, (int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Width * 0.236), TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Height);
          Rectangle boundingBox2 = new Rectangle((int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Width * 0.236), 768 - (int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Height * 0.789), (int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Width * 0.7412), (int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Height * 0.789));
          Rectangle boundingBox3 = new Rectangle((int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Width * 0.7412), 768 - TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Height, (int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Width * 0.258), TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Height);

          if (boundingBox1.Contains(x, y) ||
              boundingBox2.Contains(x, y) ||
              boundingBox3.Contains(x, y))
          {
              return true;
          }
          return false;
      }

        /// <summary>
        /// drawHeadsUpDisplay draws the HUD onto the screen.  Parameter SpriteBatch is used.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void draw(SpriteBatch spriteBatch, int height, SpriteFont font)
        {
            //TODO: DON'T BOTHER PASSING IN UNITS; JUST GET THEM FROM MANAGER
            List<BaseGameEntity> selectedUnits = EntityManager.getInstance().getSelectedUnits();
            int numUnitsSelected = selectedUnits.Count;

            // draw the bars
            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture, new Rectangle(0, height - TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Height, TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.headsUpDisplay.texture.Height), Color.White);
            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.resourceDisplay.texture, new Rectangle(1024 - TextureBank.getInstance().InterfaceTextures.interfaceTextures.resourceDisplay.texture.Width, 0, TextureBank.getInstance().InterfaceTextures.interfaceTextures.resourceDisplay.texture.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.resourceDisplay.texture.Height), Color.White);

            //for the selected unit portrait
            if (numUnitsSelected != 0)
            {
                spriteBatch.Draw(
                    selectedUnits[0].renderComponent.textures.portrait, 
                    new Rectangle(
                        241, 
                        height - 129, 
                        selectedUnits[0].renderComponent.textures.portrait.Width, 
                        selectedUnits[0].renderComponent.textures.portrait.Height), 
                    Color.White);   
            }

            //unit attributes
            if (numUnitsSelected == 1)
            {
                //for the unit's display name
                spriteBatch.DrawString(font, selectedUnits[0].entityName, new Vector2(572, height - 118), Color.White, 0, font.MeasureString(selectedUnits[0].entityName) / 2, 1.0f, SpriteEffects.None, 0.5f);
                
                //stats!
                spriteBatch.DrawString(font, "Health: " + selectedUnits[0].getHealth(), new Vector2(572, height - 90), Color.White, 0, font.MeasureString("Health: XXX") / 2, 1.0f, SpriteEffects.None, 0.5f);

                if(selectedUnits[0].combatComponent != null)
                    spriteBatch.DrawString(font, "Attack: " + selectedUnits[0].combatComponent.damage + "   Armor: " + selectedUnits[0].armor, new Vector2(572, height - 65), Color.White, 0, font.MeasureString("Attack: XXX   Armor: XXX") / 2, 1.0f, SpriteEffects.None, 0.5f);

                if (selectedUnits[0].factoryComponent != null)
                {
                    double buildRatio = selectedUnits[0].factoryComponent.getPercentDone();

                    //lets the player know what we're building, and the progress on that construction
                    if (selectedUnits[0].factoryComponent.isBuilding())
                    {
                        spriteBatch.DrawString(font, "Building:  " + selectedUnits[0].factoryComponent.currentlyBuildingThis, new Vector2(572, height - 65), Color.White, 0, font.MeasureString("Building:  XXXXXXXXXXXXXXX") / 2, 1.0f, SpriteEffects.None, 0.5f);
                        spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture, new Rectangle(462, height - 50, TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Width * 5, TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Height * 3), Color.Black);
                        spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture, new Rectangle(462, height - 50, (int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Width * 5 * buildRatio), TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Height * 3), Color.Lime);
                    }
                    
                }

                if (selectedUnits[0].experienceComponent != null)
                {
                    double expRatio = selectedUnits[0].experienceComponent.getPercentageLvlUp();
                    string pointString = (selectedUnits[0].experienceComponent.experiencePoints + "/" + selectedUnits[0].experienceComponent.pointsToNextLevel);

                    //EXPERIENCE POINTS
                    spriteBatch.DrawString(font, pointString, new Vector2(440, height - 45), Color.White, 0, font.MeasureString(pointString) / 2, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture, new Rectangle(510, height - 50, TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Width * 5, TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Height * 3), Color.Black);
                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture, new Rectangle(510, height - 50, (int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Width * 5 * expRatio), TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Height * 3), Color.Lime);
                    
                }

                if (selectedUnits[0].capturableComponent != null && selectedUnits[0].capturableComponent.isCapping())
                {
                    
                    double capRatio = selectedUnits[0].capturableComponent.getPercentageDone();

                    //lets the player know what we're capping, and the progress
                    spriteBatch.DrawString(font, "Capturing: ", new Vector2(572, height - 65), Color.White, 0, font.MeasureString("Capturing: ") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture, new Rectangle(462, height - 50, TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Width * 5, TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Height * 3), Color.Black);
                    spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture, new Rectangle(462, height - 50, (int)(TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Width * 5 * capRatio), TextureBank.getInstance().InterfaceTextures.interfaceTextures.healthRatioTexture.texture.Height * 3), Color.Lime);
                }
            }
            else if (numUnitsSelected > 1)
            {
                for (int i = 0; i <= numUnitsSelected - 1; i++)
                {

                    if (i <= 5)
                    {
                        spriteBatch.Draw(selectedUnits[i].renderComponent.textures.icon, new Rectangle(383 + i * 60, height - 129, selectedUnits[i].renderComponent.textures.icon.Width, selectedUnits[i].renderComponent.textures.icon.Height), Color.White);
                        /*
                        if (selectedUnits[i].getType() == State.EntityName.LightInfantry)
                        {
                            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.lightInfantryIcon, new Rectangle(383 + i * 60, height - 129, TextureBank.getInstance().InterfaceTextures.interfaceTextures.lightInfantryIcon.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.lightInfantryIcon.Height), Color.White).texture;
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.Archer)
                        {
                            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.archerIcon, new Rectangle(383 + i * 60, height - 129, TextureBank.getInstance().InterfaceTextures.interfaceTextures.archerIcon.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.archerIcon.Height), Color.White).texture;
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.HeavyInfantry)
                        {
                            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.heavyInfantryIcon, new Rectangle(383 + i * 60, height - 129, TextureBank.getInstance().InterfaceTextures.interfaceTextures.heavyInfantryIcon.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.heavyInfantryIcon.Height), Color.White).texture;
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.Hero)
                        {
                            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.heroIcon, new Rectangle(383 + i * 60, height - 129, TextureBank.getInstance().InterfaceTextures.interfaceTextures.heroIcon.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.heroIcon.Height), Color.White).texture;
                        }
                        */
                    }
                    else
                    {
                        spriteBatch.Draw(selectedUnits[i].renderComponent.textures.icon, new Rectangle(383 + i * 60 - 6 * 60, height - 84, selectedUnits[i].renderComponent.textures.icon.Width, selectedUnits[i].renderComponent.textures.icon.Height), Color.White);

                        /*
                        if (selectedUnits[i].getType() == State.EntityName.LightInfantry)
                        {
                            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.lightInfantryIcon, new Rectangle(383 + i * 60 - 6 * 60, height - 84, TextureBank.getInstance().InterfaceTextures.interfaceTextures.lightInfantryIcon.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.lightInfantryIcon.Height), Color.White).texture;
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.Archer)
                        {
                            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.archerIcon, new Rectangle(383 + i * 60 - 6 * 60, height - 84, TextureBank.getInstance().InterfaceTextures.interfaceTextures.archerIcon.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.archerIcon.Height), Color.White).texture;
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.HeavyInfantry)
                        {
                            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.heavyInfantryIcon, new Rectangle(383 + i * 60 - 6 * 60, height - 84, TextureBank.getInstance().InterfaceTextures.interfaceTextures.heavyInfantryIcon.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.heavyInfantryIcon.Height), Color.White).texture;
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.Hero)
                        {
                            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.heroIcon, new Rectangle(383 + i * 60, height - 84, TextureBank.getInstance().InterfaceTextures.interfaceTextures.heroIcon.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.heroIcon.Height), Color.White).texture;
                        }
                        */
                    }

                }
            }


            //UNIT / STRUCTURE COMMANDS
            if (numUnitsSelected > 0)
            {
                if (selectedUnits[0].factoryComponent != null && selectedUnits[0].getPlayer() == PlayerManager.getInstance().currentPlayerId)
                {
                    //Go through the available commands for the camp

                    buildPanel.Draw(spriteBatch);
                    /*
                    lightInfantryButton.Draw(spriteBatch);
                    archerButton.Draw(spriteBatch);
                    towerButton.Draw(spriteBatch);
                    heavyInfantryButton.Draw(spriteBatch);
                    */
                }
            }

            //RESOURCE BAR
            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.moneyIcon.texture,
                new Rectangle(945, 10, TextureBank.getInstance().InterfaceTextures.interfaceTextures.moneyIcon.texture.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.moneyIcon.texture.Height), Color.White);
            spriteBatch.DrawString(font, " " + PlayerManager.getInstance().currentPlayer.MONETARY_UNIT, new Vector2(975, 23), Color.White, 0, font.MeasureString("XXXX") / 2, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.Draw(TextureBank.getInstance().InterfaceTextures.interfaceTextures.controlPointInterfaceIcon.texture,
                new Rectangle(860, 12, TextureBank.getInstance().InterfaceTextures.interfaceTextures.controlPointInterfaceIcon.texture.Width, TextureBank.getInstance().InterfaceTextures.interfaceTextures.controlPointInterfaceIcon.texture.Height), Color.White);

            spriteBatch.DrawString(font, " " + EntityManager.getInstance().getPlayerTotalOwnedControlPoints(PlayerManager.getInstance().currentPlayerId) + "/" + EntityManager.getInstance().getTotalControlPoints(), new Vector2(900, 23), Color.White, 0, font.MeasureString("XXXXX") / 2, 1.0f, SpriteEffects.None, 0.5f);


        }
    }
}

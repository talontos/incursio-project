using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Incursio.Interface;
using Incursio.Managers;

namespace Incursio.Classes
{
  public class HeadsUpDisplay
    {
        Button lightInfantryButton;
        Button archerButton;
        Button towerButton;
        Button heavyInfantryButton;

        //helper variables for determining selection
        private int barX;
        private int numUnits;
        

        /// <summary>
        /// loadHeadsUpDisplay loads the HUD content into the game.  from here it can be displayed on the screen.
        /// note:  the HUD will only be displayed when the gamestate is in scenario play.
        /// </summary>
        /*public void loadHeadsUpDisplay(Texture2D bar, Texture2D lightInfantry, Texture2D archer, Texture2D infIcon, Texture2D archIcon, Texture2D resourceBar)
        {
            // Load the images for the UI
            utilityBar = bar;
            lightInfantryPortrait = lightInfantry;
            archerPortrait = archer;
            lightInfantryIcon = infIcon;
            archerIcon = archIcon;
            barX = 0;
            numUnits = 0;
            this.resourceBar = resourceBar;
        }*/

        public void loadHeadsUpDisplay()
        {
            barX = 0;
            numUnits = 0;
            lightInfantryButton = new Button(new Vector2(775, 605), TextureBank.InterfaceTextures.lightInfantryIcon, TextureBank.InterfaceTextures.lightInfantryIcon);
            archerButton = new Button(new Vector2(850, 605), TextureBank.InterfaceTextures.archerIcon, TextureBank.InterfaceTextures.archerIcon);
            towerButton = new Button(new Vector2(775, 650), TextureBank.InterfaceTextures.guardTowerIcon, TextureBank.InterfaceTextures.guardTowerIcon);
            heavyInfantryButton = new Button(new Vector2(925, 605), TextureBank.InterfaceTextures.heavyInfantryIcon, TextureBank.InterfaceTextures.heavyInfantryIcon);
        }

        public List<BaseGameEntity> update(Cursor cursor)
        {
            List<BaseGameEntity> selectedUnits = EntityManager.getInstance().getSelectedUnits();
            int numUnitsSelected = selectedUnits.Count;

            //To see what to Update
            if (numUnitsSelected > 0)
            {

                if (selectedUnits[0].getType() == State.EntityName.Camp && selectedUnits[0].getPlayer() == State.PlayerId.HUMAN)
                {
                    lightInfantryButton.Update(cursor);
                    archerButton.Update(cursor);
                    towerButton.Update(cursor);
                    heavyInfantryButton.Update(cursor);

                    //See if any buttons are being pressed
                    if (!lightInfantryButton.getPressed() && lightInfantryButton.getFocus())
                    {
                        lightInfantryButton.setFocus(false);
                        EntityManager.getInstance().tryToBuild(new LightInfantryUnit());
                    }
                    else if (!heavyInfantryButton.getPressed() && heavyInfantryButton.getFocus())
                    {
                        heavyInfantryButton.setFocus(false);
                        EntityManager.getInstance().tryToBuild(new HeavyInfantryUnit());
                    }
                    else if (!archerButton.getPressed() && archerButton.getFocus())
                    {
                        archerButton.setFocus(false);
                        EntityManager.getInstance().tryToBuild(new ArcherUnit());
                    }
                    else if (!towerButton.getPressed() && towerButton.getFocus())
                    {
                        towerButton.setFocus(false);
                        InputManager.getInstance().positioningTower = true;
                        TextureBank.InterfaceTextures.cursorEvent = TextureBank.EntityTextures.guardTowerTexturePlayer;
                    }
                }
            }

            //For a cursor press that is within the bounds of the HUD
            if (cursor.getMouseState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed && this.isCursorWithin((int)cursor.getPos().X, (int)cursor.getPos().Y))
            {

                if (numUnitsSelected > 0)
                {
                    if (selectedUnits[0].getType() == State.EntityName.Camp && selectedUnits[0].getPlayer() == State.PlayerId.HUMAN)
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
          Rectangle boundingBox1 = new Rectangle(0, 768 - TextureBank.InterfaceTextures.headsUpDisplay.Height, (int)(TextureBank.InterfaceTextures.headsUpDisplay.Width * 0.236), TextureBank.InterfaceTextures.headsUpDisplay.Height);
          Rectangle boundingBox2 = new Rectangle((int)(TextureBank.InterfaceTextures.headsUpDisplay.Width * 0.236), 768 - (int)(TextureBank.InterfaceTextures.headsUpDisplay.Height * 0.789), (int)(TextureBank.InterfaceTextures.headsUpDisplay.Width * 0.7412), (int)(TextureBank.InterfaceTextures.headsUpDisplay.Height * 0.789));
          Rectangle boundingBox3 = new Rectangle((int)(TextureBank.InterfaceTextures.headsUpDisplay.Width * 0.7412), 768 - TextureBank.InterfaceTextures.headsUpDisplay.Height, (int)(TextureBank.InterfaceTextures.headsUpDisplay.Width * 0.258), TextureBank.InterfaceTextures.headsUpDisplay.Height);

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
            spriteBatch.Draw(TextureBank.InterfaceTextures.headsUpDisplay, new Rectangle(0, height - TextureBank.InterfaceTextures.headsUpDisplay.Height, TextureBank.InterfaceTextures.headsUpDisplay.Width, TextureBank.InterfaceTextures.headsUpDisplay.Height), Color.White);
            spriteBatch.Draw(TextureBank.InterfaceTextures.resourceDisplay, new Rectangle(1024 - TextureBank.InterfaceTextures.resourceDisplay.Width, 0, TextureBank.InterfaceTextures.resourceDisplay.Width, TextureBank.InterfaceTextures.resourceDisplay.Height), Color.White);

            //for the selected unit portrait
            if (numUnitsSelected != 0)
            {
                if (selectedUnits[0].getType() == State.EntityName.LightInfantry)
                {
                    spriteBatch.Draw(TextureBank.InterfaceTextures.lightInfantryPortrait, new Rectangle(241, height - 129, TextureBank.InterfaceTextures.lightInfantryPortrait.Width, TextureBank.InterfaceTextures.lightInfantryPortrait.Height), Color.White);
                }
                else if (selectedUnits[0].getType() == State.EntityName.Archer)
                {
                    spriteBatch.Draw(TextureBank.InterfaceTextures.archerPortrait, new Rectangle(241, height - 129, TextureBank.InterfaceTextures.archerPortrait.Width, TextureBank.InterfaceTextures.archerPortrait.Height), Color.White);
                }
                else if (selectedUnits[0].getType() == State.EntityName.Camp)
                {
                    spriteBatch.Draw(TextureBank.InterfaceTextures.basePortrait, new Rectangle(241, height - 129, TextureBank.InterfaceTextures.basePortrait.Width, TextureBank.InterfaceTextures.basePortrait.Height), Color.White);
                }
                else if (selectedUnits[0].getType() == State.EntityName.GuardTower)
                {
                    spriteBatch.Draw(TextureBank.InterfaceTextures.guardTowerPortrait, new Rectangle(241, height - 129, TextureBank.InterfaceTextures.guardTowerPortrait.Width, TextureBank.InterfaceTextures.guardTowerPortrait.Height), Color.White);
                }
                else if (selectedUnits[0].getType() == State.EntityName.HeavyInfantry)
                {
                    spriteBatch.Draw(TextureBank.InterfaceTextures.heavyInfantryPortrait, new Rectangle(241, height - 129, TextureBank.InterfaceTextures.heavyInfantryPortrait.Width, TextureBank.InterfaceTextures.heavyInfantryPortrait.Height), Color.White);
                }
                else if (selectedUnits[0].getType() == State.EntityName.ControlPoint)
                {
                    spriteBatch.Draw(TextureBank.InterfaceTextures.controlPointPortrait, new Rectangle(241, height - 129, TextureBank.InterfaceTextures.controlPointPortrait.Width, TextureBank.InterfaceTextures.controlPointPortrait.Height), Color.White);
                }
                else if (selectedUnits[0].getType() == State.EntityName.Hero)
                {
                    spriteBatch.Draw(TextureBank.InterfaceTextures.heroPortrait, new Rectangle(241, height - 129, TextureBank.InterfaceTextures.heroPortrait.Width, TextureBank.InterfaceTextures.heroPortrait.Height), Color.White);
                }
            }

            //unit attributes
            if (numUnitsSelected == 1)
            {
                //for the unit's display name
                if (selectedUnits[0].getType() == State.EntityName.LightInfantry)
                {
                    spriteBatch.DrawString(font, "Light Infantry", new Vector2(572, height - 118), Color.White, 0, font.MeasureString("Light Infantry") / 2, 1.0f, SpriteEffects.None, 0.5f);
                }
                else if (selectedUnits[0].getType() == State.EntityName.Archer)
                {
                    spriteBatch.DrawString(font, "Archer", new Vector2(572, height - 118), Color.White, 0, font.MeasureString("Archer") / 2, 1.0f, SpriteEffects.None, 0.5f);
                }
                else if (selectedUnits[0].getType() == State.EntityName.HeavyInfantry)
                {
                    spriteBatch.DrawString(font, "Heavy Infantry", new Vector2(572, height - 118), Color.White, 0, font.MeasureString("Heavy Infantry") / 2, 1.0f, SpriteEffects.None, 0.5f);
                }
                else if (selectedUnits[0].getType() == State.EntityName.Hero)
                {
                    spriteBatch.DrawString(font, "Hero", new Vector2(572, height - 118), Color.White, 0, font.MeasureString("Hero") / 2, 1.0f, SpriteEffects.None, 0.5f);
                }
                else if (selectedUnits[0].getType() == State.EntityName.ControlPoint)
                {
                    spriteBatch.DrawString(font, "Control Point", new Vector2(572, height - 118), Color.White, 0, font.MeasureString("Control Point") / 2, 1.0f, SpriteEffects.None, 0.5f);
                }
                else if (selectedUnits[0].getType() == State.EntityName.Camp)
                {
                    spriteBatch.DrawString(font, "Camp", new Vector2(572, height - 118), Color.White, 0, font.MeasureString("Camp") / 2, 1.0f, SpriteEffects.None, 0.5f);
                }
                else if (selectedUnits[0].getType() == State.EntityName.GuardTower)
                {
                    spriteBatch.DrawString(font, "Guard Tower", new Vector2(572, height - 118), Color.White, 0, font.MeasureString("Guard Tower") / 2, 1.0f, SpriteEffects.None, 0.5f);
                }
                else if (selectedUnits[0].getType() == State.EntityName.ControlPoint)
                {
                    spriteBatch.DrawString(font, "Control Point", new Vector2(572, height - 118), Color.White, 0, font.MeasureString("Control Point") / 2, 1.0f, SpriteEffects.None, 0.5f);
                }

                //stats!
                spriteBatch.DrawString(font, "Health: " + selectedUnits[0].getHealth(), new Vector2(572, height - 90), Color.White, 0, font.MeasureString("Health: XXX") / 2, 1.0f, SpriteEffects.None, 0.5f);

                if(selectedUnits[0].getType() == State.EntityName.Archer || selectedUnits[0].getType() == State.EntityName.HeavyInfantry ||
                   selectedUnits[0].getType() == State.EntityName.Hero   || selectedUnits[0].getType() == State.EntityName.LightInfantry)
                spriteBatch.DrawString(font, "Attack: " + (selectedUnits[0] as Unit).getDamage() + "   Armor: " + (selectedUnits[0] as Unit).getArmor(), new Vector2(572, height - 65), Color.White, 0, font.MeasureString("Attack: XXX   Armor: XXX") / 2, 1.0f, SpriteEffects.None, 0.5f);

                if (selectedUnits[0].getType() == State.EntityName.Camp)
                {
                    double buildRatio = (selectedUnits[0] as CampStructure).getPercentDone();

                    //lets the player know what we're building, and the progress on that construction
                    if ((selectedUnits[0] as CampStructure).isBuilding())
                    {
                        spriteBatch.DrawString(font, "Building:  " + (selectedUnits[0] as CampStructure).getCurrentlyBuilding(), new Vector2(572, height - 65), Color.White, 0, font.MeasureString("Building:  XXXXXXXXXXXXXXX") / 2, 1.0f, SpriteEffects.None, 0.5f);
                        spriteBatch.Draw(TextureBank.EntityTextures.healthRatioTexture, new Rectangle(462, height - 50, TextureBank.EntityTextures.healthRatioTexture.Width * 5, TextureBank.EntityTextures.healthRatioTexture.Height * 3), Color.Black);
                        spriteBatch.Draw(TextureBank.EntityTextures.healthRatioTexture, new Rectangle(462, height - 50, (int) (TextureBank.EntityTextures.healthRatioTexture.Width * 5 * buildRatio) , TextureBank.EntityTextures.healthRatioTexture.Height * 3), Color.Lime);
                    }
                    
                }

                if (selectedUnits[0].getType() == State.EntityName.ControlPoint && (selectedUnits[0] as ControlPoint).isCapping())
                {
                    
                    double capRatio = (selectedUnits[0] as ControlPoint).getPercentageDone();

                    //lets the player know what we're capping, and the progress
                    spriteBatch.DrawString(font, "Capturing: ", new Vector2(572, height - 65), Color.White, 0, font.MeasureString("Capturing: ") / 2, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(TextureBank.EntityTextures.healthRatioTexture, new Rectangle(462, height - 50, TextureBank.EntityTextures.healthRatioTexture.Width * 5, TextureBank.EntityTextures.healthRatioTexture.Height * 3), Color.Black);
                    spriteBatch.Draw(TextureBank.EntityTextures.healthRatioTexture, new Rectangle(462, height - 50, (int)(TextureBank.EntityTextures.healthRatioTexture.Width * 5 * capRatio), TextureBank.EntityTextures.healthRatioTexture.Height * 3), Color.Lime);
                }
            }
            else if (numUnitsSelected > 1)
            {
                for (int i = 0; i <= numUnitsSelected - 1; i++)
                {

                    if (i <= 5)
                    {
                        if (selectedUnits[i].getType() == State.EntityName.LightInfantry)
                        {
                            spriteBatch.Draw(TextureBank.InterfaceTextures.lightInfantryIcon, new Rectangle(383 + i * 60, height - 129, TextureBank.InterfaceTextures.lightInfantryIcon.Width, TextureBank.InterfaceTextures.lightInfantryIcon.Height), Color.White);
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.Archer)
                        {
                            spriteBatch.Draw(TextureBank.InterfaceTextures.archerIcon, new Rectangle(383 + i * 60, height - 129, TextureBank.InterfaceTextures.archerIcon.Width, TextureBank.InterfaceTextures.archerIcon.Height), Color.White);
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.HeavyInfantry)
                        {
                            spriteBatch.Draw(TextureBank.InterfaceTextures.heavyInfantryIcon, new Rectangle(383 + i * 60, height - 129, TextureBank.InterfaceTextures.heavyInfantryIcon.Width, TextureBank.InterfaceTextures.heavyInfantryIcon.Height), Color.White);
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.Hero)
                        {
                            spriteBatch.Draw(TextureBank.InterfaceTextures.heroIcon, new Rectangle(383 + i * 60, height - 129, TextureBank.InterfaceTextures.heroIcon.Width, TextureBank.InterfaceTextures.heroIcon.Height), Color.White);
                        }
                    }
                    else
                    {
                        if (selectedUnits[i].getType() == State.EntityName.LightInfantry)
                        {
                            spriteBatch.Draw(TextureBank.InterfaceTextures.lightInfantryIcon, new Rectangle(383 + i * 60 - 6 * 60, height - 84, TextureBank.InterfaceTextures.lightInfantryIcon.Width, TextureBank.InterfaceTextures.lightInfantryIcon.Height), Color.White);
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.Archer)
                        {
                            spriteBatch.Draw(TextureBank.InterfaceTextures.archerIcon, new Rectangle(383 + i * 60 - 6 * 60, height - 84, TextureBank.InterfaceTextures.archerIcon.Width, TextureBank.InterfaceTextures.archerIcon.Height), Color.White);
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.HeavyInfantry)
                        {
                            spriteBatch.Draw(TextureBank.InterfaceTextures.heavyInfantryIcon, new Rectangle(383 + i * 60 - 6 * 60, height - 84, TextureBank.InterfaceTextures.heavyInfantryIcon.Width, TextureBank.InterfaceTextures.heavyInfantryIcon.Height), Color.White);
                        }
                        else if (selectedUnits[i].getType() == State.EntityName.Hero)
                        {
                            spriteBatch.Draw(TextureBank.InterfaceTextures.heroIcon, new Rectangle(383 + i * 60, height - 84, TextureBank.InterfaceTextures.heroIcon.Width, TextureBank.InterfaceTextures.heroIcon.Height), Color.White);
                        }
                    }

                }
            }


            //UNIT / STRUCTURE COMMANDS
            if (numUnitsSelected > 0)
            {
                if (selectedUnits[0].getType() == State.EntityName.Camp && selectedUnits[0].getPlayer() == State.PlayerId.HUMAN)
                {
                    //Go through the available commands for the camp
                    lightInfantryButton.Draw(spriteBatch);
                    archerButton.Draw(spriteBatch);
                    towerButton.Draw(spriteBatch);
                    heavyInfantryButton.Draw(spriteBatch);
                    //spriteBatch.Draw(TextureBank.InterfaceTextures.lightInfantryIcon, new Rectangle(775, height - 163, 75, 48), Color.White);
                    //spriteBatch.Draw(TextureBank.InterfaceTextures.archerIcon, new Rectangle(850, height - 163, 75, 48), Color.White);
                    //spriteBatch.Draw(TextureBank.InterfaceTextures.guardTowerIcon, new Rectangle(925, height - 163, 75, 48), Color.White);
                }
            }

            //RESOURCE BAR
            spriteBatch.Draw(TextureBank.InterfaceTextures.moneyIcon,
                new Rectangle(945, 10, TextureBank.InterfaceTextures.moneyIcon.Width, TextureBank.InterfaceTextures.moneyIcon.Height), Color.White);
            spriteBatch.DrawString(font, " " + PlayerManager.getInstance().humanPlayer.MONETARY_UNIT, new Vector2(975, 23), Color.White, 0, font.MeasureString("XXXX") / 2, 1.0f, SpriteEffects.None, 0.5f);

        }
    }
}

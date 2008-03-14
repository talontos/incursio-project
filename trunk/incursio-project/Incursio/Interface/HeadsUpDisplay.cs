using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Incursio.Classes
{
  public class HeadsUpDisplay
    {
        //Texture2D containing the utility bar.
        Texture2D utilityBar; 

        //Texture2D containing the resource bar
        Texture2D resourceBar;
        
        //portraits
        Texture2D lightInfantryPortrait;
        Texture2D archerPortrait;

        //icons
        Texture2D lightInfantryIcon;
        Texture2D archerIcon;

        //helper variables for determining selection
        private int barX;
        private int numUnits;
        

        /// <summary>
        /// loadHeadsUpDisplay loads the HUD content into the game.  from here it can be displayed on the screen.
        /// note:  the HUD will only be displayed when the gamestate is in scenario play.
        /// </summary>
        public void loadHeadsUpDisplay(Texture2D bar, Texture2D lightInfantry, Texture2D archer, Texture2D infIcon, Texture2D archIcon, Texture2D resourceBar)
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
        }

        public List<Unit> update(Cursor cursor, List<Unit> selectedUnits, int numUnitsSelected)
        {

            if (numUnitsSelected > 1)
            {
                if (cursor.getIsLeftPressed() && cursor.getPos().X >= 383 && cursor.getPos().X <= 760 && cursor.getPos().Y >= 638 && cursor.getPos().Y <= 733)
                {
                    this.barX = (int)((cursor.getPos().X - 383) / 63);
                    if (cursor.getPos().Y - 686 > 0)
                    {
                        this.barX = this.barX + 6;
                    }

                    List<Unit> temp = new List<Unit>();
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
        /// drawHeadsUpDisplay draws the HUD onto the screen.  Parameter SpriteBatch is used.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void draw(SpriteBatch spriteBatch, int height, List<Unit> selectedUnits, SpriteFont font, int numUnitsSelected)
        {
            // draw the bars
            spriteBatch.Draw(utilityBar, new Rectangle(0, height - utilityBar.Height, utilityBar.Width, utilityBar.Height), Color.White);
            spriteBatch.Draw(resourceBar, new Rectangle(1024 - resourceBar.Width, 0, resourceBar.Width, resourceBar.Height), Color.White);

            //for the selected unit portrait
            if (numUnitsSelected != 0)
            {
                if (selectedUnits[0].getType() == State.EntityName.LightInfantry)
                {
                    spriteBatch.Draw(lightInfantryPortrait, new Rectangle(241, height - 129, lightInfantryPortrait.Width, lightInfantryPortrait.Height), Color.White);
                }
                else if (selectedUnits[0].getType() == State.EntityName.Archer)
                {
                    spriteBatch.Draw(archerPortrait, new Rectangle(241, height - 129, archerPortrait.Width, archerPortrait.Height), Color.White);
                }
            }

            //unit attributes
            if (numUnitsSelected == 1)
            {
                //commented out selectedUnit[].getType returns enum not string
                //spriteBatch.DrawString(font, selectedUnits[0].getType(), new Vector2(572, height - 118), Color.White, 0, font.MeasureString(selectedUnits[0].getType()) / 2, 1.0f, SpriteEffects.None, 0.5f);

                spriteBatch.DrawString(font, "Health: " + selectedUnits[0].getHealth(), new Vector2(572, height - 90), Color.White, 0, font.MeasureString("Health: XXX") / 2, 1.0f, SpriteEffects.None, 0.5f);

                spriteBatch.DrawString(font, "Attack: " + selectedUnits[0].getDamage() + "   Armor: " + selectedUnits[0].getArmor(), new Vector2(572, height - 65), Color.White, 0, font.MeasureString("Attack: XXX   Armor: XXX") / 2, 1.0f, SpriteEffects.None, 0.5f);
            }
            else if (numUnitsSelected > 1)
            {
                for (int i = 0; i <= numUnitsSelected - 1; i++)
                {

                    if (i <= 5)
                    {
                        if (selectedUnits[i].getType().Equals("Light Infantry"))
                        {
                            spriteBatch.Draw(lightInfantryIcon, new Rectangle(383 + i * 60, height - 129, lightInfantryIcon.Width, lightInfantryIcon.Height), Color.White);
                        }
                        else if (selectedUnits[i].getType().Equals("Archer"))
                        {
                            spriteBatch.Draw(archerIcon, new Rectangle(383 + i * 60, height - 129, archerIcon.Width, archerIcon.Height), Color.White);
                        }
                    }
                    else
                    {
                        if (selectedUnits[i].getType().Equals("Light Infantry"))
                        {
                            spriteBatch.Draw(lightInfantryIcon, new Rectangle(383 + i * 60 - 6*60, height - 84, lightInfantryIcon.Width, lightInfantryIcon.Height), Color.White);
                        }
                        else if (selectedUnits[i].getType().Equals("Archer"))
                        {
                            spriteBatch.Draw(archerIcon, new Rectangle(383 + i * 60 - 6*60, height - 84, archerIcon.Width, archerIcon.Height), Color.White);
                        }
                    }

                }
            }

        }
    }
}
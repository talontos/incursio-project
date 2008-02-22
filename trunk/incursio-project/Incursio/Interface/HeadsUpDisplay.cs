using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Incursio.Classes
{
    class HeadsUpDisplay
    {

        Texture2D utilityBar;                   //Texture2D containing the utility bar.
        
        //portraits
        Texture2D lightInfantryPortrait;
        Texture2D archerPortrait;
        

        /// <summary>
        /// loadHeadsUpDisplay loads the HUD content into the game.  from here it can be displayed on the screen.
        /// note:  the HUD will only be displayed when the gamestate is in scenario play.
        /// </summary>
        public void loadHeadsUpDisplay(Texture2D bar, Texture2D lightInfantry, Texture2D archer)
        {
            // Load the images for the UI
            utilityBar = bar;
            lightInfantryPortrait = lightInfantry;
            archerPortrait = archer;
        }

        public void update(Cursor cursor, Unit[] selectedUnits)
        {

        }

        /// <summary>
        /// drawHeadsUpDisplay draws the HUD onto the screen.  Parameter SpriteBatch is used.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void draw(SpriteBatch spriteBatch, int height, Unit[] selectedUnits, SpriteFont font, int numUnitsSelected)
        {
            // draw our images
            spriteBatch.Draw(utilityBar, new Rectangle(0, height - utilityBar.Height, utilityBar.Width, utilityBar.Height), Color.White);

            //for the selected unit portrait
            if (numUnitsSelected != 0)
            {
                if (selectedUnits[0].getType().Equals("Light Infantry"))
                {
                    spriteBatch.Draw(lightInfantryPortrait, new Rectangle(241, height - 129, lightInfantryPortrait.Width, lightInfantryPortrait.Height), Color.White);
                }
                else if (selectedUnits[0].getType().Equals("Archer"))
                {
                    spriteBatch.Draw(archerPortrait, new Rectangle(241, height - 129, archerPortrait.Width, archerPortrait.Height), Color.White);
                }
            }

            //unit attributes
            if (numUnitsSelected == 1)
            {
                spriteBatch.DrawString(font, selectedUnits[0].getType(), new Vector2(572, height - 118), Color.White, 0, font.MeasureString(selectedUnits[0].getType()) / 2, 1.0f, SpriteEffects.None, 0.5f);

                spriteBatch.DrawString(font, "Health: " + selectedUnits[0].getHealth(), new Vector2(572, height - 90), Color.White, 0, font.MeasureString("Health: XXX") / 2, 1.0f, SpriteEffects.None, 0.5f);

                spriteBatch.DrawString(font, "Attack: " + selectedUnits[0].getDamage() + "   Armor: " + selectedUnits[0].getArmor(), new Vector2(572, height - 65), Color.White, 0, font.MeasureString("Attack: XXX   Armor: XXX") / 2, 1.0f, SpriteEffects.None, 0.5f);
            }
        }
    }
}

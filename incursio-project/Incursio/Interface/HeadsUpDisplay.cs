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

        /// <summary>
        /// loadHeadsUpDisplay loads the HUD content into the game.  from here it can be displayed on the screen.
        /// note:  the HUD will only be displayed when the gamestate is in scenario play.
        /// </summary>
        public void loadHeadsUpDisplay(Texture2D bar)
        {
            // Load the images for the UI
            utilityBar = bar;
        }

        /// <summary>
        /// drawHeadsUpDisplay draws the HUD onto the screen.  Parameter SpriteBatch is used.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void drawHeadsUpDisplay(SpriteBatch spriteBatch, int height)
        {
            // draw our images
            spriteBatch.Draw(utilityBar, new Rectangle(0, height - utilityBar.Height, utilityBar.Width, utilityBar.Height), Color.White);
        }
    }
}

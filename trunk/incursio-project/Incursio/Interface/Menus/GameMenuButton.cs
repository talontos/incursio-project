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

using Incursio.Interface;
using Incursio.Classes;
using Incursio.Utils;

namespace Incursio.Interface
{
  public class GameMenuButton : Button
    {
        Button resumeButton;
        Button exitGameButton;
        Texture2D holderBox;
        bool bringUpMenu;

        public GameMenuButton(Vector2 position, Texture2D passive, Texture2D pressed, 
            Texture2D rPassive, Texture2D rPressed,
            Texture2D ePassive, Texture2D ePressed,
            Texture2D holderBox)
        {
            this.position = position;
            this.passive = passive;
            this.pressed = pressed;

            this.resumeButton = new Button(new Vector2(475, 349), rPassive, rPressed);
            this.exitGameButton = new Button(new Vector2(475, 384), ePassive, ePressed);
            this.holderBox = holderBox;
            this.bringUpMenu = false;
        }

        public override void Action(Cursor cursor, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(holderBox, new Rectangle(465, 344, holderBox.Width, holderBox.Height), Color.White);
            resumeButton.Draw(spriteBatch);
            exitGameButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (bringUpMenu)
            {
                spriteBatch.Draw(holderBox, new Rectangle(465, 344, holderBox.Width, holderBox.Height), Color.White);
                resumeButton.Draw(spriteBatch);
                exitGameButton.Draw(spriteBatch);
            }
            else
            {
                if (isPressed == false)
                {
                    spriteBatch.Draw(passive, position, Color.White);
                }
                else if (isPressed == true)
                {
                    spriteBatch.Draw(pressed, position, Color.White);
                }
            }
        }

        public Button getExitButton()
        {
            return exitGameButton;
        }

        public Button getResumeButton()
        {
            return resumeButton;
        }

        public void pullUpMenu(bool pullUp)
        {
            this.bringUpMenu = pullUp;
            this.isFocus = pullUp;
            if (!pullUp)
            {
                this.exitGameButton.setFocus(pullUp);
                this.resumeButton.setFocus(pullUp);
            }

        }
    }
}

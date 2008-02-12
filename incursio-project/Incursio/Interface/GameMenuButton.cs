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
    class GameMenuButton : Button
    {
        Button resumeButton;
        Button exitGameButton;
        Texture2D holderBox;

        public GameMenuButton(Vector2 position, Texture2D passive, Texture2D pressed, 
            Texture2D rPassive, Texture2D rPressed,
            Texture2D ePassive, Texture2D ePressed,
            Texture2D holderBox)
        {
            this.position = position;
            this.passive = passive;
            this.pressed = pressed;

            this.resumeButton = new Button(new Vector2(470, 349), rPassive, rPressed);
            this.exitGameButton = new Button(new Vector2(470, 384), ePassive, ePressed);
            this.holderBox = holderBox;
        }

        public void Action(Cursor cursor, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(holderBox, new Rectangle(465, 344, holderBox.Width, holderBox.Height), Color.White);
            resumeButton.Draw(spriteBatch);
            exitGameButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isPressed == false)
            {
                spriteBatch.Draw(passive, position, Color.White);
            }
            else if (isPressed == true)
            {
                spriteBatch.Draw(pressed, position, Color.White);
            }

            if (isFocus)
            {
                spriteBatch.Draw(holderBox, new Rectangle(465, 344, holderBox.Width, holderBox.Height), Color.White);
                resumeButton.Draw(spriteBatch);
                exitGameButton.Draw(spriteBatch);
            }
        }
    }
}

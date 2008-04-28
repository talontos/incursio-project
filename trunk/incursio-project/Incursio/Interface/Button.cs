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
  public class Button
    {
        protected Vector2 position;
        protected Texture2D passive;
        protected Texture2D pressed;
        protected bool isPressed;
        protected bool isFocus;

        public Button()
        {

        }

        public Button(Vector2 position, Texture2D passive, Texture2D pressed)
        {
            this.position = position;
            this.passive = passive;
            this.pressed = pressed;
            isPressed = false;
            isFocus = false;
        }

        public virtual void Draw(SpriteBatch batch)
        {
            if (isPressed == false || isFocus == false)
            {
                batch.Draw(passive, position, Color.White);
            }
            else if (isPressed == true)
            {
                batch.Draw(pressed, position, Color.White);
            }
        }

        public virtual void Update(Cursor cursor)
        {
            if (cursor.getPos().X >= position.X && cursor.getPos().X <= position.X + passive.Width && cursor.getPos().Y >= position.Y && cursor.getPos().Y <= position.Y + passive.Height && cursor.getIsLeftPressed())
            {
                isPressed = true;
                isFocus = true;
            }
            else
            {
                if (cursor.getPos().X < position.X || cursor.getPos().X > position.X + passive.Width || cursor.getPos().Y < position.Y || cursor.getPos().Y > position.Y + passive.Height && isPressed)
                {
                    isFocus = false;
                }
                else
                {
                    isPressed = false;
                }
            }

        }

        public virtual void Action(Cursor cursor, SpriteBatch spriteBatch)
        {

        }

        //getters and setters
        public bool getPressed()
        {
            return isPressed;
        }

        public void setFocus(bool focus)
        {
            this.isFocus = focus;
        }

        public bool getFocus()
        {
            return isFocus;
        }
    }
}

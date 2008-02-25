using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Incursio.Classes
{
  public class Cursor
    {
        private Vector2 pos;                        // position of the cursor
        private Texture2D passive;                  // texture of the mouse while idle
        private Texture2D pressed;                  // texture of the mouse if pressed
        private MouseState mouseState;
        private bool isPressed;                     // tells us whether the mouse is pressed or not

        public Cursor(Vector2 pos, Texture2D pass, Texture2D press)
        {
            this.pos = pos;
            this.passive = pass;
            this.pressed = press;
            this.isPressed = false;
        }

        public void Update()
        {
            mouseState = Mouse.GetState();
            this.pos.X = mouseState.X;
            this.pos.Y = mouseState.Y;

            if (mouseState.LeftButton == ButtonState.Pressed && this.isPressed == false)
            {
                this.isPressed = true;
            }
            else if (mouseState.LeftButton == ButtonState.Released && this.isPressed == true)
            {
                this.isPressed = false;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (this.isPressed == false)
            {
                batch.Draw(this.passive, this.pos, Color.White);
            }
            else if(this.isPressed == true)
            {
                batch.Draw(this.pressed, this.pos, Color.White);
            }
        }

        public Vector2 getPos()
        {
            return pos;
        }

        public MouseState getMouseState()
        {
            return mouseState;
        }

        public bool getIsPressed()
        {
            return isPressed;
        }
    }
}

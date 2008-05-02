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
using Microsoft.Xna.Framework.Input;

using Incursio.Managers;

namespace Incursio.Interface
{
  public class Cursor
    {
        private Vector2 pos;                        // position of the cursor
        private MouseState mouseState;
        private MouseState previousState;
        private bool isLeftPressed;                     // tells us whether the mouse is pressed or not
        private bool isRightPressed;

        /*public Cursor(Vector2 pos, Texture2D pass, Texture2D press)
        {
            this.pos = pos;
            this.passive = pass;
            this.pressed = press;
            this.isLeftPressed = false;
            this.isRightPressed = false;
        }*/

        public Cursor(Vector2 pos)
        {
            this.pos = pos;
            this.isLeftPressed = false;
            this.isRightPressed = false;
        }

        public void Update()
        {
            previousState = mouseState; //remember previous state
            mouseState = Mouse.GetState();
            this.pos.X = mouseState.X;
            this.pos.Y = mouseState.Y;

            if (mouseState.LeftButton == ButtonState.Pressed && this.isLeftPressed == false)
            {
                this.isLeftPressed = true;
            }
            else if (mouseState.LeftButton == ButtonState.Released && this.isLeftPressed == true)
            {
                this.isLeftPressed = false;
            }

            if (mouseState.RightButton == ButtonState.Pressed && this.isRightPressed == false)
            {
                this.isRightPressed = true;
            }
            else if (mouseState.RightButton == ButtonState.Released && this.isRightPressed == true)
            {
                this.isRightPressed = false;
            }
        }

        public void Draw(SpriteBatch batch)
        {
            if (TextureBank.InterfaceTextures.cursorEvent != null)
            {
                batch.Draw(TextureBank.InterfaceTextures.cursorEvent, 
                    new Vector2 (this.pos.X - (int)(TextureBank.InterfaceTextures.cursorEvent.Width / 2),
                                 this.pos.Y - (int)(TextureBank.InterfaceTextures.cursorEvent.Height * 0.80)), 
                    new Color(255, 255, 255, 125));
            }
            else
            {
                if (this.isLeftPressed == false && this.isRightPressed == false)
                {
                    batch.Draw(TextureBank.InterfaceTextures.cursor, this.pos, Color.White);
                }
                else if (this.isLeftPressed == true || this.isRightPressed == true)
                {
                    batch.Draw(TextureBank.InterfaceTextures.cursorPressed, this.pos, Color.White);
                }
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

        public bool getIsLeftPressed()
        {
            return isLeftPressed;
        }

        public bool getIsRightPressed()
        {
            return isRightPressed;
        }

        public MouseState getPreviousState(){
            return previousState;
        }
    }
}

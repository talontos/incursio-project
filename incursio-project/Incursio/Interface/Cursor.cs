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
using Incursio.Entities;

namespace Incursio.Interface
{
  public class Cursor
  {
      private static Cursor instance;
      private Vector2 pos;                        // position of the cursor
      private Vector2 tooltipPos;
      private MouseState mouseState;
      private MouseState previousState;
      private bool isLeftPressed;                     // tells us whether the mouse is pressed or not
      private bool isRightPressed;

      public string tooltip;
      public bool placingStructure = false;

      public static Cursor getInstance(){
          if (instance == null)
              instance = new Cursor(new Vector2(0, 0));

          return instance;
      }

      private Cursor(Vector2 pos)
      {
          this.pos = pos;
          this.tooltipPos = pos;
          this.isLeftPressed = false;
          this.isRightPressed = false;
      }

      public void Update()
      {
          previousState = mouseState; //remember previous state
          mouseState = Mouse.GetState();
          this.pos.X = mouseState.X;
          this.pos.Y = mouseState.Y;
          this.tooltipPos.Y = pos.Y - 30;
          this.tooltipPos.X = pos.X;

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

          if(tooltip != null){
              //TODO: DRAW A BOX AROUND IT?

              //draw what I build at the cursor
              batch.DrawString(Incursio.getInstance().getFont_Arial(), tooltip, this.tooltipPos, Color.White);
          }

      }

      public void beginPlaceStructure(BaseGameEntityConfiguration c){
          if(c.isStructure){
              //TODO: we need to get either an image to show, or a bounding box/occupancy to show
              this.placingStructure = true;
          }
      }

      public void finishPlaceStructure(){

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

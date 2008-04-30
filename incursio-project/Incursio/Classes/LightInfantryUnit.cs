using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Managers;
using Microsoft.Xna.Framework;
using Incursio.Utils;

namespace Incursio.Classes
{
  public class LightInfantryUnit : Unit
    {
      public static string CLASSNAME = "Incursio.Classes.LightInfantryUnit";

      public LightInfantryUnit() : base(){
          this.pointValue = 50;

          //TODO: set LightInfantry Values
          this.armor = 3;
          this.damage = 25;
          this.moveSpeed = 125.0f;
          this.sightRange = 10;
          this.attackSpeed = 2;
          this.attackRange = 1;
          this.setType(State.EntityName.LightInfantry);
      }

      public override string getTextureName()
      {
          return @"infantryUnit";
      }

      public override void updateBounds()
      {
          Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.lightInfantrySouth;

          this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
              location.x - myRef.Width / 2,
              (int)(location.y - myRef.Height * 0.80),
              myRef.Width,
              myRef.Height
          );
      }

      public override void drawThyself(ref SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
      {

          this.visible = true;
          this.justDrawn = true;
          Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(this.location);
          Rectangle unit = this.boundingBox;
          Color colorMask = EntityManager.getInstance().getColorMask(this.owner);

          //depending on the unit's state, draw their textures
          //idle
          if (this.currentState == State.UnitState.Idle)
          {
              switch(this.directionState){
                  
                  case State.Direction.Still:
                  case State.Direction.South:
                      spriteBatch.Draw(TextureBank.EntityTextures.lightInfantrySouth,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantrySouth.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantrySouth.Width, TextureBank.EntityTextures.lightInfantrySouth.Height), colorMask);
                      break;

                  case State.Direction.East:
                      spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryEast,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryEast.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantryEast.Width, TextureBank.EntityTextures.lightInfantryEast.Height), colorMask);
                      break;

                  case State.Direction.West:
                      spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryWest,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryWest.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantryWest.Width, TextureBank.EntityTextures.lightInfantryWest.Height), colorMask);
                      break;

                  case State.Direction.North:
                      spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryNorth,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryNorth.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantryNorth.Width, TextureBank.EntityTextures.lightInfantryNorth.Height), colorMask);
                      break;
              }
              
          }
          else if (this.currentState == State.UnitState.Attacking)
          {

              switch (this.directionState){
                  case State.Direction.West:
                  case State.Direction.North:
                      spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryAttackingWest,
                      new Rectangle(onScreen.x - (25 / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryWest.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantryWest.Width, TextureBank.EntityTextures.lightInfantryWest.Height),
                      new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 25, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.lightInfantryAttackingWest.Width - 25)
                          {
                              this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 25;
                          }
                          else
                          {
                              this.currentFrameXAttackDeath = 0;
                          }
                      }
                      break;

                  case State.Direction.East:
                  case State.Direction.South:
                      spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryAttackingEast,
                      new Rectangle(onScreen.x - (25 / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryEast.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantryEast.Width, TextureBank.EntityTextures.lightInfantryEast.Height),
                      new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 25, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.lightInfantryAttackingEast.Width - 25)
                          {
                              this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 25;
                          }
                          else
                          {
                              this.currentFrameXAttackDeath = 0;
                          }
                      }
                      break;
              }
          }
          else if (this.currentState == State.UnitState.Dead)
          {
              switch (this.directionState)
              {
                  case State.Direction.West:
                  case State.Direction.North:
                      if (!this.playedDeath)
                      {
                          spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryDeathEast,
                          new Rectangle(onScreen.x - (int)(25 / 2), onScreen.y - (int)(30 * 0.80), 25, 30),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 25, 30), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.lightInfantryDeathEast.Width - 25)
                              {
                                  this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 25;
                              }
                              else
                              {
                                  this.playedDeath = true;
                              }
                          }
                      }
                      else
                      {
                          spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryDeathEast,
                          new Rectangle(onScreen.x - (int)(25 / 2), onScreen.y - (int)(30 * 0.80), 25, 30),
                          new Rectangle(75, 0, 25, 30), colorMask);
                      }
                      break;

                  case State.Direction.East:
                  case State.Direction.South:
                      if (!this.playedDeath)
                      {
                          spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryDeathWest,
                          new Rectangle(onScreen.x - (int)(25 / 2), onScreen.y - (int)(30 * 0.80), 25, 30),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 25, 30), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.lightInfantryDeathWest.Width - 25)
                              {
                                  this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 25;
                              }
                              else
                              {
                                  this.playedDeath = true;
                              }
                          }
                      }
                      else
                      {
                          spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryDeathWest,
                          new Rectangle(onScreen.x - (int)(25 / 2), onScreen.y - (int)(30 * 0.80), 25, 30),
                          new Rectangle(75, 0, 25, 30), colorMask);
                      }
                      break;

              }

          }
          else if (this.currentState == State.UnitState.Guarding)
          {
              //TODO:
              //Guarding Animation
          }
          else if (this.currentState == State.UnitState.Moving)
          {

              switch(this.directionState){
                  case State.Direction.West:
                      spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryMovingWest,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryWest.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantryWest.Width, TextureBank.EntityTextures.lightInfantryWest.Height),
                      new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameX < TextureBank.EntityTextures.lightInfantryMovingWest.Width - 20)
                          {
                              this.currentFrameX = this.currentFrameX + 20;
                          }
                          else
                          {
                              this.currentFrameX = 0;
                          }
                      }
                      break;

                  case State.Direction.East:
                      spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryMovingEast,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryEast.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantryEast.Width, TextureBank.EntityTextures.lightInfantryEast.Height),
                      new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameX < TextureBank.EntityTextures.lightInfantryMovingEast.Width - 20)
                          {
                              this.currentFrameX = this.currentFrameX + 20;
                          }
                          else
                          {
                              this.currentFrameX = 0;
                          }
                      }
                      break;

                  case State.Direction.South:
                      spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryMovingSouth,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantrySouth.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantrySouth.Width, TextureBank.EntityTextures.lightInfantrySouth.Height),
                      new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameX < TextureBank.EntityTextures.lightInfantryMovingSouth.Width - 20)
                          {
                              this.currentFrameX = this.currentFrameX + 20;
                          }
                          else
                          {
                              this.currentFrameX = 0;
                          }
                      }
                      break;

                  case State.Direction.North:
                      spriteBatch.Draw(TextureBank.EntityTextures.lightInfantryMovingNorth,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantryNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantryNorth.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantryNorth.Width, TextureBank.EntityTextures.lightInfantryNorth.Height),
                      new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameX < TextureBank.EntityTextures.lightInfantryMovingNorth.Width - 20)
                          {
                              this.currentFrameX = this.currentFrameX + 20;
                          }
                          else
                          {
                              this.currentFrameX = 0;
                          }
                      }
                      break;
              }

          }
          else if (this.currentState == State.UnitState.UnderAttack)
          {
              //TODO:
              //Under Attack Animation
              spriteBatch.Draw(TextureBank.EntityTextures.lightInfantrySouth,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantrySouth.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantrySouth.Width, TextureBank.EntityTextures.lightInfantrySouth.Height), Color.Red);
          }
          else
          {
              spriteBatch.Draw(TextureBank.EntityTextures.lightInfantrySouth,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.lightInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.lightInfantrySouth.Height * 0.80),
                      TextureBank.EntityTextures.lightInfantrySouth.Width, TextureBank.EntityTextures.lightInfantrySouth.Height), colorMask);
          }

      
      }

      public override void playOrderAttackSound()
      {
          SoundManager.getInstance().PlaySound(SoundCollection.VoiceSounds.LightInfantryVoice.issueAttackOrder[0], false);
      }
    }
}

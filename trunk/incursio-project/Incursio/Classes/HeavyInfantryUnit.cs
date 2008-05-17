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

using Incursio.Managers;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Classes
{
  public class HeavyInfantryUnit : Unit
    {

      public static String CLASSNAME = "Incursio.Classes.HeavyInfantryUnit";

        public HeavyInfantryUnit() : base(){
            this.pointValue = 100;

            this.moveSpeed = 100.0f;
            this.armor = 5;
            this.damage = 30;
            this.sightRange = 8;
            this.attackSpeed = 2;
            this.attackRange = 1;
            this.maxHealth = 150;
            this.health = 150;

            this.setType(State.EntityName.HeavyInfantry);
        }

      public override void updateBounds()
      {
          Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.heavyInfantrySouth;

          this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
              location.x - myRef.Width / 2,
              (int)(location.y - myRef.Height * 0.80),
              myRef.Width,
              myRef.Height
          );
      }

      public override void drawThyself(ref Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
      {
          {
              this.visible = true;
              this.justDrawn = true;
              Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(location);
              Rectangle unit = this.boundingBox;
              Color colorMask = EntityManager.getInstance().getColorMask(this.owner);

              //depending on the unit's state, draw their textures
              //idle
              if (this.currentState == State.EntityState.Idle)
              {
                  switch(this.directionState){
                      case State.Direction.Still:
                      case State.Direction.South:
                          spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantrySouth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantrySouth.Height * 0.80),
                          TextureBank.EntityTextures.heavyInfantrySouth.Width, TextureBank.EntityTextures.heavyInfantrySouth.Height), colorMask);
                          break;

                      case State.Direction.East:
                          spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryEast,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryEast.Height * 0.80),
                          TextureBank.EntityTextures.heavyInfantryEast.Width, TextureBank.EntityTextures.heavyInfantryEast.Height), colorMask);
                          break;

                      case State.Direction.West:
                          spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryWest,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryWest.Height * 0.80),
                          TextureBank.EntityTextures.heavyInfantryWest.Width, TextureBank.EntityTextures.heavyInfantryWest.Height), colorMask);
                          break;

                      case State.Direction.North:
                          spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryNorth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryNorth.Height * 0.80),
                          TextureBank.EntityTextures.heavyInfantryNorth.Width, TextureBank.EntityTextures.heavyInfantryNorth.Height), colorMask);
                          break;
                  }

              }
              else if (this.currentState == State.EntityState.Attacking)
              {

                  switch(this.directionState){
                      case State.Direction.East:
                      case State.Direction.South:
                          spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryAttackingEast,
                          new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 45, 38), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.attackFramePause >= 4 || this.currentFrameXAttackDeath > 0)
                              {
                                  if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.heavyInfantryAttackingEast.Width - 45)
                                  {
                                      this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 45;
                                  }
                                  else
                                  {
                                      this.currentFrameXAttackDeath = 0;
                                  }
                                  this.attackFramePause = 0;
                              }
                              else
                              {
                                  this.attackFramePause++;
                              }

                          }
                          break;

                      case State.Direction.North:
                      case State.Direction.West:
                          spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryAttackingWest,
                          new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 45, 38), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.attackFramePause >= 4 || this.currentFrameXAttackDeath > 0)
                              {
                                  if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.heavyInfantryAttackingEast.Width - 45)
                                  {
                                      this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 45;
                                  }
                                  else
                                  {
                                      this.currentFrameXAttackDeath = 0;
                                  }
                                  this.attackFramePause = 0;
                              }
                              else
                              {
                                  this.attackFramePause++;
                              }
                          }
                          break;

                  }

              }
              else if (this.currentState == State.EntityState.Dead)
              {
                  switch(this.directionState){
                      case State.Direction.West:
                      case State.Direction.North:
                          if (!this.playedDeath)
                          {
                              spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryDeath_East,
                              new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                              new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 45, 38), colorMask);

                              if (frameTimer >= FRAME_LENGTH)
                              {
                                  if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.heavyInfantryDeath_East.Width - 45)
                                  {
                                      this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 45;
                                  }
                                  else
                                  {
                                      this.playedDeath = true;
                                  }
                              }
                          }
                          else
                          {
                              spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryDeath_East,
                              new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                              new Rectangle(135, 0, 45, 38), colorMask);
                          }
                          break;

                      case State.Direction.East:
                      case State.Direction.South:
                          if (!this.playedDeath)
                          {
                              spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryDeath_West,
                              new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                              new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 45, 38), colorMask);

                              if (frameTimer >= FRAME_LENGTH)
                              {
                                  if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.heavyInfantryDeath_West.Width - 45)
                                  {
                                      this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 45;
                                  }
                                  else
                                  {
                                      this.playedDeath = true;
                                  }
                              }
                          }
                          else
                          {
                              spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryDeath_West,
                              new Rectangle(onScreen.x - (int)(45 / 2), onScreen.y - (int)(38 * 0.80), 45, 38),
                              new Rectangle(135, 0, 45, 38), colorMask);
                          }
                          break;

                  }
              }
              else if (this.currentState == State.EntityState.Guarding)
              {
                  //TODO:
                  //Guarding Animation
              }
              else if (this.currentState == State.EntityState.Moving)
              {
                  switch(this.directionState){
                      case State.Direction.East:
                          spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryMovingEast,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryEast.Height * 0.80),
                          TextureBank.EntityTextures.heavyInfantryEast.Width, TextureBank.EntityTextures.heavyInfantryEast.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 25, 38), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heavyInfantryMovingEast.Width - 25)
                              {
                                  this.currentFrameX = this.currentFrameX + 25;
                              }
                              else
                              {
                                  this.currentFrameX = 0;
                              }
                          }
                          break;

                      case State.Direction.West:
                          spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryMovingWest,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryWest.Height * 0.80),
                          TextureBank.EntityTextures.heavyInfantryWest.Width, TextureBank.EntityTextures.heavyInfantryWest.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 25, 38), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX > 0)
                              {
                                  this.currentFrameX = this.currentFrameX - 25;
                              }
                              else
                              {
                                  this.currentFrameX = TextureBank.EntityTextures.heavyInfantryMovingWest.Width - TextureBank.EntityTextures.heavyInfantryWest.Width;
                              }
                          }
                          break;

                      case State.Direction.South:
                          spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryMovingSouth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantrySouth.Height * 0.80),
                          TextureBank.EntityTextures.heavyInfantrySouth.Width, TextureBank.EntityTextures.heavyInfantrySouth.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 25, 38), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heavyInfantryMovingSouth.Width - 50)
                              {
                                  this.currentFrameX = this.currentFrameX + 25;
                              }
                              else
                              {
                                  this.currentFrameX = 0;
                              }
                          }
                          break;

                      case State.Direction.North:
                          spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantryMovingNorth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantryNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantryNorth.Height * 0.80),
                          TextureBank.EntityTextures.heavyInfantryNorth.Width, TextureBank.EntityTextures.heavyInfantryNorth.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 25, 38), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heavyInfantryMovingNorth.Width - 50)
                              {
                                  this.currentFrameX = this.currentFrameX + 25;
                              }
                              else
                              {
                                  this.currentFrameX = 0;
                              }
                          }
                          break;
                  }
              }
              else if (this.currentState == State.EntityState.UnderAttack)
              {
                  //TODO:
                  //Under Attack Animation
              }
              else
              {
                  spriteBatch.Draw(TextureBank.EntityTextures.heavyInfantrySouth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heavyInfantrySouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heavyInfantrySouth.Height * 0.80),
                          TextureBank.EntityTextures.heavyInfantrySouth.Width, TextureBank.EntityTextures.heavyInfantrySouth.Height), colorMask);
              }
          }
      }

      /*
      public override void playOrderAttackSound()
      {
         if (owner != PlayerManager.getInstance().computerPlayerId)
          SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(SoundCollection.VoiceSounds.HeavyInfantryVoice.issueAttackOrder), false);
      }

      public override void playOrderMoveSound()
      {
         if (owner != PlayerManager.getInstance().computerPlayerId)
          SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(SoundCollection.VoiceSounds.HeavyInfantryVoice.issueMoveOrder), false);
      }

      public override void playDeathSound()
      {
          SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(SoundCollection.VoiceSounds.HeavyInfantryVoice.death), false);
      }

      public override void playEnterBattlefieldSound()
      {
         if (owner != PlayerManager.getInstance().computerPlayerId)
          SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(SoundCollection.VoiceSounds.HeavyInfantryVoice.enterBattlefield), false);
      }

      public override void playSelectionSound()
      {
         if (owner != PlayerManager.getInstance().computerPlayerId)
          SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(SoundCollection.VoiceSounds.HeavyInfantryVoice.selection), false);
      }
      */
    }
}

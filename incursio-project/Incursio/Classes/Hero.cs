using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Incursio.Utils;
using Incursio.Managers;

namespace Incursio.Classes
{
    // the colon is the java equivalent of 'extends'
  public class Hero : Unit
    {
      public static String HERO_CLASS = "Incursio.Classes.Hero";

      public String name = "";
      public int level = 1;
      public long experiencePoints = 0;
      public long pointsToNextLevel = 1000;

      public Hero() : base(){
          this.pointValue = 1000;

          //TODO: set hero properties
          this.moveSpeed = 115.0f;
          this.sightRange = 8;
          this.setType(State.EntityName.Hero);
          this.armor = 10;
          this.damage = 50;
          this.attackSpeed = 3;
          this.attackRange = 1;
          this.maxHealth = 500;
          this.health = 500;
      }

      public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
      {
          base.Update(gameTime, ref myRef);
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

      /// <summary>
      /// performs experience & level-up actions
      /// </summary>
      public override void killedTarget()
      {
          this.gainExperience(target.pointValue);

          base.killedTarget();
      }

      public void gainExperience(int exp){
          this.experiencePoints += exp;

          //CHECK FOR LEVEL-UP
          if (experiencePoints >= pointsToNextLevel)
          {
              level++;

              //TODO: Review this number - we might want to make it smaller
              pointsToNextLevel *= level;

              //TODO: Dispatch GameEvent
          }
      }

      public override void drawThyself(ref Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
      {
          {
              this.visible = true;
              this.justDrawn = true;
              Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(location);
              Rectangle unit = this.boundingBox;
              Color colorMask = Color.Gold;//EntityManager.getInstance().getColorMask(this.owner);

              //depending on the unit's state, draw their textures
              //idle
              if (this.currentState == State.UnitState.Idle)
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
              else if (this.currentState == State.UnitState.Attacking)
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
              else if (this.currentState == State.UnitState.Dead)
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
              else if (this.currentState == State.UnitState.Guarding)
              {
                  //TODO:
                  //Guarding Animation
              }
              else if (this.currentState == State.UnitState.Moving)
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
              else if (this.currentState == State.UnitState.UnderAttack)
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

      public void finishCapture(ControlPoint c){
          gainExperience(c.pointValue);
      }

    }
}

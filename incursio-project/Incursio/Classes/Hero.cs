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

      private static int healthIncrement = 25;
      private static int damageIncrement = 25;
      private static int armorIncrement = 5;

      public const int RESOURCE_TICK = 4;
      public int timeForResource = 0;

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
          this.damage = 25;
          this.attackSpeed = 3;
          this.attackRange = 1;
          this.maxHealth = 200;
          this.health = 200;
      }

      public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
      {
          base.Update(gameTime, ref myRef);

          this.updateResourceTick();
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

              //increment health
              this.maxHealth += Hero.healthIncrement;              
              //increment damage
              this.damage += Hero.damageIncrement;
              //increment defense
              this.armor += Hero.armorIncrement;

              PlayerManager.getInstance().notifyPlayer(this.owner,
                  new GameEvent(State.EventType.LEVEL_UP, "Hero Level Up!", location));

              //TODO: Review this number - we might want to make it smaller
              pointsToNextLevel *= level;
          }
      }

      /// <summary>
      /// Earns money for the player.  Additional money is computed as
      ///   double the Hero's Level
      /// </summary>
      public void updateResourceTick()
      {
          //give the owner money
          if (timeForResource >= RESOURCE_TICK * 60)
          {
              Player owningPlayer = PlayerManager.getInstance().getPlayerById(this.owner);
              timeForResource = 0;

              owningPlayer.MONETARY_UNIT += this.level * 2;
          }
          else
          {
              timeForResource++;
          }
      }

      public override void drawThyself(ref Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
      {
          {
              this.visible = true;
              this.justDrawn = true;
              Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(location);
              Rectangle unit = this.boundingBox;
              Color colorMask = Color.White;//EntityManager.getInstance().getColorMask(this.owner);

              //depending on the unit's state, draw their textures
              //idle
              if (this.currentState == State.UnitState.Idle)
              {
                  switch(this.directionState){
                      case State.Direction.Still:
                      case State.Direction.South:
                          spriteBatch.Draw(TextureBank.EntityTextures.heroSouth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroSouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroSouth.Height * 0.80),
                          TextureBank.EntityTextures.heroSouth.Width, TextureBank.EntityTextures.heroSouth.Height), colorMask);
                          break;

                      case State.Direction.East:
                          spriteBatch.Draw(TextureBank.EntityTextures.heroEast,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroEast.Height * 0.80),
                          TextureBank.EntityTextures.heroEast.Width, TextureBank.EntityTextures.heroEast.Height), colorMask);
                          break;

                      case State.Direction.West:
                          spriteBatch.Draw(TextureBank.EntityTextures.heroWest,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroWest.Height * 0.80),
                          TextureBank.EntityTextures.heroWest.Width, TextureBank.EntityTextures.heroWest.Height), colorMask);
                          break;

                      case State.Direction.North:
                          /*spriteBatch.Draw(TextureBank.EntityTextures.heroNorth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroNorth.Height * 0.80),
                          TextureBank.EntityTextures.heroNorth.Width, TextureBank.EntityTextures.heroNorth.Height), colorMask);*/
                          break;
                  }

              }
              else if (this.currentState == State.UnitState.Attacking)
              {
                  switch(this.directionState){
                      case State.Direction.East:
                      case State.Direction.South:
                          spriteBatch.Draw(TextureBank.EntityTextures.heroAttackingEast,
                          new Rectangle(onScreen.x - (int)(54 / 2), onScreen.y - (int)(50 * 0.80), 54, 50),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 54, 50), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.attackFramePause >= 4 || this.currentFrameXAttackDeath > 0)
                              {
                                  if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.heroAttackingEast.Width - 54)
                                  {
                                      this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 54;
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
                          spriteBatch.Draw(TextureBank.EntityTextures.heroAttackingWest,
                          new Rectangle(onScreen.x - (int)(54 / 2), onScreen.y - (int)(50 * 0.80), 54, 50),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 54, 50), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.attackFramePause >= 3 || this.currentFrameXAttackDeath > 0)
                              {
                                  if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.heroAttackingEast.Width - 54)
                                  {
                                      this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 54;
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
                          spriteBatch.Draw(TextureBank.EntityTextures.heroMovingEast,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroEast.Height * 0.80),
                          TextureBank.EntityTextures.heroEast.Width, TextureBank.EntityTextures.heroEast.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 40, 50), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heroMovingEast.Width - 40)
                              {
                                  this.currentFrameX = this.currentFrameX + 40;
                              }
                              else
                              {
                                  this.currentFrameX = 0;
                              }
                          }
                          break;

                      case State.Direction.West:
                          spriteBatch.Draw(TextureBank.EntityTextures.heroMovingWest,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroWest.Height * 0.80),
                          TextureBank.EntityTextures.heroWest.Width, TextureBank.EntityTextures.heroWest.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 40, 50), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heroMovingWest.Width - 40)
                              {
                                  this.currentFrameX = this.currentFrameX + 40;
                              }
                              else
                              {
                                  this.currentFrameX = 0;
                              }
                          }
                          break;

                      case State.Direction.South:
                          spriteBatch.Draw(TextureBank.EntityTextures.heroMovingSouth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroSouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroSouth.Height * 0.80),
                          TextureBank.EntityTextures.heroSouth.Width, TextureBank.EntityTextures.heroSouth.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 40, 50), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heroMovingSouth.Width - 40)
                              {
                                  this.currentFrameX = this.currentFrameX + 40;
                              }
                              else
                              {
                                  this.currentFrameX = 0;
                              }
                          }
                          break;

                      case State.Direction.North:
                          /*spriteBatch.Draw(TextureBank.EntityTextures.heroMovingNorth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroNorth.Height * 0.80),
                          TextureBank.EntityTextures.heroMovingNorth.Width, TextureBank.EntityTextures.heroMovingNorth.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 40, 50), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heroMovingNorth.Width - 40)
                              {
                                  this.currentFrameX = this.currentFrameX + 40;
                              }
                              else
                              {
                                  this.currentFrameX = 0;
                              }
                          }*/
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
                  spriteBatch.Draw(TextureBank.EntityTextures.heroSouth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroSouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroSouth.Height * 0.80),
                          TextureBank.EntityTextures.heroSouth.Width, TextureBank.EntityTextures.heroSouth.Height), colorMask);
              }
          }
      
      }

      public void finishCapture(ControlPoint c){
          gainExperience(c.pointValue);
      }

      public override bool isCapturing()
      {
          return this.currentState == State.UnitState.Capturing;
      }

    }
}

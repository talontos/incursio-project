using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Incursio.Utils;
using Incursio.Managers;

using System.Xml;
using System.Xml.Serialization;

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

      public String name = "Blargh, the Dragonsaver";
      public int level = 1;
      public int experiencePoints = 0;
      public int pointsToNextLevel = 1000;

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

      public void setHero_Badass()
      {
          this.pointValue = 1000;

          //set badass hero properties
          this.name = "Princess";
          this.moveSpeed = 115.0f;
          this.sightRange = 8;
          this.setType(State.EntityName.Hero);
          this.armor = 15;
          this.damage = 25;
          this.level = 15;
          this.experiencePoints = 10000;
          this.pointsToNextLevel = 20000;
          this.attackSpeed = 3;
          this.attackRange = 1;
          this.maxHealth = 500;
          this.health = 500;
      }

      public void copyHeroStats(Hero h){
          this.maxHealth = h.maxHealth;
          this.level = h.level;
          this.health = h.health;
          this.damage = h.damage;
          this.experiencePoints = h.experiencePoints;
          this.pointsToNextLevel = h.pointsToNextLevel;
          this.name = h.name;
          this.armor = h.armor;
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

      public double getPercentageLvlUp()
      {
          return (float)experiencePoints / pointsToNextLevel;
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
                  new GameEvent(State.EventType.LEVEL_UP, this, SoundCollection.MessageSounds.lvlUp, "Hero Level Up!", location));

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
              Color colorMask = EntityManager.getInstance().getColorMask(this.owner);

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
                          spriteBatch.Draw(TextureBank.EntityTextures.heroNorth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroNorth.Height * 0.80),
                          TextureBank.EntityTextures.heroNorth.Width, TextureBank.EntityTextures.heroNorth.Height), colorMask);
                          break;
                  }

              }
              else if (this.currentState == State.UnitState.Attacking)
              {
                  switch(this.directionState){
                      case State.Direction.East:
                      case State.Direction.South:
                          spriteBatch.Draw(TextureBank.EntityTextures.heroAttackingEast,
                          new Rectangle(onScreen.x - (int)(36 / 2), onScreen.y - (int)(40 * 0.80), 36, 40),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 36, 40), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              //if (this.attackFramePause >= 4 || this.currentFrameXAttackDeath > 0)
                              //{
                                  if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.heroAttackingEast.Width - 36)
                                  {
                                      this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 36;
                                  }
                                  else
                                  {
                                      this.currentFrameXAttackDeath = 0;
                                  }
                              //    this.attackFramePause = 0;
                              //}
                              //else
                              //{
                              //    this.attackFramePause++;
                              //}

                          }
                          break;

                      case State.Direction.North:
                      case State.Direction.West:
                          spriteBatch.Draw(TextureBank.EntityTextures.heroAttackingWest,
                          new Rectangle(onScreen.x - (int)(36 / 2), onScreen.y - (int)(40 * 0.80), 36, 40),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 36, 40), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              //if (this.attackFramePause >= 3 || this.currentFrameXAttackDeath > 0)
                              //{
                                  if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.heroAttackingEast.Width - 36)
                                  {
                                      this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 36;
                                  }
                                  else
                                  {
                                      this.currentFrameXAttackDeath = 0;
                                  }
                              //    this.attackFramePause = 0;
                              //}
                              //else
                              //{
                              //    this.attackFramePause++;
                              //}
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
                              spriteBatch.Draw(TextureBank.EntityTextures.heroDeathWest,
                              new Rectangle(onScreen.x - (int)(36 / 2), onScreen.y - (int)(40 * 0.80), 36, 40),
                              new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 36, 40), colorMask);

                              if (frameTimer >= FRAME_LENGTH)
                              {
                                  if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.heavyInfantryDeath_East.Width - 36)
                                  {
                                      this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 36;
                                  }
                                  else
                                  {
                                      this.playedDeath = true;
                                  }
                              }
                          }
                          else
                          {
                              spriteBatch.Draw(TextureBank.EntityTextures.heroDeathWest,
                              new Rectangle(onScreen.x - (int)(36 / 2), onScreen.y - (int)(40 * 0.80), 36, 40),
                              new Rectangle(144, 0, 36, 40), colorMask);
                          }
                          break;

                      case State.Direction.East:
                      case State.Direction.South:
                          if (!this.playedDeath)
                          {
                              spriteBatch.Draw(TextureBank.EntityTextures.heroDeathWest,
                              new Rectangle(onScreen.x - (int)(36 / 2), onScreen.y - (int)(40 * 0.80), 36, 40),
                              new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 36, 40), colorMask);

                              if (frameTimer >= FRAME_LENGTH)
                              {
                                  if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.heavyInfantryDeath_West.Width - 36)
                                  {
                                      this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 36;
                                  }
                                  else
                                  {
                                      this.playedDeath = true;
                                  }
                              }
                          }
                          else
                          {
                              spriteBatch.Draw(TextureBank.EntityTextures.heroDeathWest,
                              new Rectangle(onScreen.x - (int)(36 / 2), onScreen.y - (int)(40 * 0.80), 36, 40),
                              new Rectangle(144, 0, 36, 40), colorMask);
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
                          new Rectangle(this.currentFrameX, this.currentFrameY, 36, 40), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heroMovingEast.Width - 36)
                              {
                                  this.currentFrameX = this.currentFrameX + 36;
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
                          new Rectangle(this.currentFrameX, this.currentFrameY, 36, 40), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heroMovingWest.Width - 36)
                              {
                                  this.currentFrameX = this.currentFrameX + 36;
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
                          new Rectangle(this.currentFrameX, this.currentFrameY, 36, 40), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heroMovingSouth.Width - 36)
                              {
                                  this.currentFrameX = this.currentFrameX + 36;
                              }
                              else
                              {
                                  this.currentFrameX = 0;
                              }
                          }
                          break;

                      case State.Direction.North:
                          spriteBatch.Draw(TextureBank.EntityTextures.heroMovingNorth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.heroNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.heroNorth.Height * 0.80),
                          TextureBank.EntityTextures.heroNorth.Width, TextureBank.EntityTextures.heroNorth.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 36, 40), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameX < TextureBank.EntityTextures.heroMovingNorth.Width - 36)
                              {
                                  this.currentFrameX = this.currentFrameX + 36;
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

      protected override void notifyUnderAttack()
      {
          PlayerManager.getInstance().notifyPlayer(
              this.owner,
              new GameEvent(State.EventType.UNDER_ATTACK, this, SoundCollection.MessageSounds.heroAtt, "Hero under attack", this.location)
          );
      }

      public override void takeDamage(int damage, BaseGameEntity attacker)
      {
          base.takeDamage(damage, attacker);

          //notify close-to-death stuff
          if (this.health < (this.maxHealth * 0.3))
          {
              PlayerManager.getInstance().notifyPlayer(
                  this.owner,
                  new GameEvent(State.EventType.UNDER_ATTACK, this, SoundCollection.MessageSounds.heroLowHP, "Hero is about to die!", this.location)
              );
          }
          else if (this.health < (this.maxHealth * 0.6))
          {
              PlayerManager.getInstance().notifyPlayer(
                  this.owner,
                  new GameEvent(State.EventType.UNDER_ATTACK, this, SoundCollection.MessageSounds.heroBattle, "Hero is in Battle!", this.location)
              );
          }
      }

      public override void playOrderAttackSound()
      {
         if (owner != State.PlayerId.COMPUTER)
          SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(SoundCollection.VoiceSounds.HeroVoice.issueAttackOrder), false);
      }

      public override void playOrderMoveSound()
      {
         if (owner != State.PlayerId.COMPUTER)
          SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(SoundCollection.VoiceSounds.HeroVoice.issueMoveOrder), false);
      }

      public override void playDeathSound()
      {
          SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(SoundCollection.VoiceSounds.HeroVoice.death), false);
      }

      public override void playEnterBattlefieldSound()
      {
         if (owner != State.PlayerId.COMPUTER)
          SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(SoundCollection.VoiceSounds.HeroVoice.enterBattlefield), false);
      }

      public override void playSelectionSound()
      {
         if (owner != State.PlayerId.COMPUTER)
          SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(SoundCollection.VoiceSounds.HeroVoice.selection), false);
      }
    }
}

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
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Managers;

namespace Incursio.Classes
{
  public class GuardTowerStructure : Structure
    {
      protected byte alphaChan = 255;
      protected int destroyedTimer = 0;
      protected const int TIME_TILL_DESTROYED_FADE = 1;

      protected int damage = 20;
      protected int attackSpeed = 4;
      protected int updateAttackTimer = 0;
      protected int attackRange = 14;
      protected BaseGameEntity target = null;

      public GuardTowerStructure() : base(){
          this.pointValue = 250;

          this.maxHealth = 350;
          this.health = maxHealth;
          this.armor = 5;
          this.sightRange = 10;
          this.setType(State.EntityName.GuardTower);
          this.map = MapManager.getInstance().currentMap;
          this.currentState = State.EntityState.Idle;

          smartGuarding = false;
          canAttack = true;
      }

      public void attack(BaseGameEntity target)
      {
          if (target is Unit && ((target as Unit).getCurrentState() != State.EntityState.Dead && (target as Unit).getCurrentState() != State.EntityState.Buried))
          {
              this.currentState = State.EntityState.Attacking;
              this.target = target;
          }
          else if (target is Structure && (target as Structure).getCurrentState() != State.EntityState.Destroyed)
          {
              this.currentState = State.EntityState.Attacking;
              this.target = target;
          }
      }

      public override bool attackTarget()
      {
          //if target is in attackRange, attack it!
          if (map.getCellDistance(location, target.location) < attackRange)
          {
              if (this.updateAttackTimer == this.attackSpeed * 60)
              {
                  this.playAttackSound();
                  target.takeDamage(this.damage, this);

                  if (target is Unit && (target as Unit).getCurrentState() == State.EntityState.Dead ||
                       target is Structure && (target as Structure).getCurrentState() == State.EntityState.Destroyed)
                  {
                      target = null;
                      currentState = State.EntityState.Idle;
                  }

                  this.updateAttackTimer = 0;
              }
              else
              {
                  this.updateAttackTimer++;
              }

              //within range
              return true;
          }

          //outside range
          return false;
      }

      public override void setLocation(Coordinate coords)
      {
          updateOccupancy(false);
          base.setLocation(coords);
          updateOccupancy(true);
      }

      public override void updateOccupancy(bool occupied)
      {
          //hardcode blargh
          int xStart = location.x - 16;
          int yStart = location.y - (int)(32 * 0.80);
          int xEnd = location.x;// +16;
          int yEnd = location.y + (int)(32 * 0.20);

          if (xStart < 0 || xEnd < 0 || yStart < 0 || yEnd < 0)
              return;

          map.setSingleCellOccupancy(xStart, yStart, (byte)(occupied ? 0 : 1));
          map.setSingleCellOccupancy(xStart, yEnd, (byte)(occupied ? 0 : 1));
          map.setSingleCellOccupancy(xEnd, yStart, (byte)(occupied ? 0 : 1));
          map.setSingleCellOccupancy(xEnd, yEnd, (byte)(occupied ? 0 : 1));

          map.setSingleCellEntity(xStart, yStart, (byte)(occupied ? this.keyId : 1));
          map.setSingleCellEntity(xStart, yEnd, (byte)(occupied ? this.keyId : 1));
          map.setSingleCellEntity(xEnd, yStart, (byte)(occupied ? this.keyId : 1));
          map.setSingleCellEntity(xEnd, yEnd, (byte)(occupied ? this.keyId : 1));
      }

      public override void updateBounds()
      {
          Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.guardTowerTextureComputer;

          this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
              location.x - myRef.Width / 2,
              (int)(location.y - myRef.Height * 0.80),
              myRef.Width,
              myRef.Height
          );
      }

      public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
      {
          if (orders.Count == 0)
              EntityManager.getInstance().issueCommand_SingleEntity(State.Command.GUARD, false, this);

          base.Update(gameTime, ref myRef);
      }

      public override void setTarget(BaseGameEntity target)
      {
          this.target = target;
      }

      public override void setAttacking()
      {
          this.currentState = State.EntityState.Attacking;
      }

      public override bool isAttacking()
      {
          return this.currentState == State.EntityState.Attacking;
      }

      public override int getAttackDamage()
      {
          return this.damage;
      }

      public override int getAttackSpeed()
      {
          return this.attackSpeed;
      }

      public override int getArmor()
      {
          //TODO: STRUCTURE ARMOR
          return 0;
      }

      public override int getAttackRange()
      {
          return this.attackRange;
      }

      public override void drawThyself(ref SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
      {
          this.visible = true;
          this.justDrawn = false;
          Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(this.location);
          Rectangle unit = this.boundingBox;

          if (this.currentState == State.EntityState.BeingBuilt)
          {
              //TODO: draw construction?
          }
          else if (this.currentState == State.EntityState.Building)
          {
              if (this.getPlayer() == PlayerManager.getInstance().currentPlayerId)
              {
                  spriteBatch.Draw(TextureBank.EntityTextures.guardTowerTexturePlayer,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerTexturePlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.guardTowerTexturePlayer.Height * 0.80),
                      TextureBank.EntityTextures.guardTowerTexturePlayer.Width, TextureBank.EntityTextures.guardTowerTexturePlayer.Height), Color.White);
              }
              else
              {
                  spriteBatch.Draw(TextureBank.EntityTextures.guardTowerTextureComputer,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.guardTowerTextureComputer.Height * 0.80),
                      TextureBank.EntityTextures.guardTowerTextureComputer.Width, TextureBank.EntityTextures.guardTowerTextureComputer.Height), Color.White);
              }

          }
          else if (this.currentState == State.EntityState.Destroyed)
          {
              if(destroyedTimer < TIME_TILL_DESTROYED_FADE * 60){
                  if (alphaChan >= 0)
                  {
                      spriteBatch.Draw(TextureBank.EntityTextures.guardTowerExploded,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerExploded.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.guardTowerExploded.Height * 0.80),
                          TextureBank.EntityTextures.guardTowerExploded.Width, TextureBank.EntityTextures.guardTowerExploded.Height), new Color(255, 255, 255, alphaChan));
                      alphaChan -= 25;
                  }

                  destroyedTimer++;
              }
   
          }
          else if (this.currentState == State.EntityState.Idle || this.currentState == State.EntityState.Attacking)
          {
              if (this.getPlayer() == PlayerManager.getInstance().currentPlayerId)
              {
                  spriteBatch.Draw(TextureBank.EntityTextures.guardTowerTexturePlayer,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerTexturePlayer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.guardTowerTexturePlayer.Height * 0.80),
                      TextureBank.EntityTextures.guardTowerTexturePlayer.Width, TextureBank.EntityTextures.guardTowerTexturePlayer.Height), Color.White);
              }
              else
              {
                  spriteBatch.Draw(TextureBank.EntityTextures.guardTowerTextureComputer,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.guardTowerTextureComputer.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.guardTowerTextureComputer.Height * 0.80),
                      TextureBank.EntityTextures.guardTowerTextureComputer.Width, TextureBank.EntityTextures.guardTowerTextureComputer.Height), Color.White);
              }
          }
      }

      public virtual void playAttackSound()
      {
          SoundManager.getInstance().PlaySound(SoundCollection.AttackSounds.ArrowAttack, false);
      }
    }
}

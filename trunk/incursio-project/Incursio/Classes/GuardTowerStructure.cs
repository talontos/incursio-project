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
      protected int damage = 20;
      protected int attackSpeed = 4;
      protected int updateAttackTimer = 0;
      protected int attackRange = 14;
      protected BaseGameEntity target = null;

        public GuardTowerStructure() : base(){
            this.pointValue = 250;

            this.maxHealth = 350;
            this.health = maxHealth;
            this.sightRange = 10;
            this.setType(State.EntityName.GuardTower);
            this.map = Incursio.getInstance().currentMap;
            this.currentState = State.StructureState.Idle;

            smartGuarding = false;
            canAttack = true;
        }

      public override void build(BaseGameEntity toBeBuilt)
      {
          //guardtower upgrades?
          base.build(toBeBuilt);
      }

      public override void buildTick()
      {
          base.buildTick();
      }

      public void attack(BaseGameEntity target)
      {
          if (target is Unit && ((target as Unit).getCurrentState() != State.UnitState.Dead && (target as Unit).getCurrentState() != State.UnitState.Buried))
          {
              this.currentState = State.StructureState.Attacking;
              this.target = target;
          }
          else if (target is Structure && (target as Structure).getCurrentState() != State.StructureState.Destroyed)
          {
              this.currentState = State.StructureState.Attacking;
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
                  target.takeDamage(this.damage, this);

                  if (target is Unit && (target as Unit).getCurrentState() == State.UnitState.Dead ||
                       target is Structure && (target as Structure).getCurrentState() == State.StructureState.Destroyed)
                  {
                      //TODO:
                      //add AI for attacking more enemies!
                      //but for now:
                      target = null;
                      currentState = State.StructureState.Idle;
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
          this.currentState = State.StructureState.Attacking;
      }

      public override bool isAttacking()
      {
          return this.currentState == State.StructureState.Attacking;
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
          //onScreen = currentMap.positionOnScreen(this.getLocation());
          //Rectangle unit = new Rectangle(this.getLocation().x, this.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());
          Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(this.location);
          Rectangle unit = this.boundingBox;

          if (this.currentState == State.StructureState.BeingBuilt)
          {
              //TODO: draw construction?
          }
          else if (this.currentState == State.StructureState.Building)
          {
              //TODO: draw something special for when the structure is building something (fires flickering or w/e)
              if (this.getPlayer() == State.PlayerId.HUMAN)
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
          else if (this.currentState == State.StructureState.Destroyed)
          {
              //TODO: building asploded
          }
          else if (this.currentState == State.StructureState.Idle || this.currentState == State.StructureState.Attacking)
          {
              if (this.getPlayer() == State.PlayerId.HUMAN)
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
    }
}

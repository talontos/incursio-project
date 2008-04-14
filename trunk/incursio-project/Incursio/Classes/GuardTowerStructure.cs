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
      protected int damage = 10;
      protected int attackSpeed = 1;
      protected int updateAttackTimer = 0;
      protected int attackRange = 10;
      protected BaseGameEntity target = null;

        public GuardTowerStructure() : base(){
            this.maxHealth = 350;
            this.health = maxHealth;
            this.sightRange = 10;
            this.setType(State.EntityName.GuardTower);
            this.map = Incursio.getInstance().currentMap;
            this.currentState = State.StructureState.Idle;

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
    }
}

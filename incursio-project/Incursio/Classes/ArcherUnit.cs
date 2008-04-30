using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Managers;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Classes
{
  public class ArcherUnit : Unit
    {
      public static String CLASSNAME = "Incursio.Classes.ArcherUnit";
      
      //ARROW STUFF//
      private Vector2 arrowPos = new Vector2(-1, -1);
      const int ARROW_LENGTH = 5;
      const int ARROW_SPEED = 5;
      bool drawArrow = false;
      Coordinate arrowOnScreen = new Coordinate(-1, -1);
      double arrowAngle = 0;
      private Vector2 arrowDestination = new Vector2(-1, -1);

        public ArcherUnit() : base(){
            this.pointValue = 75;

            //TODO: set Archer Values
            this.armor = 1;
            this.damage = 20;
            this.moveSpeed = 150.0f;
            this.attackSpeed = 3;
            this.sightRange = 12;
            this.attackRange = 10;
            this.maxHealth = 100;
            this.health = 100;
            this.setType(State.EntityName.Archer);

        }

      public override void Update(Microsoft.Xna.Framework.GameTime gameTime, ref BaseGameEntity myRef)
      {
          base.Update(gameTime, ref myRef);

          if (drawArrow)
          {
              updateArrow();
              if(MapManager.getInstance().currentMap.isOnScreen(new Coordinate((int)arrowPos.X, (int)arrowPos.Y)))
              {
                  arrowOnScreen = MapManager.getInstance().currentMap.positionOnScreen(new Coordinate((int)arrowPos.X, (int)arrowPos.Y));
              }
              
          }
      }

      public override string getTextureName()
      {
          return @"archerUnit";
      }

      public override bool attackTarget()
      {
          //if target is in attackRange, attack it.

          int largeTargetBufferZone = 0;

          if (target.getType() == State.EntityName.Camp)
          {
              largeTargetBufferZone = (int)(64 / map.getTileWidth());
          }
          else if (target.getType() == State.EntityName.GuardTower)
          {
              largeTargetBufferZone = (int)(64 / map.getTileWidth());
          }

          if (MapManager.getInstance().currentMap.getCellDistance(location, target.location) <= attackRange + largeTargetBufferZone)
          {
              //TODO: do some math randomizing damage?
              if (this.updateAttackTimer == 0)    //this is the unit's attack time (attack every 1.5 seconds for example)
              {

                  if (target.getLocation().x > this.location.x)
                  {
                      this.directionState = State.Direction.East;
                  }
                  else if (target.getLocation().x < this.location.x)
                  {
                      this.directionState = State.Direction.West;
                  }

                  target.takeDamage(this.damage, this);
                  
                  //SHOOT AN ARROW DAWG
                  startArrow();


                  //if we just killed the thing

                  //if (target is Unit && (target as Unit).getCurrentState() == State.UnitState.Dead ||
                  //   target is Structure && (target as Structure).getCurrentState() == State.StructureState.Destroyed)
                  if (target.isDead())
                  {
                      //TODO:
                      //add AI for attacking more enemies!
                      //but for now:

                      //NOTE: killedTarget needs to be performed BEFORE
                      //  target is set to null so that we know WHAT we killed
                      this.killedTarget();

                      target = null;
                      destination = null;
                      currentState = State.UnitState.Idle;
                  }


                  this.updateAttackTimer = this.attackSpeed * 60;
              }
              else
              {
                  this.updateAttackTimer--;
              }

              return true;
          }
          else return false;// updateMovement();
      }

      public void updateArrow()
      {
          double newPosX = /*(-1) **/ (Math.Cos(arrowAngle * (Math.PI / 180)) * ARROW_SPEED);
          double newPosY = (-1) * (Math.Sin(arrowAngle * (Math.PI / 180)) * ARROW_SPEED); //INVERT TO COMPENSATE FOR PIXEL GROWTH DIRECTION

          //determine the direction, and if we are close enough to the destination, end movement
          if (arrowPos.X > arrowDestination.X)
          {
              if (arrowPos.X + newPosX < arrowDestination.X)
              {
                  drawArrow = false;
              }
          }
          else if (arrowPos.X < arrowDestination.X)
          {
              if (arrowPos.X + newPosX > arrowDestination.X)
              {
                  drawArrow = false;
              }
          }

          arrowPos.X = arrowPos.X + (float)newPosX;
          arrowPos.Y = arrowPos.Y + (float)newPosY;
      }

      public void startArrow()
      {
          if (!drawArrow)
          {
              drawArrow = true;
              arrowPos.X = (float)location.x;
              arrowPos.Y = (float)location.y;              
              arrowDestination.X = (float)target.location.x;
              arrowDestination.Y = (float)target.location.y;
              arrowOnScreen.x = -1;
              arrowOnScreen.y = -1;

              //find angle between arrow and destination
              //straight shots first
              if (arrowPos.Y == arrowDestination.Y && arrowPos.X == arrowDestination.X)
              {
                  arrowAngle = 0;
              }
              else if (arrowPos.Y == arrowDestination.Y && arrowPos.X > arrowDestination.X)
              {
                  arrowAngle = 180;
              }
              else if (arrowPos.X == arrowDestination.X && arrowPos.Y > arrowDestination.Y)
              {
                  arrowAngle = 90;
              }
              else if (arrowPos.X == arrowDestination.X && arrowPos.Y < arrowDestination.Y)
              {
                  arrowAngle = 270;
              }
              //if none of those, do based off quadrants
              else
              {
                  //quadrant one
                  if (arrowPos.X < arrowDestination.X && arrowPos.Y > arrowDestination.Y)
                  {
                      arrowAngle = (180 / Math.PI) * Math.Atan((arrowPos.Y - arrowDestination.Y) / (arrowDestination.X - arrowPos.X));
                  }
                  //quadrant two
                  else if (arrowPos.X > arrowDestination.X && arrowPos.Y > arrowDestination.Y)
                  {
                      arrowAngle = 90 + (90 - (180 / Math.PI) * Math.Atan((arrowPos.Y - arrowDestination.Y) / (arrowPos.X - arrowDestination.X)));
                  }
                  //quadrant three
                  else if (arrowPos.X > arrowDestination.X && arrowPos.Y < arrowDestination.Y)
                  {
                      arrowAngle = 180 + (180 / Math.PI) * Math.Atan((arrowDestination.Y - arrowPos.Y) / (arrowPos.X - arrowDestination.X));
                  }
                  //quadrant four
                  else if (arrowPos.X < arrowDestination.X && arrowPos.Y < arrowDestination.Y)
                  {
                      arrowAngle = 270 + (90 - (180 / Math.PI)* Math.Atan((arrowDestination.Y - arrowPos.Y) / (arrowDestination.X - arrowPos.X)));
                  }
              }
          }

          
      }

      public override void updateBounds()
      {
          Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.archerSouth;

          this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
              location.x - myRef.Width / 2,
              (int)(location.y - myRef.Height * 0.80),
              myRef.Width,
              myRef.Height
          );
      }

      public override void drawThyself(ref Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
      {
          this.visible = true;
          this.justDrawn = false;
          Coordinate onScreen = MapManager.getInstance().currentMap.positionOnScreen(this.location);
          Rectangle unit = this.boundingBox;
          Color colorMask = EntityManager.getInstance().getColorMask(this.owner);

          //draw the arrow if needed
          if (drawArrow)
          {
              spriteBatch.Draw(TextureBank.EntityTextures.arrow, 
                  new Vector2(arrowOnScreen.x, arrowOnScreen.y),
                  null, Color.White, -1 * ((float)(arrowAngle * (Math.PI / 180))), new Vector2(TextureBank.EntityTextures.arrow.Width / 2, TextureBank.EntityTextures.arrow.Height / 2), 1.0f, SpriteEffects.None, 0f);
          }

          //depending on the unit's state, draw their textures
          //idle
          if (this.currentState == State.UnitState.Idle)
          {
              switch(this.directionState){
                  case State.Direction.South:
                  case State.Direction.Still:
                      spriteBatch.Draw(TextureBank.EntityTextures.archerSouth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerSouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerSouth.Height * 0.80),
                          TextureBank.EntityTextures.archerSouth.Width, TextureBank.EntityTextures.archerSouth.Height), colorMask);
                      break;

                  case State.Direction.East:
                      spriteBatch.Draw(TextureBank.EntityTextures.archerEast,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerEast.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerEast.Height * 0.80),
                          TextureBank.EntityTextures.archerEast.Width, TextureBank.EntityTextures.archerEast.Height), colorMask);
                      break;

                  case State.Direction.West:
                      spriteBatch.Draw(TextureBank.EntityTextures.archerWest,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerWest.Height * 0.80),
                          TextureBank.EntityTextures.archerWest.Width, TextureBank.EntityTextures.archerWest.Height), colorMask);
                      break;

                  case State.Direction.North:
                      spriteBatch.Draw(TextureBank.EntityTextures.archerNorth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerNorth.Height * 0.80),
                          TextureBank.EntityTextures.archerNorth.Width, TextureBank.EntityTextures.archerNorth.Height), colorMask);
                      break;
              }

          }
          else if (this.currentState == State.UnitState.Attacking)
          {

              switch(this.directionState){
                  case State.Direction.West:
                  case State.Direction.North:
                      spriteBatch.Draw(TextureBank.EntityTextures.archerAttackingWest,
                          new Rectangle(onScreen.x - (25 / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerWest.Height * 0.80),
                          TextureBank.EntityTextures.archerWest.Width, TextureBank.EntityTextures.archerWest.Height),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 25, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.archerAttackingWest.Width - 25)
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
                      spriteBatch.Draw(TextureBank.EntityTextures.archerAttackingEast,
                          new Rectangle(onScreen.x - (25 / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerEast.Height * 0.80),
                          TextureBank.EntityTextures.archerEast.Width, TextureBank.EntityTextures.archerEast.Height),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 25, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.archerAttackingEast.Width - 25)
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
                          spriteBatch.Draw(TextureBank.EntityTextures.archerDeathEast,
                          new Rectangle(onScreen.x - (int)(33 / 2), onScreen.y - (int)(30 * 0.80), 33, 30),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 33, 30), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.archerDeathEast.Width - 33)
                              {
                                  this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 33;
                              }
                              else
                              {
                                  this.playedDeath = true;
                              }
                          }
                      }
                      else
                      {
                          spriteBatch.Draw(TextureBank.EntityTextures.archerDeathEast,
                          new Rectangle(onScreen.x - (int)(33 / 2), onScreen.y - (int)(30 * 0.80), 33, 30),
                          new Rectangle(66, 0, 33, 30), colorMask);
                      }
                      break;

                  case State.Direction.East:
                  case State.Direction.South:
                      if (!this.playedDeath)
                      {
                          spriteBatch.Draw(TextureBank.EntityTextures.archerDeathWest,
                          new Rectangle(onScreen.x - (int)(33 / 2), onScreen.y - (int)(30 * 0.80), 33, 30),
                          new Rectangle(this.currentFrameXAttackDeath, this.currentFrameYAttackDeath, 33, 30), colorMask);

                          if (frameTimer >= FRAME_LENGTH)
                          {
                              if (this.currentFrameXAttackDeath < TextureBank.EntityTextures.archerDeathWest.Width - 33)
                              {
                                  this.currentFrameXAttackDeath = this.currentFrameXAttackDeath + 33;
                              }
                              else
                              {
                                  this.playedDeath = true;
                              }
                          }
                      }
                      else
                      {
                          spriteBatch.Draw(TextureBank.EntityTextures.archerDeathWest,
                          new Rectangle(onScreen.x - (int)(33 / 2), onScreen.y - (int)(30 * 0.80), 33, 30),
                          new Rectangle(66, 0, 33, 30), colorMask);
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
                      spriteBatch.Draw(TextureBank.EntityTextures.archerMovingWest,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerWest.Height * 0.80),
                          TextureBank.EntityTextures.archerWest.Width, TextureBank.EntityTextures.archerWest.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameX < TextureBank.EntityTextures.archerMovingWest.Width - 20)
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
                      spriteBatch.Draw(TextureBank.EntityTextures.archerMovingEast,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerWest.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerWest.Height * 0.80),
                          TextureBank.EntityTextures.archerWest.Width, TextureBank.EntityTextures.archerWest.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameX < TextureBank.EntityTextures.archerMovingEast.Width - 20)
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
                      spriteBatch.Draw(TextureBank.EntityTextures.archerMovingSouth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerSouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerSouth.Height * 0.80),
                          TextureBank.EntityTextures.archerSouth.Width, TextureBank.EntityTextures.archerSouth.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameX < TextureBank.EntityTextures.archerMovingSouth.Width - 20)
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
                      spriteBatch.Draw(TextureBank.EntityTextures.archerMovingNorth,
                          new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerNorth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerNorth.Height * 0.80),
                          TextureBank.EntityTextures.archerNorth.Width, TextureBank.EntityTextures.archerNorth.Height),
                          new Rectangle(this.currentFrameX, this.currentFrameY, 20, 30), colorMask);

                      if (frameTimer >= FRAME_LENGTH)
                      {
                          if (this.currentFrameX < TextureBank.EntityTextures.archerMovingNorth.Width - 20)
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
          }
          else
          {
              spriteBatch.Draw(TextureBank.EntityTextures.archerSouth,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerSouth.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerSouth.Height * 0.80),
                      TextureBank.EntityTextures.archerSouth.Width, TextureBank.EntityTextures.archerSouth.Height), colorMask);
          }
      }

      public override void playAttackSound()
      {
          SoundManager.getInstance().PlaySound(SoundBank.AttackSounds.ArrowAttack, false);
      }
    }
}

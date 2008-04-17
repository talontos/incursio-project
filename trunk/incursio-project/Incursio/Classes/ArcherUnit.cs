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
        public ArcherUnit() : base(){
            this.pointValue = 75;

            //TODO: set Archer Values
            this.armor = 1;
            this.damage = 10;
            this.moveSpeed = 150.0f;
            this.attackSpeed = 3;
            this.sightRange = 12;
            this.attackRange = 10;
            this.setType(State.EntityName.Archer);

        }

      public override void Update(Microsoft.Xna.Framework.GameTime gameTime, ref BaseGameEntity myRef)
      {
          base.Update(gameTime, ref myRef);
      }

      public override string getTextureName()
      {
          return @"archerUnit";
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
              SoundManager.getInstance().PlaySound("../../../Content/Audio/bow release.wav", false);

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
              spriteBatch.Draw(TextureBank.EntityTextures.archerDead,
                      new Rectangle(onScreen.x - (TextureBank.EntityTextures.archerDead.Width / 2), onScreen.y - (int)(TextureBank.EntityTextures.archerDead.Height * 0.80),
                      TextureBank.EntityTextures.archerDead.Width, TextureBank.EntityTextures.archerDead.Height), colorMask);
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
    }
}

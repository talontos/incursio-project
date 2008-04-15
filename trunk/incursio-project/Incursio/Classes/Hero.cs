using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Incursio.Classes
{
    // the colon is the java equivalent of 'extends'
  public class Hero : Unit
    {
      public static String HERO_CLASS = "Incursio.Classes.Hero";

      private String name = "";
      private int level = 1;
      private long experiencePoints = 0;
      private long pointsToNextLevel = 1000;

      public Hero() : base(){
          //TODO: set hero properties
          this.moveSpeed = 150.0f;
          this.sightRange = 15;
          this.setType(State.EntityName.Hero);
      }

      public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
      {
          base.Update(gameTime, ref myRef);
      }

      /// <summary>
      /// performs experience & level-up actions
      /// </summary>
      public override void killedTarget()
      {
          this.experiencePoints += target.pointValue;

          //CHECK FOR LEVEL-UP
          if(experiencePoints >= pointsToNextLevel){
              level++;

              //TODO: Review this number - we might want to make it smaller
              pointsToNextLevel *= level;

              //TODO: Dispatch GameEvent
          }

          base.killedTarget();
      }

      public override void drawThyself(ref Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, int frameTimer, int FRAME_LENGTH)
      {
          //TODO: once we get hero textures, uncomment AND REFORMAT this block
          /*if (e.getType() == State.EntityName.Hero)
          {
              e.visible = true;
              onScreen = currentMap.positionOnScreen(e.getLocation());
              Rectangle unit = new Rectangle(e.getLocation().x, e.getLocation().y, currentMap.getTileWidth(), currentMap.getTileHeight());

              //depending on the unit's state, draw their textures
              //idle
              if ((e as Hero).getCurrentState() == State.UnitState.Idle)
              {
                  //south or idle
                  if ((e as Hero).getDirection() == State.Direction.Still || (e as Hero).getDirection() == State.Direction.South)
                  {
                      spriteBatch.Draw(this.heroSouth,
                          new Rectangle(onScreen.x - (this.heroSouth.Width / 2), onScreen.y - (int)(this.heroSouth.Height * 0.80),
                          this.heroSouth.Width, this.heroSouth.Height), Color.White);
                  }
                  //east
                  else if ((e as Hero).getDirection() == State.Direction.East)
                  {
                      spriteBatch.Draw(this.heroEast,
                          new Rectangle(onScreen.x - (this.heroEast.Width / 2), onScreen.y - (int)(this.heroEast.Height * 0.80),
                          this.heroEast.Width, this.heroEast.Height), Color.White);
                  }
                  //west
                  else if ((e as Hero).getDirection() == State.Direction.West)
                  {
                      spriteBatch.Draw(this.heroWest,
                          new Rectangle(onScreen.x - (this.heroWest.Width / 2), onScreen.y - (int)(this.heroWest.Height * 0.80),
                          this.heroWest.Width, this.heroWest.Height), Color.White);
                  }
                  //north
                  else if ((e as Hero).getDirection() == State.Direction.North)
                  {
                      spriteBatch.Draw(this.heroNorth,
                          new Rectangle(onScreen.x - (this.heroNorth.Width / 2), onScreen.y - (int)(this.heroNorth.Height * 0.80),
                          this.heroNorth.Width, this.heroNorth.Height), Color.White);
                  }

              }
              else if ((e as Hero).getCurrentState() == State.UnitState.Attacking)
              {
                  //TODO:
                  //Attacking Animation
              }
              else if ((e as Hero).getCurrentState() == State.UnitState.Dead)
              {
                  //TODO:
                  //Dead stuff
                  //with hero death, end the current map in defeat for player hero, victory if computer hero
              }
              else if ((e as Hero).getCurrentState() == State.UnitState.Guarding)
              {
                  //TODO:
                  //Guarding Animation
              }
              else if ((e as Hero).getCurrentState() == State.UnitState.Moving)
              {
                  //TODO:
                  //Moving Animation
              }
              else if ((e as Hero).getCurrentState() == State.UnitState.UnderAttack)
              {
                  //TODO:
                  //Under Attack Animation
              }
              else
              {
                  spriteBatch.Draw(this.heroSouth,
                          new Rectangle(onScreen.x - (this.heroSouth.Width / 2), onScreen.y - (int)(this.heroSouth.Height * 0.80),
                          this.heroSouth.Width, this.heroSouth.Height), Color.White);
              }
          }*/
      }

    }
}

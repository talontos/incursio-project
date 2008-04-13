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
          }

          base.killedTarget();
      }

    }
}

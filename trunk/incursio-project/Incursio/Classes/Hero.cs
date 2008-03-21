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
        private int level = 0;
        private long experiencePoints = 0;

        public Hero() : base(){
            //TODO: set hero properties
            this.setType(State.EntityName.Hero);
        }

        public override void Update(GameTime gameTime, ref BaseGameEntity myRef)
        {
            base.Update(gameTime, ref myRef);
        }
    }
}

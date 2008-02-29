using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Classes
{
    // the colon is the java equivalent of 'extends'
  public class Hero : Unit
    {
        private String name = "";
        private int level = 0;
        private long experiencePoints = 0;

        public Hero() : base(){
            //TODO: set hero properties
            this.setType(State.EntityName.Hero);
        }
    }
}

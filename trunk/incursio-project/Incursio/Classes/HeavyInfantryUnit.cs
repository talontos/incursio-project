using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Classes
{
  public class HeavyInfantryUnit : Unit
    {

      public static String CLASSNAME = "Incursio.Classes.HeavyInfantryUnit";

        public HeavyInfantryUnit() : base(){
            //TODO: set values
            this.setType(State.EntityName.HeavyInfantry);
        }
    }
}

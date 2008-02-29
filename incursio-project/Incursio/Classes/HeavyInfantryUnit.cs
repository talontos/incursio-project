using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Classes
{
  public class HeavyInfantryUnit : Unit
    {
        public HeavyInfantryUnit() : base(){
            //TODO: set values
            this.setType(State.EntityName.HeavyInfantry);
        }
    }
}

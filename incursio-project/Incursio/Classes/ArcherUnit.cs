using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Classes
{
  public class ArcherUnit : Unit
    {
        public ArcherUnit() : base(){
            //TODO: set Archer Values
            this.armor = 1;
            this.damage = 1;
            this.speed = 3;
            this.sightRange = 10;
            this.setType(State.EntityName.Archer);
        }

      public override string getTextureName()
      {
          return @"archerUnit";
      }
    }
}

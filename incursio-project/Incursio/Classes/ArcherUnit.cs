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
            this.damage = 10;
            this.speed = 3;
            this.attackSpeed = 3;
            this.sightRange = 150;
            this.attackRange = 10;
            this.setType(State.EntityName.Archer);
        }

      public override string getTextureName()
      {
          return @"archerUnit";
      }
    }
}

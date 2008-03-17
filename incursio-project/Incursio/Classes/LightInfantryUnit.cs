using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Classes
{
  public class LightInfantryUnit : Unit
    {
        public LightInfantryUnit() : base(){
            //TODO: set LightInfantry Values
            this.armor = 1;
            this.damage = 25;
            this.speed = 3;
            this.sightRange = 20;
            this.attackSpeed = 2;
            this.attackRange = 3;
            this.setType(State.EntityName.LightInfantry);
        }

      public override string getTextureName()
      {
          return @"infantryUnit";
      }
    }
}

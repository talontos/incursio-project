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
            this.damage = 1;
            this.speed = 3;
            this.sightRange = 5;
            this.setType(State.EntityName.LightInfantry);
        }
    }
}

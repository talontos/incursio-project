using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Managers;

namespace Incursio.Classes
{
  public class HeavyInfantryUnit : Unit
    {

      public static String CLASSNAME = "Incursio.Classes.HeavyInfantryUnit";

        public HeavyInfantryUnit() : base(){
            //TODO: set values
            this.moveSpeed = 100.0f;
            this.armor = 5;
            this.damage = 30;
            //this.speed = 2;
            this.sightRange = 8;
            this.attackSpeed = 2;
            this.attackRange = 1;
            this.maxHealth = 150;
            this.health = 150;

            this.setType(State.EntityName.HeavyInfantry);
        }

      public override void updateBounds()
      {
          Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.heavyInfantrySouth;

          this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
              location.x - myRef.Width / 2,
              (int)(location.y - myRef.Height * 0.80),
              myRef.Width,
              myRef.Height
          );
      }
    }
}

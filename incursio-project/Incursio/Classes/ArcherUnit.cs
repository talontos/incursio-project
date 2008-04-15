using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Managers;

namespace Incursio.Classes
{
  public class ArcherUnit : Unit
    {
      public static String CLASSNAME = "Incursio.Classes.ArcherUnit";
        public ArcherUnit() : base(){
            //TODO: set Archer Values
            this.armor = 1;
            this.damage = 10;
            this.moveSpeed = 150.0f;
            this.attackSpeed = 3;
            this.sightRange = 12;
            this.attackRange = 10;
            this.setType(State.EntityName.Archer);

        }

      public override void Update(Microsoft.Xna.Framework.GameTime gameTime, ref BaseGameEntity myRef)
      {
          base.Update(gameTime, ref myRef);
      }

      public override string getTextureName()
      {
          return @"archerUnit";
      }

      public override void updateBounds()
      {
          Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.archerSouth;

          this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
              location.x - myRef.Width / 2,
              (int)(location.y - myRef.Height * 0.80),
              myRef.Width,
              myRef.Height
          );
      }
    }
}

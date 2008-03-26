using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

using Incursio.Managers;

namespace Incursio.Classes
{
  public class LightInfantryUnit : Unit
    {
      public static string CLASSNAME = "Incursio.Classes.LightInfantryUnit";

      public LightInfantryUnit() : base(){
          //TODO: set LightInfantry Values
          this.armor = 1;
          this.damage = 25;
          this.speed = 3;
          this.sightRange = 20;
          this.attackSpeed = 2;
          this.attackRange = 2;
          this.setType(State.EntityName.LightInfantry);
      }

      public override string getTextureName()
      {
          return @"infantryUnit";
      }

      public override void updateBounds()
      {
          Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.lightInfantrySouth;

          this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
              location.x - myRef.Width / 2,
              (int)(location.y - myRef.Height * 0.80),
              myRef.Width,
              myRef.Height
          );
      }
    }
}
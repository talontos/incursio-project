using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Classes
{
    public class ControlPoint : Structure
    {
        public ControlPoint() : base(){
            //TODO: set controlpoint values
            this.setType(State.EntityName.ControlPoint);
        }

        public override void updateBounds()
        {
            /*
            Microsoft.Xna.Framework.Graphics.Texture2D myRef = TextureBank.EntityTextures.archerSouth;

            this.boundingBox = new Microsoft.Xna.Framework.Rectangle(
                location.x - myRef.Width / 2,
                location.y - myRef.Height * 0.80,
                myRef.Width,
                myRef.Height
            );
            */
        }
    }
}

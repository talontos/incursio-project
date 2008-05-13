using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities.Components
{
    public class MovementComponent : BaseComponent
    {
        public bool canMove = false;

        public MovementComponent(){

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, ref BaseGameEntity entity)
        {
            base.Update(gameTime, ref entity);
        }
    }
}

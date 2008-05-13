using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities.Components
{
    public class FactoryComponent : BaseComponent
    {
        public bool isConstructor = false;

        public FactoryComponent(){

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, ref BaseGameEntity entity)
        {
            base.Update(gameTime, ref entity);
        }
    }
}

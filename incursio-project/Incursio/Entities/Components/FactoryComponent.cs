using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;

namespace Incursio.Entities.Components
{
    public class FactoryComponent : BaseComponent
    {
        public bool isConstructor = false;

        public FactoryComponent(BaseGameEntity entity) : base(entity){

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}

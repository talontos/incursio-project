using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities.Components
{
    public class CombatComponent : BaseComponent
    {
        public bool canAttack = false;
        public bool smartGuarding = true;

        public CombatComponent(){

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, ref BaseGameEntity entity)
        {
            base.Update(gameTime, ref entity);
        }
    }
}

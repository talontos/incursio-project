using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;

namespace Incursio.Entities.Components
{
    public class CombatComponent : BaseComponent
    {
        public bool canAttack = false;
        public bool smartGuarding = true;

        public CombatComponent(ref BaseGameEntity entity) : base(entity){

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime); 
        }
    }
}

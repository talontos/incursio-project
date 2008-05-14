using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;
using Incursio.Managers;

namespace Incursio.Entities.Components
{
    public class HealComponent : BaseComponent
    {
        public int HEAL_TICK = 4;
        public int healTimer = 0;
        public int healRange = 4;
        public int healthBoost = 10;

        public HealComponent(BaseGameEntity e) : base(e){

        }

        public override void setAttributes(List<KeyValuePair<string, object>> attributes)
        {
            base.setAttributes(attributes);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.healTimer == HEAL_TICK * 90)
            {
                EntityManager.getInstance().healEntitiesInRange(this.bgEntity, this.healRange, this.healthBoost, true);
                healTimer = 0;
            }
            else if (this.healTimer == HEAL_TICK * 60)
            {
                //heal units in range
                EntityManager.getInstance().healEntitiesInRange(this.bgEntity, this.healRange, this.healthBoost, false);
                healTimer++;
            }
            else
            {
                healTimer++;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

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

        public override void setAttributes(List<KeyValuePair<string, string>> attributes)
        {
            base.setAttributes(attributes);

            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i].Key.ToUpper())
                {
                    case "TICKSPEED":   HEAL_TICK   = int.Parse(attributes[i].Value); break;
                    case "HEALRANGE":
                    case "RANGE":       healRange   = int.Parse(attributes[i].Value); break;
                    case "HEALTHBOOST": 
                    case "BOOST":       healthBoost = int.Parse(attributes[i].Value); break;
                    default: break;
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.healTimer == HEAL_TICK * 90)
            {
                //heal all units in range
                EntityManager.getInstance().healEntitiesInRange(this.bgEntity, this.healRange, this.healthBoost, true);
                healTimer = 0;
            }
            else if (this.healTimer == HEAL_TICK * 60)
            {
                //heal heros in range
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

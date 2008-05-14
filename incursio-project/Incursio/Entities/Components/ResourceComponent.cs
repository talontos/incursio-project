using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;
using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Entities.Components
{
    public class ResourceComponent : BaseComponent
    {
        public int RESOURCE_TICK = 4;
        public int timeForResource = 0;
        public int income = 8;

        public ResourceComponent(BaseGameEntity e) : base(e){

        }

        public override void setAttributes(List<KeyValuePair<string, object>> attributes)
        {
            base.setAttributes(attributes);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            this.updateResourceTick();
        }

        public void updateResourceTick(){
            //give the owner money
            if (timeForResource >= RESOURCE_TICK * 60)
            {
                timeForResource = 0;
                if (this.bgEntity.owner == State.PlayerId.HUMAN)
                {
                    MessageManager.getInstance().addMessage(new GameEvent(State.EventType.GAIN_RESOURCE, this.bgEntity, "", Convert.ToString(income), this.bgEntity.location));

                    PlayerManager.getInstance().humanPlayer.MONETARY_UNIT = PlayerManager.getInstance().humanPlayer.MONETARY_UNIT + income;
                }
                else
                {
                    PlayerManager.getInstance().computerPlayer.MONETARY_UNIT = PlayerManager.getInstance().computerPlayer.MONETARY_UNIT + income;
                }
            }
            else
            {
                timeForResource++;
            }
        }
    }
}

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
        public int delay = 4;
        public int timeForResource = 0;
        public int income = 8;

        public ResourceComponent(BaseGameEntity e) : base(e){

        }

        public override void setAttributes(List<KeyValuePair<string, string>> attributes)
        {
            base.setAttributes(attributes);

            for(int i = 0; i < attributes.Count; i++){
                switch(attributes[i].Key){
                    case "income": this.income = int.Parse(attributes[i].Value); break;
                    case "delay": this.delay = int.Parse(attributes[i].Value); break;
                    default: break;
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            this.updateResourceTick();
        }

        public void updateResourceTick(){
            //give the owner money
            if (timeForResource >= delay * 60)
            {
                timeForResource = 0;
                if (this.bgEntity.owner == PlayerManager.getInstance().currentPlayerId)
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

using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Managers;
using Incursio.Utils;

namespace Incursio.Entities.Components
{
    public class CapturableComponent : BaseComponent
    {
        public BaseGameEntity capturingEntity; 
        public int TIME_TO_CAPTURE = 15;
        int timeSpentCapping = 0;
        long entityStartingHealth;
        bool capping = false;

        public int pointValue{
            get{return this.bgEntity.pointValue;}
        }

        public CapturableComponent(BaseGameEntity entity) : base(entity){

        }

        public override void setAttributes(List<KeyValuePair<string, string>> attributes)
        {
            base.setAttributes(attributes);

            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i].Key)
                {
                    case "timeToCapture": this.TIME_TO_CAPTURE = int.Parse(attributes[i].Value); break;
                    default: break;
                }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (capping)
            {
                if (timeSpentCapping >= TIME_TO_CAPTURE * 60)
                {
                    this.finishCapture();
                }
                else
                {
                    //if the hero gets attacked, cancel capping
                    if (!capturingEntity.captureComponent.isCapturing || capturingEntity.health < entityStartingHealth)
                    {
                        capping = false;
                        timeSpentCapping = 0;
                    }
                    else
                    {
                        timeSpentCapping++;
                    }
                }
            }
        }

        public void startCap(BaseGameEntity capturingEntity)
        {
            this.capturingEntity = capturingEntity;
            entityStartingHealth = capturingEntity.health;
            timeSpentCapping = 0;
            capping = true;

            PlayerManager.getInstance().notifyPlayer(this.bgEntity.owner,
                new GameEvent(State.EventType.ENEMY_CAPTURING_POINT, this.bgEntity, "", "Enemy Capturing Point", this.bgEntity.location));

        }

        private void finishCapture()
        {
            this.bgEntity.owner = capturingEntity.getPlayer(); //change over ownership

            if(bgEntity.resourceComponent != null)
                this.bgEntity.resourceComponent.timeForResource = 0;

            timeSpentCapping = 0;

            capturingEntity.captureComponent.finishCapture(this);
            PlayerManager.getInstance().notifyPlayer(capturingEntity.owner,
                new GameEvent(State.EventType.ENEMY_CAPTURING_POINT, this.bgEntity, SoundManager.getInstance().AudioCollection.messages.townCapped, "Control Point Captured!", bgEntity.location));

            capturingEntity = null;
            capping = false;

        }

        public double getPercentageDone()
        {
            int timeTotal = TIME_TO_CAPTURE * 60;
            return (float)timeSpentCapping / timeTotal;
        }

        public bool isCapping()
        {
            return capping;
        }
    }
}

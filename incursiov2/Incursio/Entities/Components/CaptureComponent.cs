using System;
using System.Collections.Generic;
using System.Text;


namespace Incursio.Entities.Components
{
    public class CaptureComponent : BaseComponent
    {
        public bool isCapturing = false;
        public int captureDistance = 4;

        public CaptureComponent(BaseGameEntity e) : base(e){

        }

        public void finishCapture(CapturableComponent c)
        {
            this.bgEntity.experienceComponent.gainExperience(c.pointValue);
        }

        public override void setAttributes(List<KeyValuePair<string, string>> attributes)
        {
            base.setAttributes(attributes);

            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i].Key)
                {
                    case "captureDistance": captureDistance = int.Parse(attributes[i].Value); break;
                    default: break;
                }
            }
        }

    }
}

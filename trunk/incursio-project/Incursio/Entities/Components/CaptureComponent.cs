using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;

namespace Incursio.Entities.Components
{
    public class CaptureComponent : BaseComponent
    {
        public bool isCapturing = false;

        public CaptureComponent(BaseGameEntity e) : base(e){

        }

        public void finishCapture(CapturableComponent c)
        {
            this.bgEntity.experienceComponent.gainExperience(c.pointValue);
        }

    }
}

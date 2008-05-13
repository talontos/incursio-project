using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities.Components
{
    public class ExperienceComponent : BaseComponent
    {
        public int level = 1;
        public int experiencePoints = 0;
        public int pointsToNextLevel = 1000;

        public ExperienceComponent(){

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, ref BaseGameEntity entity)
        {
            base.Update(gameTime, ref entity);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;

namespace Incursio.Entities.Components
{
    public class ExperienceComponent : BaseComponent
    {
        public int level = 1;
        public int experiencePoints = 0;
        public int pointsToNextLevel = 1000;

        public ExperienceComponent(BaseGameEntity entity)
            : base(entity)
        {

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Incursio.Entities.Components
{
    public class BaseComponent
    {
        public virtual void Update(GameTime gameTime, ref BaseGameEntity entity){

        }

        /// <summary>
        /// Sets attributes of this component
        /// </summary>
        /// <param name="args">paired list of attributes and their values</param>
        public virtual void setAttributes(List<KeyValuePair<string, object>> attributes){

        }
    }
}

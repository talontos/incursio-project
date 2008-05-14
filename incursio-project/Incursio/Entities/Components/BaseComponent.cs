using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Incursio.Classes;

namespace Incursio.Entities.Components
{
    public class BaseComponent
    {
        protected BaseEntity entity;

        //TEMP
        protected BaseGameEntity bgEntity;

        public BaseComponent(ref BaseEntity e){
            this.entity = e;
        }

        public BaseComponent(BaseGameEntity e){
            this.bgEntity = e;
        }

        public virtual void Update(GameTime gameTime){

        }

        /// <summary>
        /// Sets attributes of this component
        /// </summary>
        /// <param name="args">paired list of attributes and their values</param>
        public virtual void setAttributes(List<KeyValuePair<string, object>> attributes){

        }
    }
}

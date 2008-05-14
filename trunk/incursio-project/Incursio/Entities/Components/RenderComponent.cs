using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Incursio.Classes;

namespace Incursio.Entities.Components
{
    public class RenderComponent : BaseComponent
    {
        public bool visible = false;
        public bool highlighted = false;
        public bool justDrawn = false;
        public int currentFrameX = 0;       //for animation
        public int currentFrameY = 0;       //for animation
        public int currentFrameXAttackDeath = 0;
        public int currentFrameYAttackDeath = 0;
        public int attackFramePause = 0;
        public Rectangle boundingBox;
        //TODO: for rendering, we also now need to know what textures to use!!

        public RenderComponent(BaseGameEntity entity) : base(entity){

        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            //TODO: RENDER entity
        }
    }
}

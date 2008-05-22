/****************************************
 * Copyright © 2008, Team RobotNinja:
 * 
 *     - Henry Armstrong
 *     - Andy Burras
 *     - Mitch Martin
 *     - Xuan Yu
 * 
 * All Rights Reserved
 ***************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Incursio.Managers;
using Incursio.Classes;
using Incursio.Utils;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Interface
{
    class BuildLightInfantryButton : Button
    {

        private Vector2 stringpos;

        public BuildLightInfantryButton() : 
            base(new Vector2(775, 605), 
                TextureBank.InterfaceTextures.lightInfantryIcon, 
                TextureBank.InterfaceTextures.lightInfantryIcon)
        {
            stringpos = new Vector2(position.X, position.Y + 23);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);

            batch.Draw(TextureBank.InterfaceTextures.moneyIcon,
                new Rectangle((int)stringpos.X, (int)stringpos.Y, TextureBank.InterfaceTextures.moneyIcon.Width, TextureBank.InterfaceTextures.moneyIcon.Height), Color.White);

            batch.DrawString(Incursio.getInstance().getFont_Arial(), "     " + EntityConfiguration.EntityPrices.COST_LIGHT_INFANTRY, this.stringpos, Color.Gold);
        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                EntityManager.getInstance().tryToBuild(State.EntityName.LightInfantry);
            }
        }
    }
}

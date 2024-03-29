/****************************************
 * Copyright � 2008, Team RobotNinja:
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
    class BuildHeavyInfantryButton : Button
    {
        private Vector2 stringpos;

        public BuildHeavyInfantryButton() : 
            base(new Vector2(925, 605), 
                TextureBank.InterfaceTextures.heavyInfantryIcon, 
                TextureBank.InterfaceTextures.heavyInfantryIcon)
        {
            stringpos = new Vector2(position.X + 5, position.Y + 23);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);

            batch.Draw(TextureBank.InterfaceTextures.moneyIcon,
                new Rectangle((int)stringpos.X, (int)stringpos.Y, TextureBank.InterfaceTextures.moneyIcon.Width, TextureBank.InterfaceTextures.moneyIcon.Height), Color.White);

            batch.DrawString(Incursio.getInstance().getFont_Arial(), "     " + EntityConfiguration.EntityPrices.COST_HEAVY_INFANTRY, this.stringpos, Color.Gold);
        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                EntityManager.getInstance().tryToBuild(State.EntityName.HeavyInfantry);
            }
        }
    }
}

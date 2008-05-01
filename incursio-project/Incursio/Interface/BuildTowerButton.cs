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
    class BuildTowerButton : Button
    {
        private Vector2 stringpos;

        public BuildTowerButton()
            : 
            base(new Vector2(775, 650), 
                TextureBank.InterfaceTextures.guardTowerIcon, 
                TextureBank.InterfaceTextures.guardTowerIcon)
        {
            stringpos = new Vector2(position.X, position.Y + 23);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
        {
            base.Draw(batch);

            batch.Draw(TextureBank.InterfaceTextures.moneyIcon,
                new Rectangle((int)stringpos.X, (int)stringpos.Y, TextureBank.InterfaceTextures.moneyIcon.Width, TextureBank.InterfaceTextures.moneyIcon.Height), Color.White);

            batch.DrawString(Incursio.getInstance().getFont_Arial(), "     " + EntityConfiguration.EntityPrices.COST_GUARD_TOWER, this.stringpos, Color.Gold);
        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                InputManager.getInstance().positioningTower = true;
                TextureBank.InterfaceTextures.cursorEvent = TextureBank.EntityTextures.guardTowerTexturePlayer;

            }
        }
    }
}

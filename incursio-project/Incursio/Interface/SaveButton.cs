using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;
using Microsoft.Xna.Framework;

namespace Incursio.Interface
{
    class SaveButton : Button
    {
        public SaveButton() : 
            base(new Vector2(0, 0), 
                TextureBank.InterfaceTextures.saveGameButton, 
                TextureBank.InterfaceTextures.saveGameButtonPressed)
        {

        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus()) //find out if the button is pressed
            {
                FileManager.getInstance().saveCurrentGame("Hero.wtf");
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;
using Microsoft.Xna.Framework;

namespace Incursio.Interface
{
    class LoadButton : Button
    {
        public LoadButton() : 
            base(new Vector2(0, 50), 
                TextureBank.InterfaceTextures.loadGameButton, 
                TextureBank.InterfaceTextures.loadGameButtonPressed)
        {

        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus()) //find out if the button is pressed
            {
                FileManager.getInstance().loadGame("Hero.wtf");
            }
        }
    }
}

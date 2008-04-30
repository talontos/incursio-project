using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;
using Microsoft.Xna.Framework;

namespace Incursio.Interface
{
    class SaveMenuButton : Button
    {
        public SaveMenuButton() : 
            base(new Vector2(0, 50), 
                TextureBank.InterfaceTextures.saveGameButton, 
                TextureBank.InterfaceTextures.saveGameButtonPressed)
        {

        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus()) //find out if the button is pressed
            {
                this.setFocus(false);
                Incursio.getInstance().currentState = State.GameState.SaveMenu;
            }
        }
    }
}

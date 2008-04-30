using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;
using Microsoft.Xna.Framework;

namespace Incursio.Interface
{
    class SaveButton : Button
    {
        private string filename;

        public SaveButton(string file) : 
            base(new Vector2(0, 0), 
                TextureBank.InterfaceTextures.saveGameButton, 
                TextureBank.InterfaceTextures.saveGameButtonPressed)
        {
            this.filename = file;
        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus()) //find out if the button is pressed
            {
                this.setFocus(false);
                FileManager.getInstance().saveCurrentGame(this.filename);
                Incursio.getInstance().currentState = State.GameState.PausedPlay;
            }
        }

    }
}

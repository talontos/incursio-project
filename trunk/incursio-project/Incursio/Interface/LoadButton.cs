using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;
using Microsoft.Xna.Framework;

namespace Incursio.Interface
{
    class LoadButton : Button
    {
        private string filename;

        public LoadButton(string file) : 
            base(new Vector2(0, 50), 
                TextureBank.InterfaceTextures.loadGameButton, 
                TextureBank.InterfaceTextures.loadGameButtonPressed)
        {
            this.filename = file;
        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus()) //find out if the button is pressed
            {
                this.setFocus(false);
                FileManager.getInstance().loadGame(filename);
                Incursio.getInstance().currentState = State.GameState.Menu;
            }
        }
    }
}

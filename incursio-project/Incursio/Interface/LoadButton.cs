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

        public LoadButton(string file, int num) : 
            base(new Vector2(0, 50), 
                TextureBank.InterfaceTextures.loadGameButton, 
                TextureBank.InterfaceTextures.loadGameButtonPressed)
        {
            this.filename = file;

            switch (num)
            {
                case 1:
                    this.pressed = TextureBank.InterfaceTextures.file1_pressed;
                    this.passive = TextureBank.InterfaceTextures.file1_not_pressed;
                    break;
                case 2:
                    this.pressed = TextureBank.InterfaceTextures.file2_pressed;
                    this.passive = TextureBank.InterfaceTextures.file2_not_pressed;
                    break;
                case 3:
                    this.pressed = TextureBank.InterfaceTextures.file3_pressed;
                    this.passive = TextureBank.InterfaceTextures.file3_not_pressed;
                    break;
            }
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

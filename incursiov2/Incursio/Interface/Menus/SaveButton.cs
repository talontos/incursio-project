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
using Incursio.Managers;
using Microsoft.Xna.Framework;

namespace Incursio.Interface
{
    class SaveButton : Button
    {
        private string filename;

        public SaveButton(string file, int num) : 
            base(new Vector2(0, 0), 
                TextureBank.getInstance().InterfaceTextures.interfaceTextures.saveGameButton.texture,
                TextureBank.getInstance().InterfaceTextures.interfaceTextures.saveGameButtonPressed.texture)
        {
            this.filename = file;

            switch (num)
            {
                case 1:
                    this.pressed = TextureBank.getInstance().InterfaceTextures.interfaceTextures.file1_pressed.texture;
                    this.passive = TextureBank.getInstance().InterfaceTextures.interfaceTextures.file1_not_pressed.texture;
                    break;
                case 2:
                    this.pressed = TextureBank.getInstance().InterfaceTextures.interfaceTextures.file2_pressed.texture;
                    this.passive = TextureBank.getInstance().InterfaceTextures.interfaceTextures.file2_not_pressed.texture;
                    break;
                case 3:
                    this.pressed = TextureBank.getInstance().InterfaceTextures.interfaceTextures.file3_pressed.texture;
                    this.passive = TextureBank.getInstance().InterfaceTextures.interfaceTextures.file3_not_pressed.texture;
                    break;
            }
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

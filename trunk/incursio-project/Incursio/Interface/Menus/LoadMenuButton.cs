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
    class LoadMenuButton : Button
    {
        public LoadMenuButton() : 
            base(new Vector2(0, 50), 
                TextureBank.getInstance().InterfaceTextures.interfaceTextures.loadGameButton.texture,
                TextureBank.getInstance().InterfaceTextures.interfaceTextures.loadGameButtonPressed.texture)
        {

        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus()) //find out if the button is pressed
            {
                this.setFocus(false);

                Incursio.getInstance().currentState = State.GameState.LoadMenu;
            }
        }
    }
}

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
using Microsoft.Xna.Framework;
using Incursio.Managers;

namespace Incursio.Interface
{
    public class MainMenuButton : Button
    {

        public MainMenuButton() : 
            base(new Vector2(465, 738),
                TextureBank.getInstance().InterfaceTextures.interfaceTextures.gameMenuButton.texture,
                TextureBank.getInstance().InterfaceTextures.interfaceTextures.gameMenuButtonPressed.texture
        ){
            
        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                Incursio.getInstance().currentState = State.GameState.Menu;
            }
        }

    }
}

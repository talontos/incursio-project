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
    class ExitGameButton : Button
    {
        public ExitGameButton() : 
            base(new Vector2(524, 638),
            TextureBank.getInstance().InterfaceTextures.interfaceTextures.exitGameButton.texture,
            TextureBank.getInstance().InterfaceTextures.interfaceTextures.exitGameButtonPressed.texture)
        {

        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())    //if exitGameButton is pressed, exit the game
            {
                Incursio.getInstance().exitGame();
            }
        }
    }
}

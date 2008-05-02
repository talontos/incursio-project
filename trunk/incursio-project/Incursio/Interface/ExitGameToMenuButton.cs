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
    class ExitGameToMenuButton : Button
    {
        public ExitGameToMenuButton() : 
            base(new Vector2(475, 384), 
                 TextureBank.InterfaceTextures.exitGameToMenuButton, 
                 TextureBank.InterfaceTextures.exitGameToMenuButtonPressed)
        {


        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                EntityManager.getInstance().reinitializeInstance();
                PlayerManager.getInstance().reinitializeInstance();
                Incursio.getInstance().currentState = State.GameState.Menu;
            }
        }
    }
}

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
    public class MapSelectButton : Button
    {

        public MapSelectButton() : 
            base(new Vector2(400, 638), 
                TextureBank.InterfaceTextures.newGameButton, 
                TextureBank.InterfaceTextures.newGameButtonPressed
        ){


        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                Incursio.getInstance().currentState = State.GameState.MapSelection;
            }
        }

    }
}

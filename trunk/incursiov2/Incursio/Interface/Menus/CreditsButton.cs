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
    class CreditsButton : Button
    {
        public CreditsButton() : 
            base(new Vector2(0, 0), 
                TextureBank.getInstance().InterfaceTextures.interfaceTextures.creditsButton_not_pressed.texture,
                TextureBank.getInstance().InterfaceTextures.interfaceTextures.creditsButton_pressed.texture
        ){

        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())    //if exitGameButton is pressed, exit the game
            {
                this.setFocus(false);
                
                /*EntityManager.getInstance().reinitializeInstance();
                PlayerManager.getInstance().reinitializeInstance();
                MapManager.getInstance().setCurrentLevel(State.CampaignLevel.CREDITS);
                MapManager.getInstance().initializeCurrentMap();*/
                Incursio.getInstance().currentState = State.GameState.Credits;
            }
        }
    }
}

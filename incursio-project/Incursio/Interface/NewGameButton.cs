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
    public class NewGameButton : Button
    {
        public State.CampaignLevel level;

        public NewGameButton(State.CampaignLevel level) : 
            base(new Vector2(400, 638), 
                TextureBank.InterfaceTextures.newGameButton, 
                TextureBank.InterfaceTextures.newGameButtonPressed
        ){

            this.level = level;
            switch(this.level){
                case State.CampaignLevel.ONE:
                    this.passive = TextureBank.InterfaceTextures.portMap_pressed;
                    this.pressed = TextureBank.InterfaceTextures.portMap_not_pressed;
                    break;
                case State.CampaignLevel.TWO:
                    this.passive = TextureBank.InterfaceTextures.inlandMap_pressed;
                    this.pressed = TextureBank.InterfaceTextures.inlandMap_not_pressed;
                    break;
                case State.CampaignLevel.THREE:
                    this.passive = TextureBank.InterfaceTextures.capitalMap_pressed;
                    this.pressed = TextureBank.InterfaceTextures.capitalMap_not_pressed;
                    break;
            }

        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                EntityManager.getInstance().reinitializeInstance();
                PlayerManager.getInstance().reinitializeInstance();
                MessageManager.getInstance().reinitializeInstance();
                MapManager.getInstance().setCurrentLevel(this.level);
                MapManager.getInstance().initializeCurrentMap();
                Incursio.getInstance().currentState = State.GameState.InPlay;
            }
        }
    }
}

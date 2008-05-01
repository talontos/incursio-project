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
                TextureBank.InterfaceTextures.creditsButton_not_pressed, 
                TextureBank.InterfaceTextures.creditsButton_pressed
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

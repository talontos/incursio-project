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

        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                EntityManager.getInstance().reinitializeInstance();
                PlayerManager.getInstance().reinitializeInstance();
                MapManager.getInstance().setCurrentLevel(this.level);
                MapManager.getInstance().initializeCurrentMap();
                Incursio.getInstance().currentState = State.GameState.InPlay;
            }
        }
    }
}

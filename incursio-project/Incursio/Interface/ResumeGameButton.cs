using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Incursio.Managers;

namespace Incursio.Interface
{
    class ResumeGameButton : Button
    {
        public ResumeGameButton() : 
            base(new Vector2(475, 349), 
                TextureBank.InterfaceTextures.resumeGameButton, 
                TextureBank.InterfaceTextures.resumeGameButtonPressed)
        {

        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                Incursio.getInstance().currentState = State.GameState.InPlay;
            }
        }
    }
}

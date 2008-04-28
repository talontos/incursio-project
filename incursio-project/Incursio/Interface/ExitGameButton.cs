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
            TextureBank.InterfaceTextures.exitGameButton,
            TextureBank.InterfaceTextures.exitGameButtonPressed)
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

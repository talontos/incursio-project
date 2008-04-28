using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;
using Microsoft.Xna.Framework;

namespace Incursio.Interface
{
    class SaveButton : Button
    {
        public SaveButton() : 
            base(new Vector2(0, 0), 
                TextureBank.InterfaceTextures.saveGameButton, 
                TextureBank.InterfaceTextures.saveGameButtonPressed
        ){

        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Managers;
using Microsoft.Xna.Framework;

namespace Incursio.Interface
{
    class LoadButton : Button
    {
        public LoadButton() : 
            base(new Vector2(0, 50), 
                TextureBank.InterfaceTextures.loadGameButton, 
                TextureBank.InterfaceTextures.loadGameButtonPressed
        ){

        }
    }
}

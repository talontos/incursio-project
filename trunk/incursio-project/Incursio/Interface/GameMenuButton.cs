using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Interface
{
    class GameMenuButton : Button
    {
        public GameMenuButton(Vector2 position, Texture2D passive, Texture2D pressed)
        {
            this.position = position;
            this.passive = passive;
            this.pressed = pressed;
        }

        public void Action()
        {
           
        }
    }
}

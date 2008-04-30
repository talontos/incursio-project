using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Incursio.Managers;

namespace Incursio.Interface
{
    public class InstructionButton : Button
    {

        public InstructionButton() : 
            base(new Vector2(465, 738),
                TextureBank.InterfaceTextures.instructionsButton_not_pressed,
                TextureBank.InterfaceTextures.instructionsButton_pressed
        ){
            
        }

        public override void Update(Cursor cursor)
        {
            base.Update(cursor);

            if (!this.getPressed() && this.getFocus())
            {
                this.setFocus(false);
                Incursio.getInstance().currentState = State.GameState.Instructions;
            }
        }

    }
}

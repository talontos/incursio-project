using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Incursio.Utils;
using Incursio.Commands;
using Microsoft.Xna.Framework;

namespace Incursio.Commands
{
    public class StopCommand : BaseCommand
    {
        public StopCommand(){
            this.type = State.Command.STOP;
        }

        public override void execute(GameTime gameTime, ref BaseGameEntity subject)
        {
            //set subject to idle (?) state.
            //Also remove all other commands from subject
            subject.issueOrderList();
        }
    }
}

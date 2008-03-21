using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Commands;
using Incursio.Classes;

namespace Incursio.Commands
{
    public class GuardCommand : BaseCommand
    {
        public GuardCommand(){
            this.type = State.Command.GUARD;
        }

        public override void execute(ref BaseGameEntity subject)
        {
            //look for enemy units within subject's sightRange
            base.execute(ref subject);
        }
    }
}

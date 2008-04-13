using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Microsoft.Xna.Framework;

namespace Incursio.Commands
{
    public class BaseCommand
    {
        public State.Command type = State.Command.NONE;
        public Boolean finishedExecution = false;
        
        public BaseCommand(){}

        public virtual void execute(GameTime gameTime, ref BaseGameEntity subject)
        {

        }
    }
}

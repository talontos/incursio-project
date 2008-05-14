/****************************************
 * Copyright © 2008, Team RobotNinja:
 * 
 *     - Henry Armstrong
 *     - Andy Burras
 *     - Mitch Martin
 *     - Xuan Yu
 * 
 * All Rights Reserved
 ***************************************/

using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Classes;
using Microsoft.Xna.Framework;
using Incursio.Classes.PathFinding;

namespace Incursio.Commands
{
    public class BaseCommand
    {
        public State.Command type = State.Command.NONE;
        public Boolean finishedExecution = false;
        
        public BaseCommand(){}

        public virtual void execute(GameTime gameTime, ref BaseGameEntity subject)
        {}

        public virtual void execute(GameTime gameTime, ref MovableObject subject)
        { }
    }
}

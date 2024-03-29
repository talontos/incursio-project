/****************************************
 * Copyright � 2008, Team RobotNinja:
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


using Incursio.Utils;
using Incursio.Commands;
using Microsoft.Xna.Framework;
using Incursio.Entities;

namespace Incursio.Commands
{
    public class AttackMoveCommand : BaseCommand
    {
        public Coordinate destination;

        public GuardCommand guard;
        public MoveCommand move;

        public AttackMoveCommand(Coordinate destination)
        {
            this.destination = destination;
            move = new MoveCommand(destination);
            guard = new GuardCommand();
        }

        public override void execute(GameTime gameTime, ref BaseGameEntity subject)
        {
            move.execute(gameTime, ref subject);
            guard.execute(gameTime, ref subject);

            this.finishedExecution = move.finishedExecution;
        }
    }
}

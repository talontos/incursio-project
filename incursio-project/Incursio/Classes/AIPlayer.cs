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

using Incursio.Managers;

namespace Incursio.Classes
{
    public class AIPlayer : Player
    {

        private BaseAI intelligence;

        /// <summary>
        /// Constructs an AI-Controlled player using the most basic AI
        /// </summary>
        public AIPlayer() : base(){
            intelligence = new BaseAI();
        }

        /// <summary>
        /// Constructs an AI-Controlled player using the given AI object
        /// </summary>
        /// <param name="ai">The *instantiated* AI to be used</param>
        public AIPlayer(BaseAI AI) : base(){
            intelligence = AI;
        }

        public override void dispatchEvent(global::Incursio.Utils.GameEvent e)
        {
            this.events.Add(e);
        }

        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //TODO: Give the computer a money-system
            //Make sure we always have money
            if (this.MONETARY_UNIT < 100)
                this.MONETARY_UNIT += 100;  //CHEATING!!!!

            //this update will perform AI actions for this player
            intelligence.Update(gameTime, this);
        }

    }
}

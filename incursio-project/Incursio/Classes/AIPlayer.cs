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
        public AIPlayer(BaseAI ai){
            intelligence = ai;
        }

        public override void update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            //base will update general player-stats
            base.update(gameTime);

            //this update will perform AI actions for this player
            intelligence.Update(gameTime, this);
        }

    }
}

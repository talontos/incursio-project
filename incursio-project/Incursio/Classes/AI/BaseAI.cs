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
using Incursio.Utils;
using Incursio.Managers;

namespace Incursio.Classes
{
    /// <summary>
    /// BaseAI is a very basic, but functional, reactive artificial intelligence architecture.
    /// 
    /// AIPlayers with this AI will not act on anything until acted upon by another entity.
    /// I.E. - will not actively construct units, will not explore, and will not assault. They will,
    /// however, defend themselves if attacked.
    /// </summary>
    public class BaseAI
    {
        public BaseAI(){

        }

        /// <summary>
        /// This is where all AI actions are performed.  Actions are based off of this player's
        /// event list; along with any other rules in sub-classes
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        public virtual void Update(Microsoft.Xna.Framework.GameTime gameTime, AIPlayer player){
            this.processEvents(ref player);
        }

        public virtual void processEvents(ref AIPlayer player){
            EntityManager manager = EntityManager.getInstance();
            player.events.ForEach(delegate(GameEvent e)
            {
                switch (e.type)
                {
                    case State.EventType.ENEMY_CAPTURING_POINT:
                        //we need to stop them
                        this.allUnitsAssault(e.location);
                        break;
                }

            });

            player.events = new List<GameEvent>();
        }

        public virtual void allUnitsAssault(Coordinate target){
            EntityManager.getInstance().issueCommand(State.Command.ATTACK_MOVE, false,
                EntityManager.getInstance().getLivePlayerUnits(PlayerManager.getInstance().computerPlayerId), target);
        }
    }
}

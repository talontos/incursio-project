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
            /*
            player.events.ForEach(delegate(GameEvent e)
            {
                if(e.type == State.EventType.UNDER_ATTACK){
                    //send troops to help?
                    EntityManager manager = EntityManager.getInstance();
                    manager.issueCommand(State.Command.MOVE, false, manager.getLivePlayerUnits(player.id), e.location);
                }
            });
            */
        }
    }
}

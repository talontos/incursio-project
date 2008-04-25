using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Incursio.Managers;

namespace Incursio.Classes
{
    /// <summary>
    /// DefensiveAI is a very basic, but functional, reactive artificial intelligence architecture.
    /// 
    /// AIPlayers with this AI will actively build an army and defenses, but will not actively hunt the player.
    /// This AI will also act as a BaseAI
    /// </summary>
    public class DefensiveAI : BaseAI
    {
        public DefensiveAI(){

        }

        /// <summary>
        /// This is where all AI actions are performed.  Actions are based off of this player's
        /// event list; along with any other rules in sub-classes
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="player"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime, AIPlayer player){
            //BUILD MY ARMY
            //TODO: Improve - this currently just constantly builds light infantry!
            /*EntityManager.getInstance().getLivePlayerStructures(player.id).ForEach(delegate(Structure s)
            {
                if(s.isConstructor){
                    if( !(s as CampStructure).isBuilding() ){
                        (s as CampStructure).build(new LightInfantryUnit());
                    }
                }
            });
            */
            //TODO: CHECK EVENTS

            base.Update(gameTime, player);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Incursio.Managers;

namespace Incursio.Classes
{
    /// <summary>
    /// SimpleAI is a very basic, but functional, reactive artificial intelligence architecture.
    /// 
    /// AIPlayers with this AI will actively build an army, but will not actively hunt the player.
    /// This AI will also act as a BaseAI
    /// </summary>
    public class SimpleAI : BaseAI
    {
        public SimpleAI(){

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
            List<Structure> myGuys = EntityManager.getInstance().getLivePlayerStructures(player.id);
            
            if(EntityManager.getInstance().getLivePlayerUnits(player.id).Count < 15){
                //build Army
                myGuys.ForEach(delegate(Structure s)
                {
                    if(s.isConstructor){
                        if( !(s as CampStructure).isBuilding() ){

                            int randU = Incursio.rand.Next(0, 100);

                            if(randU > 60)
                                (s as CampStructure).build(new LightInfantryUnit());

                            else if (randU > 30)
                                (s as CampStructure).build(new ArcherUnit());

                            else
                                (s as CampStructure).build(new HeavyInfantryUnit());

                            s.setDestination(new Coordinate(Incursio.rand.Next(20, 2000), Incursio.rand.Next(20, 2000)));
                        }
                    }
                });
            }

            //TODO: CHECK EVENTS

            base.Update(gameTime, player);
        }
    }
}

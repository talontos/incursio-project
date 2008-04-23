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

        private List<EntityBuildOrder> buildList = new List<EntityBuildOrder>();
        private int minPreferredArmySize = 20;

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

            //continue building army, if necessary
            this.queueBuildRandomUnit(ref player);

            //checks events
            this.processEvents(ref player);

            //processEvents will catch a creation_complete event; but if the camp is idle, we need to use it!
            if(!EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.COMPUTER)[0].isBuilding()){
                this.buildNextEntity();
            }
        }

        public override void processEvents(ref AIPlayer player)
        {
            EntityManager manager = EntityManager.getInstance();
            player.events.ForEach(delegate(GameEvent e)
            {
                switch (e.type)
                {
                    case State.EventType.ENEMY_CAPTURING_POINT:
                        //we need to stop them

                        //TODO: don't send everyone unless it's last one?
                        this.allUnitsAssault(e.location);
                        break;
                    case State.EventType.POINT_CAPTURED:
                        //we need to fortify this point
                        //we should build 1-2 towers near the point
                        //  in between the point and the enemy base
                        //  we also should station a few units here
                        //      *enqueue towers, enqueue OR re-assign units
                        break;
                    case State.EventType.UNDER_ATTACK:
                        //we should help, depending on what it is
                        break;
                    case State.EventType.CREATION_COMPLETE:
                        //we just finished building something;
                        //build the next thing on our list
                        this.buildNextEntity();
                        break;
                }
            });

            player.events = new List<GameEvent>();
        }

        private void buildNextEntity(){
            if (buildList.Count > 0)
            {
                //pop next order off queue and build
                EntityBuildOrder order = buildList[0];
                buildList.Remove(order);

                //right now we only have one constructor-class structure; so this is okay
                CampStructure camp = EntityManager.getInstance().getLivePlayerCamps(State.PlayerId.COMPUTER)[0];
                camp.build(order.entity);

                if (order.entity is GuardTowerStructure)
                    camp.setNewStructureCoords(order.location);
                else
                    camp.setDestination(order.location);
            }
        }

        private void queueBuildRandomUnit(ref AIPlayer player){
            List<Structure> myGuys = EntityManager.getInstance().getLivePlayerStructures(player.id);

            if (EntityManager.getInstance().getLivePlayerUnits(player.id).Count + this.buildList.Count <= this.minPreferredArmySize)
            {
                //'order' random unit
                int randU = Incursio.rand.Next(0, 100);
                Coordinate dest = null;// new Coordinate(Incursio.rand.Next(100, MapManager.getInstance().currentMap.width - 100),
                                       //          Incursio.rand.Next(100, MapManager.getInstance().currentMap.height - 100));

                if (randU > 60)
                    this.buildList.Add(new EntityBuildOrder(dest, new LightInfantryUnit()));

                else if (randU > 30)
                    this.buildList.Add(new EntityBuildOrder(dest, new ArcherUnit()));

                else
                    this.buildList.Add(new EntityBuildOrder(dest, new HeavyInfantryUnit()));
            }
        }
    }
}

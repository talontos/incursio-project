using System;
using System.Collections.Generic;
using System.Text;

using Incursio.Utils;
using Incursio.Classes;
using Microsoft.Xna.Framework;
using Incursio.Managers;

namespace Incursio.Commands
{
    public class BuildCommand : BaseCommand
    {
        //TODO: IMPLEMENT BUILDING USING STRING
        public String buildClass;
        public EntityBuildOrder buildOrder;
        //public BaseGameEntity toBeBuilt;

        public BuildCommand(BaseGameEntity toBeBuilt, Coordinate location){
            this.buildOrder = new EntityBuildOrder(location, toBeBuilt);
        }

        //NOTE: for building, subject will be a structure; probably a camp
        public override void execute(GameTime gameTime, ref BaseGameEntity subject)
        {
            //TODO: EXPAND ON THIS
            //Right now, camps are only building entities
            if (subject is CampStructure)
            {
                if (!(subject as Structure).isBuilding())
                {
                    //If the location of the order is not null, 
                    //we need to check if it's okay to build there
                    if(buildOrder.location != null)
                    {
                        
                        if( !(MapManager.getInstance().currentMap.getCellOccupancy_pixels(
                            buildOrder.location.x, buildOrder.location.y) == (byte)1) )
                        {
                            //we can't build something here
                            
                            //Notify owner
                            PlayerManager.getInstance().notifyPlayer(subject.owner,
                                new GameEvent(
                                    State.EventType.CANT_MOVE_THERE, subject, 
                                    "Cannot Build There", 
                                    buildOrder.location
                                )
                            );

                            this.finishedExecution = true;
                            return;
                        }
                    }
                
                    (subject as CampStructure).build(buildOrder);
                    this.finishedExecution = true;
                    
                }
                else
                {
                    //wait for building to stop
                    this.finishedExecution = false;
                }

            }
            else
                this.finishedExecution = true;
        }
    }
}

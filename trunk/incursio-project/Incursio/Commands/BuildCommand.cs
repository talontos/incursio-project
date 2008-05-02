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
using Incursio.Classes;
using Microsoft.Xna.Framework;
using Incursio.Managers;

namespace Incursio.Commands
{
    public class BuildCommand : BaseCommand
    {
        public String buildClass;
        public EntityBuildOrder buildOrder;

        public BuildCommand(BaseGameEntity toBeBuilt, Coordinate location){
            this.buildOrder = new EntityBuildOrder(location, toBeBuilt);
        }

        public override void execute(GameTime gameTime, ref BaseGameEntity subject)
        {
            //TODO: EXPAND ON THIS
            //Right now, camps are only building entities
            if (subject is CampStructure)
            {

                if(!this.checkBuildAndNotify(ref subject)){
                    this.finishedExecution = true;
                    return;
                }

                if (!(subject as Structure).isBuilding())
                {
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

        public bool checkBuildAndNotify(ref BaseGameEntity subject){
            if (!this.checkBuildOk(ref subject))
            {
                //we can't build something here

                //Notify owner
                if(buildOrder.entity is Structure){
                    PlayerManager.getInstance().notifyPlayer(subject.owner,
                        new GameEvent(
                            State.EventType.CANT_MOVE_THERE, subject,
                            SoundCollection.MessageSounds.cantBuild,
                            "Cannot Build There",
                            buildOrder.location
                        )
                    );
                }

                this.finishedExecution = true;
                return false;
            }

            return true;
        }

        private bool checkBuildOk(ref BaseGameEntity subject){
            //If the location of the order is not null, 
            //we need to check if it's okay to build there
            if (buildOrder.location != null)
            {

                if (!(MapManager.getInstance().currentMap.getCellOccupancy_pixels(
                    buildOrder.location.x, buildOrder.location.y) == (byte)1))
                {
                    return false;
                }
            }

            //now we need to make sure that, if we're building a tower, 
            //  that it's close to a key point
            if(buildOrder.entity is GuardTowerStructure){

                if(buildOrder.keyPoint == null){
                    //we need to assign it a key point
                    List<KeyPoint> kps = EntityManager.getInstance().getPlayerKeyPoints(subject.owner);

                    for(int i = 0; i < kps.Count; i++){
                        if(MapManager.getInstance().currentMap.getCellDistance(
                            buildOrder.location,
                            kps[i].structure.location) <= kps[i].structure.sightRange)
                        {
                            //in range, okay
                            buildOrder.keyPoint = kps[i];
                            return true;
                        }
                    }

                    return false;
                }

                //make sure it's within its key point's sightrange
                if(! (MapManager.getInstance().currentMap.getCellDistance(
                    buildOrder.location,
                    buildOrder.keyPoint.structure.location) <= buildOrder.keyPoint.structure.sightRange))
                {
                    //too far, no way
                    return false;
                }
            }

            return true;
        }
    }
}

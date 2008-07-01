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
using Incursio.Commands;

using Microsoft.Xna.Framework;
using Incursio.Managers;
using Incursio.Utils.PathFinding;
using Incursio.Entities.Components;
using Incursio.Entities;

namespace Incursio.Commands
{
    public class MoveCommand : BaseCommand
    {
        public Coordinate destination;
        public Coordinate start;

        public Coordinate location;

        public MoveCommand(Coordinate destination){
            this.destination = destination;

            this.start = null;
        }

        public override void execute(GameTime gameTime, ref BaseGameEntity subject)
        {
            if(subject.isConstructor){
                //won't move, just set destination
                subject.setDestination(destination);
                this.finishedExecution = true;
                return;
            }

            else if(!subject.canMove){
                //not a unit, do nothing?
                this.finishedExecution = true;
                return;
            }

            //set-up to move
            //first run; find path
            else if (start == null){

                //First, check passability:
                if (MapManager.getInstance().currentMap.getCellOccupancy_pixels(destination.x, destination.y) == (byte)0)
                {
                    //cell is occupied; move somewhere else
                    this.destination =
                        MapManager.getInstance().currentMap.getClosestPassableLocation(subject.location, destination);

                    if(destination == null){
                        finishedExecution = true;
                        return;
                    }
                }

                subject.movementComponent.updateMovableLocation();

                MovableObject unit = subject.movementComponent.movable;
                start = subject.getLocation();

                Vector2 Click = destination.toVector2();

                int X = (int)(destination.x ) / MapManager.TILE_WIDTH;
                int Y = (int)(destination.y ) / MapManager.TILE_HEIGHT;

                Point Start = new Point((int)(unit.PositionCurrent.X / MapManager.TILE_WIDTH), (int)(unit.PositionCurrent.Y / MapManager.TILE_HEIGHT));
                Point End = new Point((int)(Click.X / MapManager.TILE_WIDTH), (int)(Click.Y / MapManager.TILE_HEIGHT));

                if (Start == End)
                {
                    //will this ever happen????
                    unit.LinearMove(unit.PositionCurrent, Click);
                }
                else
                {
                    List<PathReturnNode> foundPath = MapManager.getInstance().pathFinder.FindPath(Start, End, -1, subject.keyId);

                    if (foundPath != null)
                        unit.PathMove(ref foundPath, unit.PositionCurrent, Click);
                    else{
                        //NO PATH FOUND
                        start = null;
                    }
                }
            }

            //move towards destination
            subject.setDestination(this.destination);
            subject.currentState =State.EntityState.Moving;

            this.location = subject.getLocation();

            this.finishedExecution = subject.updateMovement((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

    }
}

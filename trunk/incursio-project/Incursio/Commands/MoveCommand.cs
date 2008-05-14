/****************************************
 * Copyright � 2008, Team RobotNinja:
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

using Incursio.Classes;
using Incursio.Utils;
using Incursio.Commands;

using Microsoft.Xna.Framework;
using Incursio.Managers;
using Incursio.Classes.PathFinding;
using Incursio.Utils.PathFinding;
using Incursio.Entities.Components;

namespace Incursio.Commands
{
    public class MoveCommand : BaseCommand
    {
        public Coordinate destination;
        public Coordinate start;

        private Coordinate prevLocation;
        private int stuckCounter = 0;

        public MoveCommand(Coordinate destination){
            this.destination = destination;

            /*if(MapManager.getInstance().currentMap.getCellOccupancy_pixels(destination.x, destination.y) == (byte)0){
                //cell is occupied; move somewhere else
                this.destination =
                    MapManager.getInstance().currentMap.getPassableLocation(destination);
            }*/

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

                MovableObject unit = subject as MovableObject;
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
                    List<PathReturnNode> foundPath = MapManager.getInstance().pathFinder.FindPath(Start, End, -1);

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
            (subject as Unit).setCurrentState(State.UnitState.Moving);

            this.prevLocation = subject.getLocation();

            this.finishedExecution = subject.updateMovement((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        public override void execute(GameTime gameTime, ref MovableObject subject)
        {
            //first run; find path
            if (start == null)
            {

                //First, check passability:
                if (MapManager.getInstance().currentMap.getCellOccupancy_pixels(destination.x, destination.y) == (byte)0)
                {
                    //cell is occupied; move somewhere else
                    this.destination =
                        MapManager.getInstance().currentMap.getClosestPassableLocation(subject.location, destination);

                    if (destination == null)
                    {
                        finishedExecution = true;
                        return;
                    }
                }

                MovableObject unit = subject as MovableObject;
                start = subject.getLocation();

                Vector2 Click = destination.toVector2();

                int X = (int)(destination.x) / MapManager.TILE_WIDTH;
                int Y = (int)(destination.y) / MapManager.TILE_HEIGHT;

                Point Start = new Point((int)(unit.PositionCurrent.X / MapManager.TILE_WIDTH), (int)(unit.PositionCurrent.Y / MapManager.TILE_HEIGHT));
                Point End = new Point((int)(Click.X / MapManager.TILE_WIDTH), (int)(Click.Y / MapManager.TILE_HEIGHT));

                if (Start == End)
                {
                    //will this ever happen????
                    unit.LinearMove(unit.PositionCurrent, Click);
                }
                else
                {
                    List<PathReturnNode> foundPath = MapManager.getInstance().pathFinder.FindPath(Start, End, -1);

                    if (foundPath != null)
                        unit.PathMove(ref foundPath, unit.PositionCurrent, Click);
                    else
                    {
                        //NO PATH FOUND
                        start = null;
                    }
                }
            }

            //move towards destination

            //subject.setDestination(this.destination);
            //(subject as Unit).setCurrentState(State.UnitState.Moving);

            this.prevLocation = subject.getLocation();

            this.finishedExecution = subject.updateMovement((float)gameTime.ElapsedGameTime.TotalSeconds);
        }
    
    }
}

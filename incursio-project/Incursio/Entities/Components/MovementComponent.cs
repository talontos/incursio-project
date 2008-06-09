using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes.PathFinding;
using Incursio.Classes;
using Microsoft.Xna.Framework;
using Incursio.Commands;
using Incursio.Utils;
using Incursio.Managers;

namespace Incursio.Entities.Components
{
    public class MovementComponent : BaseComponent
    {
        public float moveSpeed = 320.0f;
        public Coordinate destination;

        public MovableObject movable;
        public State.Direction directionState = State.Direction.South;

        //TEMP
        public MovementComponent(BaseGameEntity e):base(e){
            movable = new MovableObject(ref e, this.moveSpeed);
            e.canMove = true;
        }

        public override void setAttributes(List<KeyValuePair<string, string>> attributes)
        {
            base.setAttributes(attributes);

            for(int i = 0; i < attributes.Count; i++){
                switch (attributes[i].Key){
                    case "moveSpeed": moveSpeed = float.Parse(attributes[i].Value); break;
                    default: break;
                }
            }
        }

        public void updateMovableLocation(){
            this.movable.PositionCurrent = this.bgEntity.location.toVector2();
        }

        //this should just update the movement of the entity...
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public virtual bool updateMovement(float ElapsedTime)
        {
            //move
            bool retVal = this.movable.updateMoveableObjectMovement(ElapsedTime);

            this.bgEntity.location = new Coordinate( (int)movable.PositionCurrent.X, (int)movable.PositionCurrent.Y);

            if (retVal) //movement finished
            {
                this.bgEntity.setIdle();
            }
            else
            {
                
                float xMinimumThreshold = 0.10F;
                float yMinimumThreshold = 0.10F;
                
                //get the direction to the target
                Vector2 direction = new Vector2(this.destination.x - movable.location.x, destination.y - movable.location.y);
                
                float xDirection = Vector2.Normalize(direction).X;
                float yDirection = Vector2.Normalize(direction).Y;

                if (xDirection > xMinimumThreshold && (xDirection / yDirection) >= 1)
                {
                    this.directionState = State.Direction.East;
                }
                else if (xDirection < -xMinimumThreshold && (xDirection / yDirection) <= -1)
                {
                    this.directionState = State.Direction.West;
                }

                if (yDirection > yMinimumThreshold && ((Math.Abs(xDirection)) / yDirection) < 1)
                {
                    this.directionState = State.Direction.South;
                }
                else if (yDirection < -yMinimumThreshold && (xDirection / yDirection) < 1)
                {
                    this.directionState = State.Direction.North;
                }
                
            }

            return retVal;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes.PathFinding;
using Incursio.Classes;
using Microsoft.Xna.Framework;
using Incursio.Commands;

namespace Incursio.Entities.Components
{
    public class MovementComponent : BaseComponent
    {
        public float moveSpeed = 320.0f;

        private MovableObject movable;

        private MoveCommand _currentCommand;

        public MoveCommand currentCommand{
            get { 
                return _currentCommand; 
            }

            set { 
                if(this.movable != null){
                    this.movable.PositionCurrent = this.bgEntity.location.toVector2();
                }

                _currentCommand = value;
            }
        }

        //TEMP
        public MovementComponent(BaseGameEntity e):base(e){
            movable = new MovableObject(ref e, this.moveSpeed);
        }

        public MovementComponent(ref BaseEntity e) : base(ref e){
            //we somehow need a movableobject
        }

        public override void setAttributes(List<KeyValuePair<string, object>> attributes)
        {
            base.setAttributes(attributes);

            for(int i = 0; i < attributes.Count; i++){
                switch (attributes[i].Key){
                    case "moveSpeed": moveSpeed = (float)attributes[i].Value; break;
                    default: break;
                }
            }
        }

        //this should just update the movement of the entity...
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentCommand != null){
                if(bgEntity.isConstructor){
                    //won't move, just set destination
                    bgEntity.setDestination(currentCommand.destination);
                    currentCommand.finishedExecution = true;
                }

                currentCommand.execute(gameTime, ref this.movable);

                this.bgEntity.location = this.currentCommand.location;

                if (currentCommand.finishedExecution)
                    this.currentCommand = null;
            }
        }
    }
}

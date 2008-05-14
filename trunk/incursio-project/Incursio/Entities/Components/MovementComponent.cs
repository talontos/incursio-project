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

        public MoveCommand currentCommand;

        //TEMP
        public MovementComponent(BaseGameEntity e):base(e){
            movable = new MovableObject(ref e);
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

            this.movable = new MovableObject(this.moveSpeed);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (currentCommand != null){
                currentCommand.execute(gameTime, ref this.movable);
                

                if (currentCommand.finishedExecution)
                    this.currentCommand = null;
            }
        }
    }
}

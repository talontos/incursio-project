using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Incursio.Classes
{
    public class Structure : BaseGameEntity
    {
        //for building things
        protected int timeBuilt = 0;
        protected int timeRequired = 0;
        protected BaseGameEntity buildProject;

        protected State.StructureState currentState;
        public Structure()
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (Incursio.getInstance().currentState == State.GameState.InPlay)
            {
                switch (this.currentState)
                {
                    /////////////////////
                    case State.StructureState.Idle:
                        //we're chillin
                        break;

                    /////////////////////
                    case State.StructureState.BeingBuilt:
                        beingBuiltTick();
                        break;

                    /////////////////////
                    case State.StructureState.Building:
                        buildTick();
                        break;

                    /////////////////////
                    case State.StructureState.Destroyed:

                        break;
                    /////////////////////

                    default: break;
                }
            }
        }

        public State.StructureState getCurrentState()
        {
            return currentState;
        }

        public void setCurrentState(State.StructureState state)
        {
            this.currentState = state;
        }

        public virtual void build(BaseGameEntity toBeBuilt)
        {
            this.buildProject = toBeBuilt;
        }

        public virtual void buildTick()
        {

        }

        public virtual void beingBuiltTick()
        {

        }
    }
}

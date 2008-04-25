using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Utils
{
    public class GameResult
    {
        public string result;
        public State.GameState resultState;

        //todo: statistical stuff?
        public GameResult(){
            resultState = State.GameState.None;
            result = "";
        }

        public GameResult(State.GameState state, string result){
            this.resultState = state;
            this.result = result;
        }
    }
}
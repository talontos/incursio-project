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

namespace Incursio.Utils
{
    public class GameResult
    {
        public string result;
        public State.GameState resultState;
        public string sound;

        //todo: statistical stuff?
        public GameResult(){
            resultState = State.GameState.None;
            result = "";
        }

        public GameResult(State.GameState state, string result, string sound){
            this.resultState = state;
            this.result = result;
            this.sound = sound;
        }
    }
}

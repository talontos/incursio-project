using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio_Sandbox
{
    class State
    {
        public enum GameState{
            None,   //Defaut value; no state set

            Initializing,   //Application *just* started  - initializing stuff

            Menu,   //Game menu ???

            InPlay, //Game has started

            PausedPlay, //Game ins paused

            Victory,    //Victory screen

            Defeat,     //Defeat screen

            Credits,    //Credits are 'rolling'
        }

        public enum UnitState{
            Idle,   //Default value; just standing aroud

            Moving, //Moving from point A to B

            Guarding,   //Stays in place unless enemies come into visible range

            Attacking,  //Attacking...duh.      \
                            //                   }-- Do we need two states?  Could just be 'InCombat'
            UnderAttack,    //Being attacked    /

        }

        public enum HealthStates{
            Healthy,    //Full, or almost full, health       (green bar?)

            Wounded,    //Medium health; wounded character.  (yellow bar?)

            Dying,      //Low health                         (red bar?)

            Dead,      //Uhh...he's dead.  Game over, man
        }

        public enum HeroLevels{
            Commander,      //Default, starting level (level 0);
            //...                       levels 1-X?
        }

        public enum CameraStateS{
                //*Zooms?

            Still,      //Default, camera is not moving

            Move_North,

            Move_Northeast,

            Move_East,

            Move_Southeast,

            Move_South,

            Move_Southwest,

            Move_West,

            Move_Northwest,
        }
    }
}

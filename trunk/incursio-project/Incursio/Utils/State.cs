using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio
{
    public static class State
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

        public enum UnitState
        {
            Idle,   //Default value; just standing aroud

            Moving, //Moving from point A to B

            Wandering,  //moving around to random places

            Guarding,   //Stays in place unless enemies come into visible range

            Attacking,  //Attacking...duh.      \
                            //                   }-- Do we need two states?  Could just be 'InCombat'
            UnderAttack,    //Being attacked    /

            Dead,

            Buried,

        }

        public enum StructureState  //states of a building
        {
            Idle,
            
            BeingBuilt,

            Building,

            Destroyed,

            Attacking,  //for guard towers

            Guarding,
        }

        public enum HealthStates
        {
            Healthy,    //Full, or almost full, health       (green bar?)

            Wounded,    //Medium health; wounded character.  (yellow bar?)

            Dying,      //Low health                         (red bar?)

            Dead,      //Uhh...he's dead.  Game over, man
        }

        public enum HeroLevels
        {
            Commander,      //Default, starting level (level 0);
            //...                       levels 1-X?
        }

        public enum Direction
        {
                //*Zooms?

            Still,      //Default, camera is not moving

            North,

            Northeast,

            East,

            Southeast,

            South,

            Southwest,

            West,

            Northwest,
        }

        public enum EntityName
        {
            LightInfantry,

            HeavyInfantry,

            Archer,

            Hero,

            GuardTower,

            Camp,

            ControlPoint,

            //TODO: add more entity types

        }

        public enum PlayerId{
            HUMAN,
            COMPUTER,
        }

        public enum Command{
            NONE,
            MOVE,
            ATTACK,
            ATTACK_MOVE,
            STOP,
            FOLLOW,
            GUARD,
            BUILD,
        }

        public enum CampaignLevel{
            ONE,
            TWO,
            THREE,
        }

        public enum EventType{
            UNDER_ATTACK,
            CREATION_COMPLETE,
        }
    }
}

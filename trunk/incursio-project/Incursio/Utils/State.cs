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

namespace Incursio
{
    public static class State
    {
        public enum GameState{
            None,   //Defaut value; no state set
            Initializing,   //Application *just* started  - initializing stuff
            Menu,   //Game menu ???
            MapSelection,
            LoadMenu,
            SaveMenu,
            Instructions,
            InPlay, //Game has started
            PausedPlay, //Game ins paused
            Victory,    //Victory screen
            Defeat,     //Defeat screen
            Credits,    //Credits are 'rolling'
        }

        public enum EntityState
        {
            Idle,   //Default value; just standing aroud
            Moving, //Moving from point A to B
            Wandering,  //moving around to random places
            Guarding,   //Stays in place unless enemies come into visible range
            Attacking,  //Attacking...duh.      \
                            //                   }-- Do we need two states?  Could just be 'InCombat'
            UnderAttack,    //Being attacked    /
            Capturing,
            Dead,
            Buried,
            BeingBuilt,
            Building,
            Destroyed,
            Projectile,
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
            NONE,
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
            ONE,
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX,
            SEVEN,
            EIGHT,
        }

        public enum Command{
            NONE,
            MOVE,
            ATTACK,
            ATTACK_MOVE,
            STOP,
            FOLLOW,
            CAPTURE,
            GUARD,
            COMMANDER_GUARD,
            BUILD,
        }

        public enum CampaignLevel{
            CREDITS,
            ONE,
            TWO,
            THREE,
            DEBUG,
        }

        public enum EventType{
            UNDER_ATTACK,
            CREATION_COMPLETE,
            CANT_MOVE_THERE,
            NOT_ENOUGH_RESOURCES,
            TAKING_DAMAGE,
            HEALING,
            ENEMY_CAPTURING_POINT,
            POINT_CAPTURED,
            LEVEL_UP,
            GAIN_RESOURCE,
            GAME_OVER_MAN,
            CHAT_MESSAGE,
        }

        public enum ThreatLevel{
            None,   //can't even attack
            High,  //probably kill me
            Medium,
            Low,
        }

        public enum WaterType
        {
            OpenWater,
            ShoreRight,
            ShoreLeft,
            ShoreUp,
            ShoreDown,
            ShoreLowerLeft,
            ShoreLowerRight,
            ShoreUpperLeft,
            ShoreUpperRight,
            ShoreOpenLowerLeft,
            ShoreOpenLowerRight,
            ShoreOpenUpperLeft,
            ShoreOpenUpperRight,
        }

        public enum RoadType
        {
            Horizontal,
            Vertical,
            ElbowUpRight,
            ElbowUpLeft,
            ElbowDownLeft,
            ElbowDownRight,
        }
    }
}

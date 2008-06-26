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
using Incursio.Entities.AudioCollections;

namespace Incursio.Managers
{
    /// <summary>
    /// This is a class containing static sets of arrays for playing sounds
    /// //each sound is the filename of the sound to play
    /// </summary>
    public class SoundCollection
    {
        private static SoundCollection instance;

        public List<AudioCollection> audioCollections;

        private SoundCollection(){
            this.audioCollections = new List<AudioCollection>();
        }

        public AudioCollection getCollectionByName(string name){
            for(int i = 0; i < this.audioCollections.Count; i++){
                if (audioCollections[i].name.Equals(name))
                    return audioCollections[i];
            }

            return null;
        }

        public static SoundCollection getInstance()
        {
            if(instance == null)
                instance = new SoundCollection();

            return instance;
        }

        public static string selectRandomSound(List<string> list)
        {
            return list[Incursio.rand.Next(0, list.Count)];
        }

        /*
        public static class AttackSounds{
            public static string SwordAttack = "steelsword.wav";
            public static string ArrowAttack = "bow release.wav";
            public static string Explosion = "explosion.wav";
        }
        
        public static class VoiceSounds{

            public static class HeroVoice
            {
                public static string[] enterBattlefield = 
                    {
                        ""
                    };

                public static string[] levelUp = 
                    {
                        ""
                    };

                public static string[] selection = 
                    {"heroAlert.wav",
                     "HeroConf2.wav",
                     "heroConfirm.wav",
                        "HeroAlert2.wav"
                    };

                public static string[] issueMoveOrder =
                    {"HeroAtt.wav",
                     "HeroAttack2.wav",
                     "heroConfirm.wav"
                    };

                public static string[] issueAttackOrder =
                    {"heroAtt.wav",
                     "heroAttack2.wav",
                        "HeroAlert2.wav"
                    };

                public static string[] death =
                    {"HeroDead.wav",
                        "HeroDead2.wav"
                    };
            }

            public static class LightInfantryVoice
            {
                public static string[] enterBattlefield = 
                    {
                     "arr.wav", 
                     "yaharr.wav",
                     "yaho.wav"
                    };

                public static string[] selection = 
                    {
                        "arr.wav",
                        "aye_captain.wav",
                        "aye_commander.wav"
                    };

                public static string[] issueMoveOrder =
                    {
                     "that_where_my_rum_is.wav",
                     "arr.wav",
                     "aye_captain.wav",
                     "aye_commander.wav",
                     "just_what_I_wanted.wav"
                    };

                public static string[] issueAttackOrder =
                    {
                        "aye_pointy.wav",
                        "arr.wav",
                     "aye_captain.wav",
                     "aye_commander.wav"
                    };

                public static string[] death =
                    {"archerdead.wav",
                     "HeroDead.wav",
                     "HeroDead.wav"
                    };
            }

            public static class HeavyInfantryVoice
            {
                public static string[] enterBattlefield = 
                    {"HeavyInfSelect.wav",
                        "HeavyConf2.wav"
                    };

                public static string[] selection = 
                    {"HeavyInfSelect.wav",
                        "HeavyConf2.wav",
                        "HaveAlert2.wav"
                    };

                public static string[] issueMoveOrder =
                    {"HeavyInfconf.wav",
                     "HeavyInfSelect.wav"
                    };

                public static string[] issueAttackOrder =
                    {"HeavyInfattack.wav",
                        "HeavyAttack2.wav",
                        "HeavyAlert2.wav"
                    };

                public static string[] death =
                    {"HeavyInfdead.wav",
                        "HeavyDead2.wave",
                     "HeroDead.wav",
                     "HeroDead.wav"
                    };
            }

            public static class ArcherVoice
            {
                public static string[] enterBattlefield = 
                    {"archerAlert.wav",
                     "archerAttack.wav",
                     "archerConfirm.wav"
                    };

                public static string[] selection = 
                    {"archerAlert.wav",
                     "archerConfirm.wav"
                    };

                public static string[] issueMoveOrder =
                    {"archerConfirm2.wav",
                        "archerAlert2.wav"
                    };

                public static string[] issueAttackOrder =
                    {"archerAttack.wav",
                     "archerAttack2.wav",
                        "archerAlert2.wav"
                    };

                public static string[] death =
                    {"archerDead.wav",
                     "HeroDead.wav",
                     "HeroDead.wav"
                    };
            }
        }

        public static class AmbientSounds{
            public static string main_menu = "Title.mp3";
            public static string credits = "Thunderhorse.mp3";
            public static string inPlay = "Film.mp3";
            public static string victory = "Thunderhorse.mp3";
            public static string defeat = "SadSong.mp3";
        }

        public static class MessageSounds
        {
            public static String campDestroyed = "CampDestroyed.wav";
            public static String baseAttack = "BaseUnderAtt.wav";
            public static String cantBuild = "CannotBuildThere.wav";
            public static String towerBuilt = "ConComplete.wav";
            public static String heroFallen = "HeroFallen.wav";
            public static String heroBattle = "HeroinBat.wav";
            public static String heroLowHP = "HeroisAbouttoDie.wav";
            public static String lvlUp = "HeroLevelUp.wav";
            public static String heroAtt = "HeroUnderAtt.wav";
            public static String townCapped = "TownCap.wav";
            public static String unitAtt = "Uattk.wav";
            //public static String unitReady = "UnitReady.wav";
        }
        */
    }
}

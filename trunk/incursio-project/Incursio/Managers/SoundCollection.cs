using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Managers
{
    public class SoundCollection
    {

        public static class AttackSounds{
            public static string SwordAttack = "steelsword.wav";
            public static string ArrowAttack = "bow release.wav";
        }

        public static string selectRandomSound(string[] list)
        {
            return list[Incursio.rand.Next(0, list.Length)];
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
                     "HerConf2.wav",
                     "heroConfirm.wav"
                    };

                public static string[] issueMoveOrder =
                    {"HeroAtt.wav",
                     "HeroAttack2.wav",
                     "heroConfirm.wav"
                    };

                public static string[] issueAttackOrder =
                    {"heroAtt.wav",
                     "heroAttack2.wav"
                    };

                public static string[] death =
                    {"HeroDead.wav"
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
                    {"archerdead.wav"
                    };
            }

            public static class HeavyInfantryVoice
            {
                public static string[] enterBattlefield = 
                    {"HeavyInfSelect.wav"
                    };

                public static string[] selection = 
                    {"HeavyInfSelect.wav"
                    };

                public static string[] issueMoveOrder =
                    {"HeavyInfconf.wav"
                    };

                public static string[] issueAttackOrder =
                    {"HeavyInfattack.wav"
                    };

                public static string[] death =
                    {"HeavyInfdead.wav"
                    };
            }

            public static class ArcherVoice
            {
                public static string[] enterBattlefield = 
                    {"archerAlert.wav"
                    };

                public static string[] selection = 
                    {"archerAlert.wav",
                     "archerConfirm.wav"
                    };

                public static string[] issueMoveOrder =
                    {"archerConfirm2.wav"
                    };

                public static string[] issueAttackOrder =
                    {"archerAttack.wav",
                     "archerAttack2.wav"
                    };

                public static string[] death =
                    {"archerDead.wav"
                    };
            }
        }

        public static class AmbientSounds{
            public static string main_menu = "Title.mp3";
            public static string credits = "Thunderhorse.mp3";
            public static string inPlay = "Film.mp3";
            public static string victory = "Thunder.mp3";
            public static string defeat = "SadSong.mp3";
        }

    }
}

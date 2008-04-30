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

        public static class VoiceSounds{

            public static class HeroVoice
            {
                public static string[] enterBattlefield = 
                    {"hero_enter_1.wav", 
                     "hero_enter_2.wav",
                     "hero_enter_3.wav"
                    };

                public static string[] levelUp = 
                    {"hero_lvl_1.wav", 
                     "hero_lvl_2.wav",
                     "hero_lvl_3.wav"
                    };

                public static string[] selection = 
                    {"hero_select_1.wav",
                     "hero_select_2.wav",
                     "hero_select_3.wav"
                    };

                public static string[] issueMoveOrder =
                    {"light_order_1.wav",
                     "light_order_2.wav",
                     "light_order_3.wav"
                    };

                public static string[] issueAttackOrder =
                    {"light_order_1.wav",
                     "light_order_2.wav",
                     "light_order_3.wav"
                    };

                public static string[] death =
                    {"hero_death_1.wav",
                     "hero_death_2.wav",
                     "hero_death_3.wav"
                    };
            }

            public static class LightInfantryVoice
            {
                public static string[] enterBattlefield = 
                    {"light_enter_1.wav", 
                     "light_enter_2.wav",
                     "light_enter_3.wav"
                    };

                public static string[] selection = 
                    {"light_select_1.wav",
                     "light_select_2.wav",
                     "light_select_3.wav"
                    };

                public static string[] issueMoveOrder =
                    {"light_order_1.wav",
                     "light_order_2.wav",
                     "light_order_3.wav"
                    };

                public static string[] issueAttackOrder =
                    {"light_order_1.wav",
                     "light_order_2.wav",
                     "light_order_3.wav"
                    };

                public static string[] death =
                    {"light_death_1.wav",
                     "light_death_2.wav",
                     "light_death_3.wav"
                    };
            }

            public static class HeavyInfantryVoice
            {
                public static string[] enterBattlefield = 
                    {"heavy_enter_1.wav", 
                     "heavy_enter_2.wav",
                     "heavy_enter_3.wav"
                    };

                public static string[] selection = 
                    {"heavy_select_1.wav",
                     "heavy_select_2.wav",
                     "heavy_select_3.wav"
                    };

                public static string[] issueMoveOrder =
                    {"light_order_1.wav",
                     "light_order_2.wav",
                     "light_order_3.wav"
                    };

                public static string[] issueAttackOrder =
                    {"light_order_1.wav",
                     "light_order_2.wav",
                     "light_order_3.wav"
                    };

                public static string[] death =
                    {"heavy_death_1.wav",
                     "heavy_death_2.wav",
                     "heavy_death_3.wav"
                    };
            }

            public static class ArcherVoice
            {
                public static string[] enterBattlefield = 
                    {"archer_enter_1.wav", 
                     "archer_enter_2.wav",
                     "archer_enter_3.wav"
                    };

                public static string[] selection = 
                    {"archer_select_1.wav",
                     "archer_select_2.wav",
                     "archer_select_3.wav"
                    };

                public static string[] issueMoveOrder =
                    {"light_order_1.wav",
                     "light_order_2.wav",
                     "light_order_3.wav"
                    };

                public static string[] issueAttackOrder =
                    {"light_order_1.wav",
                     "light_order_2.wav",
                     "light_order_3.wav"
                    };

                public static string[] death =
                    {"archer_death_1.wav",
                     "archer_death_2.wav",
                     "archer_death_3.wav"
                    };
            }
        }

        public static class AmbientSounds{
            public static string main_menu = "mainMenu.wav";
            public static string credits = "credits.wav";
            public static string inPlay = "inPlay.wav";
            public static string victory = "victory.wav";
            public static string defeat = "defeat.wave";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities.AudioCollections
{
    public class MessageCollection : AudioSet
    {
        public string campDestroyed = "CampDestroyed.wav";
        public string baseAttack = "BaseUnderAtt.wav";
        public string cantBuild = "CannotBuildThere.wav";
        public string towerBuilt = "ConComplete.wav";
        public string heroFallen = "HeroFallen.wav";
        public string heroBattle = "HeroinBat.wav";
        public string heroLowHP = "HeroisAbouttoDie.wav";
        public string lvlUp = "HeroLevelUp.wav";
        public string heroAtt = "HeroUnderAtt.wav";
        public string townCapped = "TownCap.wav";
        public string unitAtt = "Uattk.wav";
        public string unitReady= "UnitReady.wav";

        public override void addSound(string type, string fileName)
        {
            switch (type)
            {
                case "campDestroyed": campDestroyed = fileName; break;
                case "baseAttack": baseAttack = fileName; break;
                case "cantBuild": cantBuild = fileName; break;
                case "towerBuilt": towerBuilt = fileName; break;
                case "heroFallen": heroFallen = fileName; break;
                case "heroBattle": heroBattle = fileName; break;
                case "heroLowHP": heroLowHP = fileName; break;
                case "lvlUp": lvlUp = fileName; break;
                case "heroAtt": heroAtt = fileName; break;
                case "townCapped": townCapped = fileName; break;
                case "unitAtt": unitAtt = fileName; break;
                case "unitReady": unitReady = fileName; break;
                default: break;
            }
        }
    }
}

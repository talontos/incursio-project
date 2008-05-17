using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Utils;

namespace Incursio.Entities.AudioCollections
{
    public class AttackCollection : AudioSet
    {
        public string SwordAttack{
            get{
                return Util.selectRandomString(swordAttack);
            }
        }

        public string ArrowAttack{
            get{
                return Util.selectRandomString(arrowAttack);
            }
        }

        public string Explosion{
            get{
                return Util.selectRandomString(explosion);
            }
        }

        public List<string> swordAttack;// = "steelsword.wav";
        public List<string> arrowAttack;// = "bow release.wav";
        public List<string> explosion;// = "explosion.wav";

        public override void addSound(string type, string fileName)
        {
            switch (type)
            {
                case "SwordAttack": swordAttack.Add(fileName); break;
                case "ArrowAttack": arrowAttack.Add(fileName); break;
                case "Explosion": explosion.Add(fileName); break;
                default: break;
            }
        }
    }
}

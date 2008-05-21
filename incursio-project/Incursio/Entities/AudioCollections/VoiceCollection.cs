using System;
using System.Collections.Generic;
using System.Text;

namespace Incursio.Entities.AudioCollections
{
    public class VoiceCollection : AudioSet
    {
        public List<string> enterBattlefield;
        public List<string> levelUp;
        public List<string> selection;
        public List<string> issueMoveOrder;
        public List<string> issueAttackOrder;
        public List<string> death;

        public VoiceCollection() : base(){
            enterBattlefield = new List<string>();
            levelUp = new List<string>();
            selection = new List<string>();
            issueMoveOrder = new List<string>();
            issueAttackOrder = new List<string>();
            death = new List<string>();
        }

        public override void addSound(string type, string fileName){
            switch(type){
                case "enterBattlefield": enterBattlefield.Add(fileName); break;
                case "levelUp": levelUp.Add(fileName); break;
                case "selection": selection.Add(fileName); break;
                case "issueMoveOrder": issueMoveOrder.Add(fileName); break;
                case "issueAttackOrder": issueAttackOrder.Add(fileName); break;
                case "death": death.Add(fileName); break;
                default: break;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;
using Incursio.Entities.AudioCollections;
using Incursio.Managers;

namespace Incursio.Entities.Components
{
    public class AudioComponent : BaseComponent
    {
        public AudioCollection audioCollection;

        public AudioComponent(BaseGameEntity e) : base(e){

        }

        public override void setAttributes(List<KeyValuePair<string, string>> attributes)
        {
            base.setAttributes(attributes);

            for(int i = 0; i < attributes.Count; i++){
                switch(attributes[i].Key){
                    case "collectionName": this.audioCollection = SoundCollection.getInstance().getCollectionByName(attributes[i].Value); break;
                    default: break;
                }
            }

            //remove me
            if (audioCollection == null)
                this.bgEntity.audioComponent = null;
        }

        private void playSoundFromSet(List<string> set){
            if(set.Count > 0)
                SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(set), false);
        }

        public void playSelectionSound()
        {
            if (audioCollection.voices != null)
                playSoundFromSet(audioCollection.voices.selection);
        }

        public void playOrderMoveSound(){
            if (audioCollection.voices != null)
                playSoundFromSet(audioCollection.voices.issueMoveOrder);
        }

        public void playOrderAttackSound()
        {
            if (audioCollection.voices != null)
                playSoundFromSet(audioCollection.voices.issueAttackOrder);
        }

        public void playDeathSound()
        {
            if (audioCollection.voices != null)
                playSoundFromSet(audioCollection.voices.death);
        }

        public void playEnterBattlefieldSound()
        {
            if (audioCollection.voices != null)
                playSoundFromSet(audioCollection.voices.enterBattlefield);
        }

        //TODO: ADD FUNCTIONALITY FOR MULTIPLE SOUND TYPES?
        public void playAttackSound(){
            if(audioCollection.attack != null){
                if (audioCollection.attack.swordAttack != null)
                    playSoundFromSet(audioCollection.attack.swordAttack);
                else if (audioCollection.attack.arrowAttack != null)
                    playSoundFromSet(audioCollection.attack.arrowAttack);
                else if (audioCollection.attack.explosion != null)
                    playSoundFromSet(audioCollection.attack.explosion);
            }
        }
    }
}

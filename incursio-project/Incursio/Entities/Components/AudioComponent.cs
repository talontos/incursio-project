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

        public void playSelectionSound()
        {
            SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(audioCollection.voices.selection), false);
        }

        public void playOrderMoveSound()
        {
            SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(audioCollection.voices.issueMoveOrder), false);
        }

        public void playOrderAttackSound()
        {
            SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(audioCollection.voices.issueAttackOrder), false);
        }

        public void playDeathSound()
        {
            SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(audioCollection.voices.death), false);
        }

        public void playEnterBattlefieldSound()
        {
            SoundManager.getInstance().PlaySound(SoundCollection.selectRandomSound(audioCollection.voices.enterBattlefield), false);
        }
    }
}
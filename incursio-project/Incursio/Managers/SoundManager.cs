using System;
using System.Collections.Generic;
using System.Text;
using Incursio.Classes;
using Microsoft.Xna.Framework;
using Incursio.Utils;
using IrrKlang;

namespace Incursio.Managers
{
    class SoundManager
    {
        ISoundEngine soundEngine;
        private static SoundManager instance;

        public static SoundManager getInstance()
        {
            if (instance == null)
            {
                instance = new SoundManager();
                instance.InitializeEngine();
            }
            return (instance);
        }

        //Initializes the audio engine
        public void InitializeEngine()
        {
            ISoundEngine soundEngine = new ISoundEngine();
        }

        //update the sound engine
        //public void updateSounds()
        //{
        //    soundEngine.Update();
        //}

        //Plays the sound
        public void PlaySound(String file, bool loop)
        {
            soundEngine.Play2D(file, loop);
        }
    
        //Stops the sound
        public void StopSound(String file)
        {
            //method isn't needed using irrKlang
        }

        public ISoundEngine getAudioEngine()
        {
            return (soundEngine);
        }
    }    
}
    
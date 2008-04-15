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
        ISoundEngine soundEngine = null;
        private static SoundManager instance = null;

        private SoundManager()
        {
            soundEngine = new ISoundEngine();
        }

        //creates an instance of the sound manager
        public static SoundManager getInstance()
        {
            if (instance == null)
            {
                instance = new SoundManager();
                //instance.InitializeEngine();
            }
            return instance;
        }

        public void InitializeEngine()
        {
            ISoundEngine soundEngine = new ISoundEngine();
        }

        //update the sound engine
        public void updateSounds()
        {
            try
            {
                soundEngine.Update();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("Null Reference Exception Found!");
                Console.WriteLine("Update Error");
                Console.WriteLine(e);
            }
        }

        //Plays the sound
        //Files must be .mp3, .ogg, or .wav
        //file need to be in the form of "../../../Content/Audio/<File Name Here>"
        //loop will loop the sound if true.  Otherwise, sound will play once and stop
        public void PlaySound(String filename, bool loop) 
        {
            try
            {
                if (soundEngine.IsCurrentlyPlaying(filename) == false)
                {
                    soundEngine.Play2D(filename, loop);
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\n Null Reference Exception Found!");
                Console.WriteLine("Song File error");
                Console.WriteLine(e);
            }
        }
    
        //Stops the sound
        public void StopSound(String file)
        {
            soundEngine.StopAllSounds();
        }

        public ISoundEngine getAudioEngine()
        {
            return (soundEngine);
        }
    }    
}
    
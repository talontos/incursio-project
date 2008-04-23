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

        public SoundManager()
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

        //public void InitializeEngine()
        //{
        //    ISoundEngine soundEngine = new ISoundEngine();
        //}

        //update the sound engine
        //This class is pointless unless the game is using 3D sound
        //We may try 3D sounds later
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
        //file need to be in the form of "../../../Content/Audio/<File Name Here>". Make sure to include the extension
        //loop will loop the sound if true.  Otherwise, sound will play once and stop
        public void PlaySound(String filename, bool loop) 
        {
            try
            {
                //This makes sure that the file will only start once and not stack on top of itself over and over.
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

        //Works the same way as playSound, but does so in a 3D environment
        public void PlaySound3D(String filename, float xVal, float yVal, bool loop)
        {
            try
            {
                soundEngine.Play3D(filename, xVal, yVal, 0, loop);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\n Null Reference Exception Found!");
                Console.WriteLine("Song File error");
                Console.WriteLine(e);
            }
        }
    
        //Stops the sound
        //IrrKlangs only stop method.  Will cause all sounds currently being played by the engine to stop
        public void StopSound(String file)
        {
            soundEngine.StopAllSounds();
        }


        //Method for volume control
        //Pass in a floating point to control volume.
        //0.0 = muted, 1.0 = full volume (this is in accordance to the speaker level)
        //public void setVolume(float volLev)
        //{
        //    soundEngine.g
        //}

        public ISoundEngine getAudioEngine()
        {
            return (soundEngine);
        }
    }    
}
    
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TM2
{
    class AudioManager
    {
        List<Song> songs = new List<Song>();
        List<String> names = new List<String>();
        float volume;
        bool increase, fading;
        float targetVolume;
        int time, targetTime;
        public void LoadContent()
        {

        }

        public void UnloadContent()
        {

        }

        public void Update()
        {
            if (fading)
            {
                if (increase && targetVolume != MediaPlayer.Volume && time != targetTime)
                    MediaPlayer.Volume += .01f;
                else if (!increase && targetVolume != MediaPlayer.Volume && time != targetTime)
                    MediaPlayer.Volume -= .01f;
                else
                    fading = false;
            }
            time++;
        }
        //Target time is actually the time between fades representing frames
        public void fadeIn(float targetVolume, int targetTime)
        {
            increase = true;
            fading = true;
            this.targetVolume = targetVolume;
            this.targetTime = targetTime;
        }
        //Target time is actually the time between fades representing frames
        public void fadeOut(float targetVolume, int targetTime)
        {
            increase = false;
            fading = true;
            this.targetVolume = targetVolume;
            this.targetTime = targetTime;
        }
        public void Play(int songNum)
        {
            MediaPlayer.Play(songs[songNum]);
        }
        public void Draw()
        {

        }
    }
}

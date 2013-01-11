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
        public List<Song> songs = new List<Song>();
        float volume;
        bool increase, fading;
        float targetVolume, sourceVolume;
        TimeSpan time, targetTime;
        private MusicFadeEffect fadeEffect;
        private bool isMusicPaused = false;

        private Song currentSong = null;
        public bool IsSongPaused { get { return currentSong != null && isMusicPaused; } }

        /// <summary>
        /// Gets the name of the currently playing song, or null if no song is playing.
        /// </summary>
        public string CurrentSong { get; private set; }

        /// <summary>
        /// Gets or sets the volume to play songs. 1.0f is max volume.
        /// </summary>
        public float MusicVolume
        {
            get { return MediaPlayer.Volume; }
            set { MediaPlayer.Volume = value; }
        }

        public void LoadContent()
        {

        }

        public void UnloadContent()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (fading && !isMusicPaused)
            {
                if (songs != null && MediaPlayer.State == MediaState.Playing)
                {
                    if (fadeEffect.Update(gameTime.ElapsedGameTime))
                    {
                        fading = false;
                    }

                    MediaPlayer.Volume = fadeEffect.GetVolume();
                }
                else
                {
                    fading = false;
                }
            }
        }
        /// <summary>
        /// Smoothly transition between two volumes.
        /// </summary>
        /// <param name="targetVolume">Target volume, 0.0f to 1.0f</param>
        /// <param name="duration">Length of volume transition</param>
        public void FadeSong(float targetVolume, TimeSpan duration)
        {
            fadeEffect = new MusicFadeEffect(MediaPlayer.Volume, targetVolume, duration);
            fading = true;
        }

        //Target time is actually the time between fades representing frames
        public void fadeIn(float targetVolume, TimeSpan targetTime)
        {
            increase = true;
            fading = true;
            this.targetVolume = targetVolume;
            this.targetTime = targetTime;
        }

        //Target time is actually the time between fades representing frames
        public void fadeOut(float targetVolume, TimeSpan targetTime)
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

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        public void PlaySong(string songName)
        {
            PlaySong(songName, false);
        }

        /// <summary>
        /// Starts playing the song with the given name. If it is already playing, this method
        /// does nothing. If another song is currently playing, it is stopped first.
        /// </summary>
        /// <param name="songName">Name of the song to play</param>
        /// <param name="loop">True if song should loop, false otherwise</param>
        public void PlaySong(string songName, bool loop)
        {
            if (CurrentSong != songName)
            {
                if (currentSong != null)
                {
                    MediaPlayer.Stop();
                }

                CurrentSong = songName;

                isMusicPaused = false;
                MediaPlayer.IsRepeating = loop;
                MediaPlayer.Play(currentSong);
            }
        }

        #region MusicFadeEffect
        private struct MusicFadeEffect
        {
            public float SourceVolume;
            public float TargetVolume;

            private TimeSpan _time;
            private TimeSpan _duration;

            public MusicFadeEffect(float sourceVolume, float targetVolume, TimeSpan duration)
            {
                SourceVolume = sourceVolume;
                TargetVolume = targetVolume;
                _time = TimeSpan.Zero;
                _duration = duration;
            }

            public bool Update(TimeSpan time)
            {
                _time += time;

                if (_time >= _duration)
                {
                    _time = _duration;
                    return true;
                }

                return false;
            }

            public float GetVolume()
            {
                return MathHelper.Lerp(SourceVolume, TargetVolume, (float)_time.Ticks / _duration.Ticks);
            }
        }
        #endregion

        public void Draw()
        {

        }
    }
}

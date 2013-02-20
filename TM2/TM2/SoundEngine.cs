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
    public class SoundEngine
    {
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;

        // 3D audio objects
        AudioEmitter emitter = new AudioEmitter();
        AudioListener listener = new AudioListener();
        Cue cue;

        ContentManager content;

        public void LoadContent(ContentManager content)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");

            audioEngine = new AudioEngine("bin/x86/Debug/Content/Audio/TM2Audio.xgs");
            waveBank = new WaveBank(audioEngine, "bin/x86/Debug/Content/Audio/Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, "bin/x86/Debug/Content/Audio/Sound Bank.xsb");

            // Set emitter and listener position.
            emitter.Position = new Vector3(0f, 0f, 0f);
            listener.Position = new Vector3(0f, 0f, 0f);

            cue = soundBank.GetCue("zombie moan");

            cue.Apply3D(listener, emitter);

            cue.Play();
        }

        public void PlaySound(String soundCue)
        {
            soundBank.PlayCue(soundCue);
        }

        private Vector3 CalculateLocation(float angle, float distance)
        {
            return new Vector3(
                (float)Math.Cos(angle) * distance,
                0,
                (float)Math.Sin(angle) * distance);
        }

        public void Update(GameTime gameTime, Entity e, Entity e2)
        {
            // Move the sound left and right out to maximum distance.
            emitter.Position = new Vector3(
                e.Position.X, e.Position.Y, 0.0f);

            listener.Position = new Vector3(
                e2.Position.X, e2.Position.Y, 0.0f);

            cue.Apply3D(listener, emitter); // apply the 3D transform to the cue
        }
    }
}

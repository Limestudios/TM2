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
    public class CreditsScreen : GameScreen
    {
        SpriteFont font;
        FileManager fileManager;
        GUIManager gui;
        AudioManager audio;

        Video video;
        VideoPlayer videoPlayer;
        Texture2D videoTexture;

        bool dul;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            font = this.content.Load<SpriteFont>("CreditsScreen/Coolvetica Rg");

            gui = new GUIManager();
            gui.LoadContent(Content, "Credits");

            audio = new AudioManager();
            audio.LoadContent(Content, "Credits");

            dul = false;

            fileManager = new FileManager();
            fileManager.LoadContent("Load/Credits.txt");

            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                {
                    switch (fileManager.Attributes[i][j])
                    {
                        case "Videos" :
                            video = this.content.Load<Video>(fileManager.Contents[i][j]);
                            videoPlayer = new VideoPlayer();
                            videoPlayer.Play(video);
                            videoPlayer.IsLooped = true;
                            videoPlayer.Pause();
                            break;
                    }
                }
            }

            audio.PlaySong(0, true);
            audio.FadeSong(1.0f, new TimeSpan(0, 0, 0, 1));
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
            videoPlayer.Stop();
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            if (inputManager.KeyDown(Keys.Space, Keys.Enter))
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);

            if (inputManager.KeyPressed(Keys.D))
                dul = !dul;

            if (dul)
            {
                videoPlayer.Resume();
                MediaPlayer.Pause();
            }
            else
            {
                videoPlayer.Pause();
                MediaPlayer.Resume();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Only call GetTexture if a video is playing or paused
            if (dul)
                videoTexture = videoPlayer.GetTexture();
            else
                videoTexture = null;

            // Drawing to the rectangle will stretch the 
            // video to fill the screen
            Rectangle screen = new Rectangle((int)ScreenManager.Instance.Dimensions.X - video.Width / 2, (int)ScreenManager.Instance.Dimensions.Y - video.Height / 2, video.Width / 2, video.Height / 2);

            spriteBatch.Begin();
            // Draw the video, if we have a texture to draw.
            if (videoTexture != null)
            {
                spriteBatch.Draw(videoTexture, screen, Color.White);
            }
            spriteBatch.DrawString(font, "Made by Liam Craver (Lime Studios)", new Vector2(100, 200), Color.Black);
            spriteBatch.DrawString(font, "Based off the members of Team Mongoose", new Vector2(100, 300), Color.Black);
            gui.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}

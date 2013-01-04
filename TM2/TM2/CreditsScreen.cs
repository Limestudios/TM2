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
        Song song;
        FileManager fileManager;

        Video video;
        VideoPlayer videoPlayer;
        Texture2D videoTexture;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            font = content.Load<SpriteFont>("CreditsScreen/Coolvetica Rg");

            fileManager = new FileManager();
            fileManager.LoadContent("Load/Credits.txt", attributes, contents);

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Sounds":
                            song = content.Load<Song>(contents[i][j]);
                            MediaPlayer.Play(song);
                            MediaPlayer.Volume = 0.1f;
                            MediaPlayer.IsRepeating = true;
                            break;
                        case "Videos" :
                            video = Content.Load<Video>(contents[i][j]);
                            videoPlayer = new VideoPlayer();
                            break;
                    }
                }
            }
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

            if (inputManager.KeyDown(Keys.D))
            {
                if (videoPlayer.State == MediaState.Stopped)
                {
                    videoPlayer.IsLooped = true;
                    videoPlayer.Play(video);
                }
            }
                
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Only call GetTexture if a video is playing or paused
            if (videoPlayer.State != MediaState.Stopped)
                videoTexture = videoPlayer.GetTexture();

            // Drawing to the rectangle will stretch the 
            // video to fill the screen
            Rectangle screen = new Rectangle(0, 0, 1280, 720);

            // Draw the video, if we have a texture to draw.
            if (videoTexture != null)
            {
                spriteBatch.Draw(videoTexture, screen, Color.White);
                spriteBatch.DrawString(font, "Happy Birthday Dul!", new Vector2(100, 100), Color.Black);
            }
        }
    }
}

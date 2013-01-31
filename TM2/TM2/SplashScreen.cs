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
    public class SplashScreen : GameScreen
    {
        SpriteFont font;

        List<FadeAnimation> fade;
        List<Texture2D> images;
        Song song;

        FileManager fileManager;

        AudioManager audio;

        int imageNumber;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            //The splash screen Text
            font = this.content.Load<SpriteFont>("SplashScreen/Coolvetica Rg");

            imageNumber = 0;
            fileManager = new FileManager();
            audio = new AudioManager();
            fade = new List<FadeAnimation>();
            images = new List<Texture2D>();

            fileManager.LoadContent("Load/Splash.txt", attributes, contents);

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Image":
                            images.Add(this.content.Load<Texture2D>(contents[i][j]));
                            fade.Add(new FadeAnimation());
                            break;
                        case "Songs":
                            string[] temp = contents[i][j].Split(',');
                            song = this.content.Load<Song>(temp[1]);
                            audio.songs.Add(song);
                            break;
                    }
                }
            }

            for (int i = 0; i < fade.Count; i++)
            {
                fade[i].LoadContent(content, images[i], "", new Vector2(ScreenManager.Instance.Dimensions.X / 2 - images[i].Bounds.Width / 2, ScreenManager.Instance.Dimensions.Y / 2 - images[i].Bounds.Height / 2));
                fade[i].Scale = 1.0f;
                fade[i].IsActive = true;
                fade[i].FadeSpeed = 0.8f;
                fade[i].Alpha = 0.000000001f;
                fade[i].Increase = true;
                fade[i].Timer = new TimeSpan(0, 0, 5);
            }

            audio.Play(0);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
            inputManager = null;
            attributes.Clear();
            contents.Clear();
            this.content.Unload();
            attributes.Clear();
            contents.Clear();
            attributes.Clear();
            contents.Clear();
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            fade[imageNumber].Update(gameTime);

            if(fade[imageNumber].Alpha == 0.0f)
                imageNumber++;

            if (imageNumber + 1 > fade.Count || inputManager.KeyPressed(Keys.Enter))
            {
                //probably make it so the screenmanager handles this but for now...
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (imageNumber < fade.Count)
                fade[imageNumber].Draw(spriteBatch);
        }
    }
}

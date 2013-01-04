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
        List<SoundEffect> sounds;

        FileManager fileManager;

        int imageNumber;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            //The splash screen Text
            font = content.Load<SpriteFont>("SplashScreen/Coolvetica Rg");

            imageNumber = 0;
            fileManager = new FileManager();
            fade = new List<FadeAnimation>();
            images = new List<Texture2D>();
            sounds = new List<SoundEffect>();

            fileManager.LoadContent("Load/Splash.txt", attributes, contents);

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Image":
                            images.Add(content.Load<Texture2D>(contents[i][j]));
                            fade.Add(new FadeAnimation());
                            break;
                        case "Sounds":
                            sounds.Add(content.Load<SoundEffect>(contents[i][j]));
                            break;
                    }
                }
            }

            for (int i = 0; i < fade.Count; i++)
            {
                fade[i].LoadContent(content, images[i], "", new Vector2(ScreenManager.Instance.Dimensions.X / 2 - images[i].Bounds.Width / 2, ScreenManager.Instance.Dimensions.Y / 2 - images[i].Bounds.Height / 2));
                fade[i].Scale = 1.0f;
                fade[i].IsActive = true;
                fade[i].FadeSpeed = 0.6f;
                fade[i].Alpha = 2.0f;
                fade[i].Increase = true;
                fade[i].Timer = new TimeSpan(0, 0, 10);
            }

            for (int i = 0; i < sounds.Count; i++)
            {
                sounds[i].Play();
            }
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            fileManager = null;
            inputManager = null;
            attributes.Clear();
            contents.Clear();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();

            fade[imageNumber].Update(gameTime);

            if(fade[imageNumber].Alpha == 0.0f)
                imageNumber++;

            if (imageNumber >= fade.Count - 1 || inputManager.KeyPressed(Keys.Enter))
            {
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (imageNumber <= fade.Count)
                fade[imageNumber].Draw(spriteBatch);
        }
    }
}

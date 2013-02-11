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

        List<Animation> animation;
        List<Texture2D> images;
        Song song;

        FileManager fileManager;
        AudioManager audio;
        int imageNumber;

        FadeAnimation fAnimation;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            //The splash screen Text
            font = this.content.Load<SpriteFont>("SplashScreen/Coolvetica Rg");

            imageNumber = 0;
            fileManager = new FileManager();
            audio = new AudioManager();
            animation = new List<Animation>();
            fAnimation = new FadeAnimation();
            images = new List<Texture2D>();

            fileManager.LoadContent("Load/Splash.txt", attributes, contents);
            audio = new AudioManager();
            audio.LoadContent(content, "Splash");

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Image":
                            images.Add(this.content.Load<Texture2D>(contents[i][j]));
                            animation.Add(new FadeAnimation());
                            break;
                    }
                }
            }

            for (int i = 0; i < animation.Count; i++)
            {
                animation[i].LoadContent(content, images[i], "", new Vector2(ScreenManager.Instance.Dimensions.X / 2 - images[i].Bounds.Width / 2, ScreenManager.Instance.Dimensions.Y / 2 - images[i].Bounds.Height / 2));
                animation[i].Scale = 1.0f;
                animation[i].IsActive = true;
                animation[i].Alpha = 1.0f;
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
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            audio.Update(gameTime);

            Animation a = animation[imageNumber];
            animation[imageNumber] = a;

            if (animation[imageNumber].Alpha == 0.0f)
                imageNumber++;

            if (imageNumber + 1 == animation.Count && animation[imageNumber].Alpha == 1.0f || inputManager.KeyPressed(Keys.Enter))
            {
                //probably make it so the screenmanager handles this but for now...
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }
            else
            {
                fAnimation.Update(gameTime, ref a);
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animation[imageNumber].Draw(spriteBatch);
        }
    }
}
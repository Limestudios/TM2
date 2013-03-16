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
        List<string> splashItems, animationTypes;
        List<Texture2D> splashImages;
        List<Animation> Animation;
        FadeAnimation fAnimation;
        SpriteSheetAnimation ssAnimation;

        FileManager fileManager;
        AudioManager audio;

        Vector2 position;
        Rectangle source;
        SpriteFont font;

        int axis, itemNumber;
        List<float> alpha;
        float fadeSpeed;
        String align;

        private void SetSplashItems()
        {
            for (int i = 0; i < splashItems.Count; i++)
            {
                if (splashImages.Count == i)
                    splashImages.Add(ScreenManager.Instance.NullImage);
            }
            for (int i = 0; i < splashImages.Count; i++)
            {
                if (splashItems.Count == i)
                    splashItems.Add("");
            }
        }

        private void SetAnimations()
        {
            Vector2 dimensions = Vector2.Zero;
            Vector2 pos = Vector2.Zero;

            if (align.Contains("Center"))
            {
                for (int i = 0; i < splashItems.Count; i++)
                {
                    dimensions.X += splashImages[i].Width;
                    dimensions.Y += splashImages[i].Height;
                }

                if (axis == 1)
                {
                    pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;
                }
                else if (axis == 2)
                {
                    pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                }
                else
                {
                    pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;
                    pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                }
            }
            else
            {
                pos = position;
            }

            for (int i = 0; i < splashImages.Count; i++)
            {
                dimensions = new Vector2(splashImages[i].Width,
                    splashImages[i].Height);

                if (axis == 1)
                    pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.X) / 2;
                else if (axis == 2)
                    pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.Y) / 2;
                else
                {
                    pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;
                    pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                }

                Animation.Add(new FadeAnimation());
                Animation[Animation.Count - 1].LoadContent(content, splashImages[i], splashItems[i], pos);
                Animation[Animation.Count - 1].Font = font;

                if (axis == 1)
                {
                    pos.X += dimensions.X;
                }
                else if (axis == 2)
                {
                    pos.Y += dimensions.Y;
                }
                else
                {
                    pos.X += dimensions.X;
                    pos.Y += dimensions.Y;
                }
            }

            for (int i = 0; i < Animation.Count; i++)
            {
                Animation[i].Alpha = alpha[i];
                fAnimation.FadeSpeed = fadeSpeed;
            }
        }

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            splashItems = new List<string>();
            splashImages = new List<Texture2D>();

            Animation = new List<Animation>();
            fAnimation = new FadeAnimation();
            fAnimation.DefaultAlpha = 0.001f;

            ssAnimation = new SpriteSheetAnimation();
            animationTypes = new List<string>();

            alpha = new List<float>();
            itemNumber = 0;
            position = Vector2.Zero;
            align = "Center";

            fileManager = new FileManager();
            fileManager.LoadContent("Load/Splash.txt");

            audio = new AudioManager();
            audio.LoadContent(content, "Splash");
            audio.PlaySong(0);
            audio.MusicVolume = 0.0f;
            audio.FadeSong(1.0f, new TimeSpan(0,0,1));

            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                {
                    switch (fileManager.Attributes[i][j])
                    {
                        case "Font":
                            font = this.content.Load<SpriteFont>(fileManager.Contents[i][j]);
                            break;
                        case "Item":
                            splashItems.Add(fileManager.Contents[i][j]);
                            break;
                        case "Image":
                            splashImages.Add(content.Load<Texture2D>(fileManager.Contents[i][j]));
                            break;
                        case "Axis":
                            axis = int.Parse(fileManager.Contents[i][j]);
                            break;
                        case "Position":
                            string[] temp = fileManager.Contents[i][j].Split(' ');
                            position = new Vector2(float.Parse(temp[0]),
                                float.Parse(temp[1]));
                            break;
                        case "Animation":
                            animationTypes.Add(fileManager.Contents[i][j]);
                            break;
                        case "Alpha":
                            alpha.Add(float.Parse(fileManager.Contents[i][j]));
                            break;
                        case "FadeSpeed":
                            fadeSpeed = float.Parse(fileManager.Contents[i][j]);
                            break;
                        case "Align":
                            align = fileManager.Contents[i][j];
                            break;
                    }
                }
            }
            SetSplashItems();
            SetAnimations();
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

            for (int i = 0; i < splashImages.Count; i++)
            {
                if (itemNumber == i)
                {
                    Animation[i].IsActive = true;
                }
                else
                {
                    Animation[i].IsActive = false;
                }

                Animation a = Animation[i];

                if (Animation[itemNumber].Alpha == 0.0f && fAnimation.Increase == true)
                {
                    itemNumber++;
                }

                if (itemNumber + 1 == Animation.Count && fAnimation.Increase == false || inputManager.KeyPressed(Keys.Enter))
                {
                    //probably make it so the screenmanager handles this but for now...
                    ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
                    audio.FadeSong(0.0f, new TimeSpan(0, 0, 0, 0, 800));
                }
                else
                {
                    fAnimation.Update(gameTime, ref a);
                }
            }

            /*if (imageNumber + 1 == Animation.Count && Animation[imageNumber].Alpha == 1.0f || inputManager.KeyPressed(Keys.Enter))
            {
                //probably make it so the screenmanager handles this but for now...
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }
            else
            {
                fAnimation.Update(gameTime, ref a);
            }*/

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < Animation.Count; i++)
            {
                Animation[i].Scale = new Vector2(ScreenManager.Instance.ScreenScale.X, ScreenManager.Instance.ScreenScale.Y);
                Animation[itemNumber].Draw(spriteBatch);
            }
            spriteBatch.End();
        }
    }
}
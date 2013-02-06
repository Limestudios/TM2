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
    public class TitleScreen : GameScreen
    {
        SpriteFont font;
        MenuManager menu;
        FileManager fileManager;
        List<Texture2D> images;
        AudioManager audio;
        Song song;

        List<Rectangle> sourceRect;

        int imageNumber;

        List<float> scale;

        List<Vector2> position;

        String align;

        Vector2 posTemp;

        public override void LoadContent(ContentManager content, InputManager inputManager)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            images = new List<Texture2D>();
            base.LoadContent(content, inputManager);
            font = this.content.Load<SpriteFont>("TitleScreen/Coolvetica Rg");

            menu = new MenuManager();
            menu.LoadContent(content, "Title");

            fileManager = new FileManager();
            fileManager.LoadContent("Load/Title.txt", attributes, contents);
            audio = new AudioManager();

            position = new List<Vector2>();
            scale = new List<float>();
            sourceRect = new List<Rectangle>();

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Images" :
                            images.Add(this.content.Load<Texture2D>(contents[i][j]));
                            break;
                        case "Position":
                            string[] temp = contents[i][j].Split(',');
                            posTemp = new Vector2(float.Parse(temp[0]),
                                float.Parse(temp[1]));
                            break;
                        case "Scale":
                            scale.Add(float.Parse(contents[i][j]));
                            break;
                        case "Songs" :
                            temp = contents[i][j].Split(',');
                            song = this.content.Load<Song>(temp[1]);
                            audio.songs.Add(song);
                            break;
                        case "Align":
                            temp = contents[i][j].Split(',');
                            align = temp[1];
                            if (temp[0] == "Center")
                            {
                                if (align == "X")
                                {
                                    position.Add(new Vector2((ScreenManager.Instance.Dimensions.X - images[i].Width * scale[i]) / 2, posTemp.Y));
                                }
                                else if (align == "Y")
                                {
                                    position.Add(new Vector2(posTemp.X, (ScreenManager.Instance.Dimensions.Y - images[i].Height * scale[i]) / 2));
                                }
                            }
                            else
                            {
                                position.Add(posTemp);
                            }
                            break;
                    }
                }
                sourceRect.Add(new Rectangle(0, 0, images[imageNumber].Width, images[imageNumber].Height));
            }
            audio.PlaySong(1, true);
            //audio.FadeSong(0.0f, new TimeSpan(0, 0, 5));
        }

        public override void UnloadContent()
        {
            menu.UnloadContent();
            inputManager = null;
            attributes.Clear();
            contents.Clear();
            attributes.Clear();
            contents.Clear();
            content.Unload();
            MediaPlayer.Stop();
            this.content.Unload();
            audio.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            menu.Update(gameTime, inputManager);
            audio.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int k = 0; k < images.Count; k++)
            {
                spriteBatch.Draw(images[k], position[k], sourceRect[k], Color.White, 0.0f, Vector2.Zero, scale[k], SpriteEffects.None, 1.0f);
            }
            menu.Draw(spriteBatch);
        }
    }
}

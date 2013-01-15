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
    public class OptionsScreen : GameScreen
    {
        SpriteFont font;
        OptionsManager options;
        FileManager fileManager;
        List<Texture2D> images;
        AudioManager audio;
        Song song;

        protected Rectangle sourceRect;

        int imageNumber;

        protected float scale;

        Vector2 position;

        public override void LoadContent(ContentManager content, InputManager inputManager)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            images = new List<Texture2D>();
            base.LoadContent(content, inputManager);
            font = this.content.Load<SpriteFont>("TitleScreen/Coolvetica Rg");

            imageNumber = 0;

            options = new OptionsManager();
            audio = new AudioManager();
            options.LoadContent(content, "Options");

            fileManager = new FileManager();
            fileManager.LoadContent("Load/Options.txt", attributes, contents, "OptionsScreen");

            position = Vector2.Zero;

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Songs":
                            String[] temp = contents[i][j].Split(',');
                            song = this.content.Load<Song>(temp[1]);
                            audio.songs.Add(song);
                            audio.PlaySong(0, true);
                            break;
                    }
                }
            }
        }

        public override void UnloadContent()
        {
            options.UnloadContent();
            inputManager = null;
            attributes.Clear();
            contents.Clear();
            attributes.Clear();
            contents.Clear();
            content.Unload();
            MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            options.Update(gameTime, inputManager);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            options.Draw(spriteBatch);
        }
    }
}
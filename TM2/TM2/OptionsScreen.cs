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
        AudioManager audioManager;
        List<Texture2D> images;
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
            options.LoadContent(content, "Options");

            audioManager = new AudioManager();
            audioManager.LoadContent(content, "Options");
            audioManager.PlaySong(0, true);

            fileManager = new FileManager();
            fileManager.LoadContent("Load/Options.txt", attributes, contents, "OptionsScreen");

            position = Vector2.Zero;
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
            spriteBatch.Begin();
            options.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
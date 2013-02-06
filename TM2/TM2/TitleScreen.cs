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
        List<Texture2D> images;
        public AudioManager audio;
        GUIManager gui;

        public override void LoadContent(ContentManager content, InputManager inputManager)
        {
            images = new List<Texture2D>();
            base.LoadContent(content, inputManager);
            font = this.content.Load<SpriteFont>("TitleScreen/Coolvetica Rg");

            gui = new GUIManager();
            gui.LoadContent(content, "Title");

            menu = new MenuManager();
            menu.LoadContent(content, "Title");

            audio = new AudioManager();
            audio.LoadContent(content, "Title");

            audio.PlaySong(0, true);
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
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            menu.Update(gameTime, inputManager, audio);
            gui.Update(gameTime);
            audio.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            gui.Draw(spriteBatch);
            menu.Draw(spriteBatch);
        }
    }
}

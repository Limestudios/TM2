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

using TM2.Helpers;

namespace TM2
{
    public class OptionsScreen : GameScreen
    {
        MenuManager menu;
        List<Texture2D> images;
        public AudioManager audio;
        GUIManager gui;

        public override void LoadContent(ContentManager content, InputManager inputManager)
        {
            images = new List<Texture2D>();
            base.LoadContent(content, inputManager);

            menu = new MenuManager();
            menu.LoadContent(content, "Option");

            audio = new AudioManager();
            audio.LoadContent(content, "Option");
            audio.MusicVolume = 1.0f;

            audio.FadeSong(1.0f, new TimeSpan(0, 0, 0, 1));
        }

        public override void UnloadContent()
        {
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
            menu.Update(gameTime, inputManager, audio);

            if (inputManager.KeyPressed(Keys.Back))
            {
                SettingsManager.Save("Settings/GameSettings.xml", SettingsManager.gameSettings);
                SettingsManager.ConfigureGraphicsManager(Game1.graphics, SettingsManager.gameSettings);
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
                audio.FadeSong(0.0f, new TimeSpan(0, 0, 0, 0, 800));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            menu.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}
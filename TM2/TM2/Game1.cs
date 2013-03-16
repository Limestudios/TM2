using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        InputHelper inputHelper;

        public Game1()
        {
            Window.Title = "TEAM MONGOOSE VS ZAMBIES!";
            Content.RootDirectory = "Content";

            //create and configure graphicsDevice
            SettingsManager.GameSettings gameSettings = SettingsManager.Read("Settings/GameSettings.xml");
            graphics = new GraphicsDeviceManager(this);
            ConfigureGraphicsManager(gameSettings);

            this.IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ScreenManager.Instance.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed |
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            /*if (inputHelper.IsKeyPressed(Buttons.F))
            {
                SettingsManager.GameSettings gameSettings = SettingsManager.Read("Settings/GameSettings.xml");
                gameSettings.PreferredFullScreen = !gameSettings.PreferredFullScreen;
                SettingsManager.Save("Settings/GameSettings.xml", gameSettings);
                ConfigureGraphicsManager(gameSettings);
            }*/

            // TODO: Add your update logic here
            ScreenManager.Instance.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            ScreenManager.Instance.Draw(spriteBatch);
            base.Draw(gameTime);
        }

        private void ConfigureGraphicsManager(SettingsManager.GameSettings gameSettings)
        {
            graphics.PreferredBackBufferWidth = gameSettings.PreferredWindowWidth;
            graphics.PreferredBackBufferHeight = gameSettings.PreferredWindowHeight;
            graphics.IsFullScreen = gameSettings.PreferredFullScreen;

            ScreenManager.Instance.Initialize();
            ScreenManager.Instance.ScreenScale = new Vector2((float)gameSettings.PreferredWindowWidth / (float)gameSettings.DefaultWindowWidth, (float)gameSettings.PreferredWindowHeight / (float)gameSettings.DefaultWindowHeight);
            ScreenManager.Instance.Dimensions = new Vector2(gameSettings.PreferredWindowWidth, gameSettings.PreferredWindowHeight);
            graphics.ApplyChanges();
        }
    }
}

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
    public class ScreenManager
    {

        #region Variables
        /// <summary>
        /// Creating our own custom contentManager
        /// </summary>

        ContentManager content;

        /// <summary>
        /// current screen that is being displayed
        /// </summary>
        GameScreen currentScreen;

        /// <summary>
        /// New screen that we will be loading
        /// </summary>
        GameScreen newScreen;

        /// <summary>
        /// Screen Manager Instance ( this is good coding right ;) )
        /// </summary>

        private static ScreenManager instance;

        /// <summary>
        /// Screen Stacking 
        /// </summary>

        Stack<GameScreen> screenStack = new Stack<GameScreen>();

        /// <summary>
        /// Screen's width and height
        /// </summary>

        Vector2 dimensions;

        /// <summary>
        /// let's us no to transition or not
        /// </summary>

        bool transition;

        FadeAnimation fade;

        Texture2D fadeTexture;

        Texture2D nullImage;

        InputManager inputManager;

        AudioManager audio;

        #endregion

        #region Properties

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();
                return instance;
            }
        }

        //you can do this directly in the vector but this makes it easier to visualize
        public Vector2 Dimensions
        {
            get { return dimensions; }
            set { dimensions = value; }
        }

        public Texture2D NullImage
        {
            get { return nullImage; }
        }

        #endregion

        #region Main Methods

        public void AddScreen(GameScreen screen, InputManager inputManager)
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.Alpha = 0.0f;
            fade.ActivateValue = 1.0f;
            this.inputManager = inputManager;
            audio.FadeSong(0.0f, new TimeSpan(0, 0, 0, 0, 1));
        }

        public void AddScreen(GameScreen screen, InputManager inputManager, float alpha)
        {
            transition = true;
            newScreen = screen;
            fade.IsActive = true;
            fade.ActivateValue = 1.0f;
            if (alpha != 1.0f)
                fade.Alpha = 1.0f - alpha;
            else
                fade.Alpha = alpha;
            fade.Increase = true;
            this.inputManager = inputManager;
            audio.FadeSong(1.0f, new TimeSpan(0, 0, 10));
        }

        public void Initialize()
        {
            currentScreen = new TitleScreen();
            fade = new FadeAnimation();
            inputManager = new InputManager();
            audio = new AudioManager();
        }
        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(Content, inputManager);

            fadeTexture = this.content.Load<Texture2D>("fade");
            nullImage = this.content.Load<Texture2D>("null");
            fade.LoadContent(content, fadeTexture, "", Vector2.Zero);
            fade.Scale = dimensions.X;
        }
        public void Update(GameTime gameTime)
        {
            if (!transition)
                currentScreen.Update(gameTime);
            else
                Transition(gameTime);

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
            if (transition)
                fade.Draw(spriteBatch);
        }

        #endregion

        #region Private Methods

        private void Transition(GameTime gameTime)
        {
            fade.Update(gameTime);
            if (fade.Alpha == 1.0f && fade.Timer.TotalSeconds == 2.0f)
            {
                screenStack.Push(newScreen);
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent(content, this.inputManager);
            }
            else if (fade.Alpha == 0.0f)
            {
                transition = false;
                fade.IsActive = false;
            }

            if (fade.Increase == true)
            {
                audio.FadeSong(0.0f, new TimeSpan(0, 0, 0, 1));
            }
            else
            {
                audio.FadeSong(1.0f, new TimeSpan(0, 0, 0, 1));
            }
        }

        #endregion
    }
}

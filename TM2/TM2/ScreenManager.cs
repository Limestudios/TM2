﻿using System;
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

        protected Animation Animation;
        protected SpriteSheetAnimation ssAnimation;
        protected FadeAnimation fAnimation;

        Texture2D fadeTexture;

        Texture2D nullImage;

        InputManager inputManager;

        public ContentManager Content
        {
            get { return content; }
        }

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

        Vector2 screenScale;

        public Vector2 ScreenScale
        {
            get { return screenScale; }
            set { screenScale = value; }
        }

        public Texture2D NullImage
        {
            get { return nullImage; }
        }

        public GraphicsDevice graphicsDevice;
        public GraphicsDeviceManager graphicsDeviceManager;

        #endregion

        #region Main Methods

        public void AddScreen(GameScreen screen, InputManager inputManager)
        {
            transition = true;
            newScreen = screen;
            Animation.IsActive = true;
            Animation.Alpha = 0.0f;
            fAnimation.ActivateValue = 1.0f;
            fAnimation.Increase = true;
            this.inputManager = inputManager;
            //audio.FadeSong(0.0f, new TimeSpan(0, 0, 0, 0, 1200));
        }

        public void AddScreen(GameScreen screen, InputManager inputManager, float alpha)
        {
            transition = true;
            newScreen = screen;
            Animation.IsActive = true;
            fAnimation.ActivateValue = 1.0f;
            if (alpha != 1.0f)
            {
                fAnimation.Alpha = 1.0f - alpha;
            }
            else
            {
                fAnimation.Alpha = alpha;
            }
            fAnimation.Increase = true;
            this.inputManager = inputManager;
            //audio.Play(0);
        }

        public void Initialize()
        {
            Animation = new Animation();
            ssAnimation = new SpriteSheetAnimation();
            fAnimation = new FadeAnimation();

            currentScreen = new SplashScreen();
            inputManager = new InputManager();
        }
        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(Content, inputManager);

            fadeTexture = this.content.Load<Texture2D>("fade");
            nullImage = this.content.Load<Texture2D>("null");
            Animation.LoadContent(content, fadeTexture, "", Vector2.Zero, Color.White);
            Animation.Scale = new Vector2(this.Dimensions.X, this.Dimensions.Y);
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
            {
                spriteBatch.Begin();
                Animation.Draw(spriteBatch);
                spriteBatch.End();
            }
        }

        #endregion

        #region Private Methods

        private void Transition(GameTime gameTime)
        {
            Animation.Scale = new Vector2(this.Dimensions.X, this.Dimensions.Y);
            fAnimation.Update(gameTime, ref Animation);
            if (Animation.Alpha == 1.0f && fAnimation.Timer.TotalSeconds == 1.0f)
            {
                screenStack.Push(newScreen);
                currentScreen.UnloadContent();
                currentScreen = newScreen;
                currentScreen.LoadContent(content, this.inputManager);
            }
            else if (Animation.Alpha == 0.0f)
            {
                transition = false;
                Animation.IsActive = false;
            }
            else if (Animation.Alpha < 1.0f && fAnimation.Increase == false)
            {
                currentScreen.Update(gameTime);
            }
        }

        #endregion
    }
}

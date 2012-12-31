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

        #endregion

        #region Main Methods

        public void AddScreen(GameScreen screen)
        {
            //just in case
            newScreen = screen;
            screenStack.Push(screen);
            currentScreen.UnloadContent();
            currentScreen = newScreen;
            currentScreen.LoadContent(content);
        }

        public void Initialize()
        {
            currentScreen = new SplashScreen();
        }
        public void LoadContent(ContentManager Content)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            currentScreen.LoadContent(Content);
        }
        public void Update(GameTime gameTime)
        {
            currentScreen.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            currentScreen.Draw(spriteBatch);
        }

        #endregion
    }
}

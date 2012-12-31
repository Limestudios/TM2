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
    }
}

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
    public class SplashScreen : GameScreen
    {
        Boolean first = true;
        KeyboardState keyState;
        SpriteFont font;
        SoundEffect splashSound;
        Texture2D splashTexture;
        Texture2D splashBackground;

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            //The splash screen Text
            font = content.Load<SpriteFont>("SplashScreen/Coolvetica Rg");
            //The splash screen sound
            splashSound = content.Load<SoundEffect>("SplashScreen/SplashSound");
            //The splash screen Texture
            splashTexture = content.Load<Texture2D>("SplashScreen/SplashTexture");
            //The splash screen Background Texture
            splashBackground = content.Load<Texture2D>("SplashScreen/SplashBackground");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            if (first) { splashSound.Play(); first = false; }
            if (keyState.IsKeyDown(Keys.Z))
                ScreenManager.Instance.AddScreen(new TitleScreen());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(splashBackground,
                new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(font, "A game by",
                new Vector2(ScreenManager.Instance.Dimensions.X/2, 100), Color.Black);
            spriteBatch.Draw(splashTexture,
                new Vector2(170, 200), Color.White);
        }
    }
}

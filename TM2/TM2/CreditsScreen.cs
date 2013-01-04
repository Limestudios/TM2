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
    public class CreditsScreen : GameScreen
    {
        SpriteFont font;
        MenuManager menu;

        public override void LoadContent(ContentManager Content, InputManager inputManager)
        {
            base.LoadContent(Content, inputManager);
            font = content.Load<SpriteFont>("CreditsScreen/Coolvetica Rg");

            menu = new MenuManager();
            menu.LoadContent(content, "Credits");
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            menu.UnloadContent();
            MediaPlayer.Stop();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            menu.Update(gameTime, inputManager);

            if (inputManager.KeyDown(Keys.Space, Keys.Enter))
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
                
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
            spriteBatch.DrawString(font, "CREDITS", Vector2.Zero, Color.Black);
        }
    }
}

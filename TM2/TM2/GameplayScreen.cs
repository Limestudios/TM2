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
    public class GameplayScreen : GameScreen
    {
        Player player;
        Map map;
        AudioManager audio;
        Camera camera;
        Vector2 parallax;
        Viewport viewport;
        GUIManager guiManager;

        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            player = new Player();
            map = new Map();
            audio = new AudioManager();
            map.LoadContent(content, map, "Map1");
            player.LoadContent(content, input);
            audio.LoadContent(content, "Map1");

            viewport = new Viewport(0, 0, (int)ScreenManager.Instance.Dimensions.X, (int)ScreenManager.Instance.Dimensions.Y);
            camera = new Camera(viewport);
            parallax = Vector2.One;

            guiManager = new GUIManager();
            guiManager.LoadContent(content, "Map1");

            audio.PlaySong(0, true);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            player.Update(gameTime, inputManager, map.collision, map.layer);
            map.Update(gameTime);
            camera.LookAt(player.position);
            guiManager.Update(gameTime);

            if (inputManager.KeyPressed(Keys.R))
            {
                //probably make it so the screenmanager handles this but for now...
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            guiManager.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}

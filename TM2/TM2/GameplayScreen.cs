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
        ParticleEngine particleEngine;

        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            player = new Player();
            map = new Map();
            map.LoadContent(content, map, "Map1");
            player.LoadContent(content, input);
            List<Texture2D> textures = new List<Texture2D>();
            particleEngine = new ParticleEngine(textures, new Vector2(400, 240));
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
            particleEngine.EmitterLocation = new Vector2(player.position.X + 32/2, player.position.Y + 32/2);
            particleEngine.Update(map.collision, gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            map.Draw(spriteBatch);
            player.Draw(spriteBatch);
            particleEngine.Draw(spriteBatch);
        }
    }
}

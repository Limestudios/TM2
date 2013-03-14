using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TM2
{
    public class Map
    {
        public Layer layer;

        string id;

        public string ID
        {
            get { return id; }
        }

        public void LoadContent(ContentManager content, Map map, string mapID)
        {
            layer = new Layer();
            id = mapID;

            layer.LoadContent(map, "Layer1");
        }

        public void UnloadContent()
        {
            //layer.UnloadContent();
            //collision.UnLoadContent();
        }

        public void Update(GameTime gameTime, Camera camera, Map map)
        {
            layer.Update(gameTime, camera, map);
        }

        public void UpdateCollision(ref Entity entity, InputManager inputManager, SoundEngine soundEngine)
        {
            layer.UpdateCollision(ref entity, inputManager, soundEngine);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            layer.Draw(spriteBatch);
        }
    }
}

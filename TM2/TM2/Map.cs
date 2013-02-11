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
        public Collision collision;
        Camera camera;
        Vector2 parallax;
        Viewport viewport;

        string id;

        public string ID
        {
            get { return id; }
        }

        public void LoadContent(ContentManager content, Map map, string mapID)
        {
            layer = new Layer();
            collision = new Collision();
            id = mapID;

            layer.LoadContent(map, "Layer1");
            collision.LoadContent(content, mapID);

            viewport = new Viewport(0, 0, (int)ScreenManager.Instance.Dimensions.X, (int)ScreenManager.Instance.Dimensions.Y);
            camera = new Camera(viewport);
            parallax = Vector2.One;
        }

        public void UnloadContent()
        {
            //layer.UnloadContent();
            //collision.UnLoadContent();
        }

        public void Update(GameTime gameTime)
        {
            layer.Update(gameTime);
        }

        public void UpdateCollision(ref Entity entity, InputManager inputManager)
        {
            layer.UpdateCollision(ref entity, inputManager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            layer.Draw(spriteBatch);
        }
    }
}

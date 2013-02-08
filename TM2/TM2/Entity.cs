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
    public class Entity
    {
        protected int health;
        protected Animation moveAnimation;
        protected SpriteSheetAnimation ssAnimation;
        protected float moveSpeed, jumpSpeed, gravity;

        protected ContentManager content;
        protected FileManager fileManager;

        protected Texture2D image;

        protected List<List<string>> attributes, contents;

        protected Vector2 position, velocity, prevPosition;

        protected bool activateGravity;
        protected bool syncTilePosition;

        public Vector2 PrevPosition
        {
            get { return prevPosition; }
        }

        public Animation Animation
        {
            get { return moveAnimation; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public bool ActivateGravity
        {
            set { activateGravity = value; }
        }

        public bool SyncTilePosition
        {
            get { return syncTilePosition; }
            set { syncTilePosition = value; }
        }

        public virtual void LoadContent(ContentManager content, InputManager input)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
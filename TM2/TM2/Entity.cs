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
    public class Entity
    {
        protected int health;
        protected SpriteSheetAnimation moveAnimation;
        protected float moveSpeed;

        protected ContentManager content;
        protected FileManager fileManager;

        protected Texture2D image;

        protected List<List<string>> attributes, contents;

        protected Vector2 position;

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

        public virtual void Update(GameTime gameTime, InputManager input)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}

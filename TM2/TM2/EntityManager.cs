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
    public class EntityManager
    {
        List<Entity> entities;
        List<List<string>> attributes, contents;
        FileManager fileManager;
        InputManager inputManager;

        public List<Entity> Entities
        {
            get { return entities; }
        }

        public void LoadContent(string entityType, ContentManager Content, string fileName, string identifier, InputManager inputManager)
        {
            this.inputManager = inputManager;
            entities = new List<Entity>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            fileManager = new FileManager();

            if (identifier == String.Empty)
                fileManager.LoadContent(fileName, attributes, contents);
            else
                fileManager.LoadContent(fileName, attributes, contents, identifier);

            for (int i = 0; i < attributes.Count; i++)
            {
                Type newClass = Type.GetType("TM2." + entityType);
                entities.Add((Entity)Activator.CreateInstance(newClass));
                entities[i].LoadContent(Content, attributes[i], contents[i], this.inputManager);
            }
        }

        public void UnloadContent()
        {
            for (int i = 0; i < entities.Count; i++)
                entities[i].UnloadContent();
        }

        public void Update(GameTime gameTime, Map map)
        {
            for (int i = 0; i < entities.Count; i++)
                entities[i].Update(gameTime, inputManager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; i++)
                entities[i].Draw(spriteBatch);
        }
    }
}

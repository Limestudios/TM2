using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TM2
{
    public class Collision
    {
        FileManager fileManager;
        List<List<string>> attributes, contents, collisionMap;
        List<string> row;

        public List<List<string>> CollisionMap
        {
            get { return collisionMap; }
        }

        public void LoadContent(ContentManager content, string mapID)
        {
            fileManager = new FileManager();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            collisionMap = new List<List<string>>();
            row = new List<string>();

            fileManager.LoadContent("Load/Maps/" + mapID + ".txt", attributes, contents, "Collision");

            for (int i = 0; i < contents.Count; i++)
            {
                for (int j = 0; j < contents[i].Count; j++)
                {
                    row.Add(contents[i][j]);
                }
                collisionMap.Add(row);
                row = new List<string>();
            }
        }

        public void Update(GameTime gameTime, ref Vector2 playerPosition, Vector2 playerDimensions, Vector2 tileDimensions)
        {
            
        }
    }
}

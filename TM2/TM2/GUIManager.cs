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
    public class GUIManager
    {
        FileManager fileManager;
        List<Texture2D> images;
        List<List<string>> attributes, contents;
        List<Rectangle> sourceRect;

        int imageNumber;

        List<float> scale;

        List<Vector2> position;

        String align;

        Vector2 posTemp;

        ContentManager content;

        public void LoadContent(ContentManager content, String id)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            images = new List<Texture2D>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();

            imageNumber = 0;

            fileManager = new FileManager();
            fileManager.LoadContent("Load/GUIs.txt", attributes, contents, id);

            position = new List<Vector2>();
            scale = new List<float>();
            sourceRect = new List<Rectangle>();

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Images":
                            images.Add(this.content.Load<Texture2D>(contents[i][j]));
                            break;
                        case "Position":
                            string[] temp = contents[i][j].Split(',');
                            posTemp = new Vector2(float.Parse(temp[0]),
                                float.Parse(temp[1]));
                            break;
                        case "Scale":
                            scale.Add(float.Parse(contents[i][j]));
                            break;
                        case "Align":
                            temp = contents[i][j].Split(',');
                            align = temp[1];
                            if (temp[0] == "Center")
                            {
                                if (align == "X")
                                {
                                    position.Add(new Vector2((ScreenManager.Instance.Dimensions.X - images[i].Width * scale[i]) / 2, posTemp.Y));
                                }
                                else if (align == "Y")
                                {
                                    position.Add(new Vector2(posTemp.X, (ScreenManager.Instance.Dimensions.Y - images[i].Height * scale[i]) / 2));
                                }
                            }
                            else
                            {
                                position.Add(posTemp);
                            }
                            break;
                    }
                }
                sourceRect.Add(new Rectangle(0, 0, images[imageNumber].Width, images[imageNumber].Height));
            }
        }

        public void UnloadContent()
        {
            fileManager = null;
            attributes = null;
            contents = null;
            images = null;
            position = null;
            posTemp = Vector2.Zero;
            align = null;
            scale = null;
            sourceRect = null;
            content.Unload();
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int k = 0; k < images.Count; k++)
            {
                spriteBatch.Draw(images[k], position[k], sourceRect[k], Color.White, 0.0f, Vector2.Zero, scale[k], SpriteEffects.None, 1.0f);
            }
        }
    }
}

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
        List<Rectangle> sourceRect;

        int imageNumber;

        List<Vector2> scale;

        List<Vector2> position;

        String align;

        Vector2 posTemp;

        ContentManager content;

        public void LoadContent(ContentManager content, String id)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            images = new List<Texture2D>();

            imageNumber = 0;

            fileManager = new FileManager();
            fileManager.LoadContent("Load/GUIs.txt", id);

            position = new List<Vector2>();
            scale = new List<Vector2>();
            sourceRect = new List<Rectangle>();

            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                {
                    switch (fileManager.Attributes[i][j])
                    {
                        case "Images":
                            images.Add(this.content.Load<Texture2D>(fileManager.Contents[i][j]));
                            sourceRect.Add(new Rectangle(0, 0, images[i].Width, images[i].Height));
                            break;
                        case "Position":
                            string[] temp = fileManager.Contents[i][j].Split(',');
                            posTemp = new Vector2(float.Parse(temp[0]),
                                float.Parse(temp[1]));
                            break;
                        case "Scale":
                            temp = fileManager.Contents[i][j].Split(',');
                            scale.Add(new Vector2(float.Parse(temp[0]) * ScreenManager.Instance.ScreenScale.X,
                                float.Parse(temp[1]) * ScreenManager.Instance.ScreenScale.Y));
                            break;
                        case "Align":
                            temp = fileManager.Contents[i][j].Split(',');
                            if (temp[0] == "Center")
                            {
                                align = temp[1];
                                if (align == "X")
                                {
                                    position.Add(new Vector2((ScreenManager.Instance.Dimensions.X - images[i].Width * scale[i].X) / 2, posTemp.Y));
                                }
                                else if (align == "Y")
                                {
                                    position.Add(new Vector2(posTemp.X, (ScreenManager.Instance.Dimensions.Y - images[i].Height * scale[i].Y) / 2));
                                }
                            }
                            else
                            {
                                position.Add(posTemp);
                            }
                            break;
                    }
                }
            }
        }

        public void UnloadContent()
        {
            fileManager = null;
            fileManager.Attributes = null;
            fileManager.Contents = null;
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
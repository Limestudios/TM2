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
    public class MenuManager
    {
        List<string> menuItems;
        List<string> animationTypes, linkType, linkID;
        List<Texture2D> menuImages;
        List<Animation> animation;
        FadeAnimation fAnimation;
        SpriteSheetAnimation ssAnimation;

        ContentManager content;
        FileManager fileManager;

        Vector2 position;
        Rectangle source;
        SpriteFont font;

        int axis, itemNumber;
        String align;

        private void SetMenuItems()
        {
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (menuImages.Count == i)
                    menuImages.Add(ScreenManager.Instance.NullImage);
            }
            for (int i = 0; i < menuImages.Count; i++)
            {
                if (menuItems.Count == i)
                    menuItems.Add("");
            }
        }

        private void SetAnimations()
        {
            Vector2 dimensions = Vector2.Zero;
            Vector2 pos = Vector2.Zero;

            if (align.Contains("Center"))
            {
                for (int i = 0; i < menuItems.Count; i++)
                {
                    dimensions.X += font.MeasureString(menuItems[i]).X + menuImages[i].Width;
                    dimensions.Y += font.MeasureString(menuItems[i]).Y + menuImages[i].Height;
                }

                if (axis == 1)
                {
                    pos.X = (ScreenManager.Instance.Dimensions.X - dimensions.X) / 2;
                }
                else if (axis == 2)
                {
                    pos.Y = (ScreenManager.Instance.Dimensions.Y - dimensions.Y) / 2;
                }
            }
            else
            {
                pos = position;
            }

            for (int i = 0; i < menuImages.Count; i++)
            {
                dimensions = new Vector2(font.MeasureString(menuItems[i]).X + menuImages[i].Width,
                    font.MeasureString(menuItems[i]).Y + menuImages[i].Height);

                if (axis == 1)
                    pos.Y = (ScreenManager.Instance.Dimensions.Y) / 2;
                else
                    pos.X = (ScreenManager.Instance.Dimensions.X) / 2 - font.MeasureString(menuItems[i]).X / 2;

                animation.Add(new FadeAnimation());
                animation[animation.Count - 1].LoadContent(content, menuImages[i], menuItems[i], pos);
                animation[animation.Count - 1].Font = font;

                if (axis == 1)
                {
                    pos.X += dimensions.X;
                }
                else
                {
                    pos.Y += dimensions.Y;
                }
            }
        }

        public void LoadContent(ContentManager content, string id)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");
            menuItems = new List<string>();
            menuImages = new List<Texture2D>();
            animation = new List<Animation>();
            fAnimation = new FadeAnimation();
            ssAnimation = new SpriteSheetAnimation();
            animationTypes = new List<string>();
            linkID = new List<string>();
            linkType = new List<string>();
            itemNumber = 0;

            position = Vector2.Zero;
            fileManager = new FileManager();
            fileManager.LoadContent("Load/Menus.txt", id);

            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                {
                    switch (fileManager.Attributes[i][j])
                    {
                        case "Font":
                            font = this.content.Load<SpriteFont>(fileManager.Contents[i][j]);
                            break;
                        case "Item":
                            menuItems.Add(fileManager.Contents[i][j]);
                            break;
                        case "Image":
                            menuImages.Add(this.content.Load<Texture2D>(fileManager.Contents[i][j]));
                            break;
                        case "Axis":
                            axis = int.Parse(fileManager.Contents[i][j]);
                            break;
                        case "Position":
                            string[] temp = fileManager.Contents[i][j].Split(' ');
                            position = new Vector2(float.Parse(temp[0]),
                                float.Parse(temp[1]));
                            break;
                        case "Source":
                            temp = fileManager.Contents[i][j].Split(' ');
                            source = new Rectangle(int.Parse(temp[0]),
                                int.Parse(temp[1]), int.Parse(temp[2]),
                                int.Parse(temp[3]));
                            break;
                        case "Animation":
                            animationTypes.Add(fileManager.Contents[i][j]);
                            break;
                        case "Align":
                            align = fileManager.Contents[i][j];
                            break;
                        case "LinkType":
                            linkType.Add(fileManager.Contents[i][j]);
                            break;
                        case "LinkID":
                            linkID.Add(fileManager.Contents[i][j]);
                            break;
                    }
                }
            }
            SetMenuItems();
            SetAnimations();
        }

        public void UnloadContent()
        {
            content.Unload();
            position = Vector2.Zero;
            animation.Clear();
            menuImages.Clear();
            menuItems.Clear();
            animationTypes.Clear();
        }

        public void Update(GameTime gameTime, InputManager inputManager, AudioManager audio)
        {
            if (axis == 1)
            {
                if (inputManager.KeyPressed(Keys.Right, Keys.D))
                    itemNumber++;
                else if (inputManager.KeyPressed(Keys.Left, Keys.A))
                    itemNumber--;
            }
            else
            {
                if (inputManager.KeyPressed(Keys.Down, Keys.S))
                    itemNumber++;
                else if (inputManager.KeyPressed(Keys.Up, Keys.W))
                    itemNumber--;
            }

            if (inputManager.KeyPressed(Keys.Enter, Keys.Space))
            {
                if (linkType[itemNumber] == "Screen")
                {
                    Type newClass = Type.GetType("TM2." + linkID[itemNumber]);
                    ScreenManager.Instance.AddScreen((GameScreen)Activator.CreateInstance(newClass), inputManager);
                }
            }

            if (itemNumber < 0)
                itemNumber = menuItems.Count - 1;
            else if (itemNumber > menuItems.Count - 1)
                itemNumber = 0;

            for (int i = 0; i < animation.Count; i++)
            {
                for (int j = 0; j < animationTypes.Count; j++)
                {
                    if (itemNumber == i)
                        animation[i].IsActive = true;
                    else
                        animation[i].IsActive = false;

                    Animation a = animation[i];

                    switch (animationTypes[j])
                    {
                        case "Fade" :
                            fAnimation.Update(gameTime, ref a);
                            break;
                        case "SSheet":
                            ssAnimation.Update(gameTime, ref a);
                            break;
                    }

                    animation[i] = a;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < animation.Count; i++)
            {
                animation[i].Draw(spriteBatch);
            }
        }
    }
}
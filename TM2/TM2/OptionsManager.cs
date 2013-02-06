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
    public class OptionsManager
    {
        List<string> optionsItems;
        List<string> animationTypes, linkType, linkID;
        List<Texture2D> optionsImages;
        List<List<Animation>> animation;
        List<List<string>> attributes, contents;
        List<Animation> tempAnimation;
        List<int> variable;
        Song song;

        ContentManager content;
        FileManager fileManager;
        AudioManager audio;

        Vector2 position, limits;
        Rectangle source;
        SpriteFont font;

        int axis, itemNumber, optionNumber;
        String align;

        private void SetoptionsItems()
        {
            for (int i = 0; i < optionsItems.Count; i++)
            {
                if (optionsImages.Count == i)
                    optionsImages.Add(ScreenManager.Instance.NullImage);
            }
            for (int i = 0; i < optionsImages.Count; i++)
            {
                if (optionsItems.Count == i)
                    optionsItems.Add("");
            }
        }

        private void SetAnimations()
        {
            Vector2 dimensions = Vector2.Zero;
            Vector2 pos = Vector2.Zero;

            if (align.Contains("Center"))
            {
                for (int i = 0; i < optionsItems.Count; i++)
                {
                    dimensions.X += font.MeasureString(optionsItems[i]).X + optionsImages[i].Width;
                    dimensions.Y += font.MeasureString(optionsItems[i]).Y + optionsImages[i].Height;
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

            tempAnimation = new List<Animation>();

            for (int i = 0; i < optionsImages.Count; i++)
            {
                dimensions = new Vector2(font.MeasureString(optionsItems[i]).X + optionsImages[i].Width,
                    font.MeasureString(optionsItems[i]).Y + optionsImages[i].Height);

                if (axis == 1)
                    pos.Y = (ScreenManager.Instance.Dimensions.Y) / 2;
                else
                    pos.X = (ScreenManager.Instance.Dimensions.X) / 2 - font.MeasureString(optionsItems[i]).X / 2;

                for (int j = 0; j < animationTypes.Count; j++)
                {
                    switch (animationTypes[j])
                    {
                        case "Fade":
                            tempAnimation.Add(new FadeAnimation());
                            tempAnimation[tempAnimation.Count - 1].LoadContent(content,
                                optionsImages[i], optionsItems[i], pos);
                            tempAnimation[tempAnimation.Count - 1].Font = font;
                            break;
                    }
                }
                if (tempAnimation.Count > 0)
                    animation.Add(tempAnimation);
                tempAnimation = new List<Animation>();

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
            optionsItems = new List<string>();
            optionsImages = new List<Texture2D>();
            animation = new List<List<Animation>>();
            animationTypes = new List<string>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            linkID = new List<string>();
            linkType = new List<string>();
            variable = new List<int>();
            itemNumber = 0;
            optionNumber = 0;

            position = Vector2.Zero;
            limits = Vector2.Zero;
            fileManager = new FileManager();
            fileManager.LoadContent("Load/options.txt", attributes, contents, id);

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Font":
                            font = this.content.Load<SpriteFont>(contents[i][j]);
                            break;
                        case "Item":
                            optionsItems.Add(contents[i][j]);
                            break;
                        case "Image":
                            optionsImages.Add(this.content.Load<Texture2D>(contents[i][j]));
                            break;
                        case "Axis":
                            axis = int.Parse(contents[i][j]);
                            break;
                        case "Position":
                            string[] temp = contents[i][j].Split(' ');
                            position = new Vector2(float.Parse(temp[0]),
                                float.Parse(temp[1]));
                            break;
                        case "Source":
                            temp = contents[i][j].Split(' ');
                            source = new Rectangle(int.Parse(temp[0]),
                                int.Parse(temp[1]), int.Parse(temp[2]),
                                int.Parse(temp[3]));
                            break;
                        case "Animation":
                            animationTypes.Add(contents[i][j]);
                            break;
                        case "Align":
                            align = contents[i][j];
                            break;
                        case "LinkType":
                            linkType.Add(contents[i][j]);
                            break;
                        case "LinkID":
                            linkID.Add(contents[i][j]);
                            break;
                        case "Limits":
                            temp = contents[i][j].Split(',');
                            limits = new Vector2(int.Parse(temp[0]), int.Parse(temp[1]));
                            break;
                        case "Variable":
                            temp = contents[i][j].Split(',');
                            variable.Add(int.Parse(temp[1]));
                            break;
                    }
                }
            }
            SetoptionsItems();
            SetAnimations();
        }

        public void UnloadContent()
        {
            content.Unload();
            position = Vector2.Zero;
            animation.Clear();
            optionsImages.Clear();
            optionsItems.Clear();
            animationTypes.Clear();
        }

        public void Update(GameTime gameTime, InputManager inputManager)
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

            if (itemNumber != null)
            {
                if (linkType[itemNumber] == "Screen")
                {
                    Type newClass = Type.GetType("TM2." + linkID[itemNumber]);
                    ScreenManager.Instance.AddScreen((GameScreen)Activator.CreateInstance(newClass), inputManager);
                }
                else if (linkType[itemNumber] == "Option")
                {
                    SelectMenuItem(itemNumber, inputManager);
                }
            }

            if (itemNumber < 0)
                itemNumber = optionsItems.Count - 1;
            else if (itemNumber > optionsItems.Count - 1)
                itemNumber = 0;

            for (int i = 0; i < animation.Count; i++)
            {
                for (int j = 0; j < animation[i].Count; j++)
                {
                    if (itemNumber == i)
                        animation[i][j].IsActive = true;
                    else
                        animation[i][j].IsActive = false;

                    animation[i][j].Update(gameTime);
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < animation.Count; i++)
            {
                for (int j = 0; j < animation[i].Count; j++)
                {
                    animation[i][j].Draw(spriteBatch);
                }
            }
            spriteBatch.DrawString(font, optionNumber.ToString(), new Vector2(200, 200), Color.Black);
        }

        private void SelectMenuItem(int itemNumber, InputManager inputManager)
        {
            optionNumber = (int)variable[itemNumber];
                if (axis == 2)
                {
                    if (inputManager.KeyPressed(Keys.Right, Keys.D) && limits.Y > optionNumber)
                    {
                        //System.Diagnostics.Debug.WriteLine("Option" + limits.X + "," + limits.Y);
                        variable[itemNumber]++;
                    }
                    else if (inputManager.KeyPressed(Keys.Left, Keys.A) && limits.X < optionNumber)
                        variable[itemNumber]--;
                }
                else
                {
                    if (inputManager.KeyPressed(Keys.Down, Keys.S) && limits.Y > optionNumber)
                        variable[itemNumber]++;
                    else if (inputManager.KeyPressed(Keys.Up, Keys.W) && limits.X < optionNumber)
                        variable[itemNumber]--;
                }
         }
    }
}

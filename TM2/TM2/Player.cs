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
    public class Player : Entity
    {
        SpriteFont font;
        public string collision;

        public override void LoadContent(ContentManager content, InputManager input)
        {
            //The splash screen Text
            font = content.Load<SpriteFont>("SplashScreen/Coolvetica Rg");
            collision = "FALSE";

            base.LoadContent(content, input);
            fileManager = new FileManager();
            moveAnimation = new SpriteSheetAnimation();
            Vector2 tempFrames = Vector2.Zero;
            moveSpeed = 100f;
            gravity = 5f;

            fileManager.LoadContent("Load/Player.txt", attributes, contents);
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Health" :
                            health = int.Parse(contents[i][j]);
                            break;
                        case "Frames" :
                            string[] frames = contents[i][j].Split(' ');
                            tempFrames = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                        case "Image" :
                            image = this.content.Load<Texture2D>(contents[i][j]);
                            break;
                        case "Postion" :
                            frames = contents[i][j].Split(' ');
                            position = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                    }
                }
            }

            moveAnimation.LoadContent(content, image, "", position);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Collision col, Layers layer)
        {
            moveAnimation.IsActive = true;

            if (input.KeyDown(Keys.Right, Keys.D))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                velocity.X += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (input.KeyDown(Keys.Left, Keys.A))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                velocity.X -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (input.KeyDown(Keys.Up, Keys.W))
            {
                if ((float)gameTime.ElapsedGameTime.TotalSeconds < 2.0f)
                    velocity.Y -= moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds - gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (input.KeyDown(Keys.Down, Keys.S))
            {
                velocity.Y += moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (velocity.X > 0)
                velocity.X -= 10;
            else
            {
                moveAnimation.IsActive = false;
                velocity.X = 0;
            }

            for (int i = 0; i < col.CollisionMap.Count; i++)
            {
                for (int j = 0; j < col.CollisionMap[i].Count; j++)
                {
                    if (col.CollisionMap[i][j] == "X")
                    {
                        if (position.X + velocity.X + moveAnimation.FrameWidth < j * layer.TileDimensions.X)
                        {
                            //no collision
                            collision = "FALSE1" + " - " + i + " - " + j + " X : " + position.X + " Y : " + position.Y;
                        }
                        else if (position.X + velocity.X > j * layer.TileDimensions.X + layer.TileDimensions.X + 1)
                        {
                            //no collision
                            velocity.Y += gravity / 100;
                            collision = "FALSE2" + " - " + i + " - " + j + " X : " + position.X + " Y : " + position.Y;
                        }
                        else if (position.Y + velocity.Y + moveAnimation.FrameHeight < i * layer.TileDimensions.Y)
                        {
                            //no collision
                            collision = "FALSE3" + " - " + i + " - " + j + " X : " + position.X + " Y : " + position.Y;
                        }
                        else if (position.Y + velocity.Y > i * layer.TileDimensions.Y + layer.TileDimensions.Y)
                        {
                            //no collision
                            velocity.Y += gravity / 100;
                            collision = "FALSE4" + " - " + i + " - " + j + " X : " + position.X + " Y : " + position.Y;
                        }
                        else
                        {
                            collision = "TRUE";
                            if (velocity.Y >= 0)
                                velocity.Y = 0;
                        }
                    }
                }
            }

            position += velocity;

            moveAnimation.Position = position;
            moveAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
            spriteBatch.DrawString(font, collision, new Vector2(400, 200), Color.Black);
        }
    }
}

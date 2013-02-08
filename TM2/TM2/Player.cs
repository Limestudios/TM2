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
        float jumpSpeed = 1500f;

        public FloatRect Rect
        {
            get { return new FloatRect(position.X, position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight); }
        }
        public override void LoadContent(ContentManager content, InputManager input)
        {
            base.LoadContent(content, input);
            fileManager = new FileManager();
            moveAnimation = new Animation();
            ssAnimation = new SpriteSheetAnimation();
            Vector2 tempFrames = Vector2.Zero;
            moveSpeed = 500f;

            fileManager.LoadContent("Load/Player.txt", attributes, contents);
            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "Health":
                            health = int.Parse(contents[i][j]);
                            break;
                        case "Frames":
                            string[] frames = contents[i][j].Split(' ');
                            tempFrames = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                        case "Image":
                            image = this.content.Load<Texture2D>(contents[i][j]);
                            break;
                        case "Postion":
                            frames = contents[i][j].Split(' ');
                            position = new Vector2(int.Parse(frames[0]), int.Parse(frames[1]));
                            break;
                    }
                }
            }

            gravity = 100f;
            velocity = Vector2.Zero;
            syncTilePosition = false;
            activateGravity = true;

            moveAnimation.Frames = new Vector2(3, 2);
            moveAnimation.LoadContent(content, image, "", position);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Collision col, Layer layer)
        {
            syncTilePosition = false;
            prevPosition = position;
            moveAnimation.IsActive = true;

            if (input.KeyDown(Keys.Right, Keys.D))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (input.KeyDown(Keys.Left, Keys.A))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                moveAnimation.IsActive = false;
                velocity.X = 0;
            }

            if (input.KeyDown(Keys.Up, Keys.W) && !activateGravity)
            {
                velocity.Y = -jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                activateGravity = true;
            }

            if (activateGravity)
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                velocity.Y = 0;

            position += velocity;

            moveAnimation.Position = position;
            ssAnimation.Update(gameTime, ref moveAnimation);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
        }
    }
}
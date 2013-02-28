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
        public override void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            base.LoadContent(content, attributes, contents,input);
            jumpSpeed = 1700f;

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(content.Load<Texture2D>("square2"));

            particleEngine = new ParticleEngine(textures, this.Position);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            moveAnimation.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Map map, Camera camera)
        {
            base.Update(gameTime, input, map, camera);
            this.moveAnimation.IsActive = true;
            this.Bleeding = false;
            this.shaking = false;

            syncTilePosition = true;

            if (input.KeyDown(Keys.Right, Keys.D))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.direction = 0;
            }
            else if (input.KeyDown(Keys.Left, Keys.A))
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.direction = 1;
            }
            else
            {
                velocity.X = 0;
            }


            if (input.KeyDown(Keys.Up, Keys.W) && !activateGravity)
            {
                velocity.Y = -jumpSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                activateGravity = true;
            }

            if (activateGravity && this.velocity.Y > 0)
            {
                moveAnimation.IsActive = true;
                if (this.Direction == 1)
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 2);
                else
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 3);
            }
            else if (activateGravity && this.velocity.Y < 0)
            {
                moveAnimation.IsActive = true;
                if (this.Direction == 1)
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 4);
                else
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 5);
            }
            else if (!input.KeyDown(Keys.Up, Keys.W, Keys.Left, Keys.A, Keys.Right, Keys.D))
            {
                if (this.Direction == 1)
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                else
                    moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);

                moveAnimation.IsActive = false;
            }

            if (ActivateGravity)
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                velocity.Y = 0;

            position += velocity;

            moveAnimation.Position = position;
            ssAnimation.Update(gameTime, ref moveAnimation);

            particleEngine.EmitterLocation = new Vector2(this.Position.X + this.Animation.FrameWidth / 2, this.Position.Y + this.Animation.FrameHeight / 2);
            particleEngine.Update(map, gameTime);
        }

        public override void OnCollision(Entity e)
        {
            Type type = e.GetType();

            if (type == typeof(Enemy))
            {
                health--;
                bleeding = true;
                shaking = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
            if (bleeding == true)
            {
                particleEngine.Blood(spriteBatch);
                particleEngine.isActive = true;
            }
            else
            {
                particleEngine.Blood(spriteBatch);
                particleEngine.isActive = false;
            }
        }
    }
}
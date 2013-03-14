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
    public class Enemy : Entity
    {
        int rangeCounter;

        Random random = new Random();

        TimeSpan previousEnemySoundTime, enemySoundTime;

        public override void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            base.LoadContent(content, attributes, contents, input);
            rangeCounter = 0;
            direction = 1;

            origPosition = position;

            if (direction == 1)
                destPosition.X = origPosition.X + range;
            else
                destPosition.X = origPosition.X - range;

        }

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime, InputManager input, Map map, Camera camera, EntityManager entityManager, SoundEngine soundEngine)
        {
            base.Update(gameTime, input, map, camera, entityManager, soundEngine);
            moveAnimation.IsActive = true;

            syncTilePosition = true;

            if (direction == 1)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 0);
                velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (direction == 2)
            {
                moveAnimation.CurrentFrame = new Vector2(moveAnimation.CurrentFrame.X, 1);
                velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (ActivateGravity)
                velocity.Y += gravity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            else
                velocity.Y = 0;

            position += velocity;

            if (direction == 1 && position.X >= destPosition.X)
            {
                direction = 2;
                destPosition.X = origPosition.X - range;
            }
            else if (direction == 2 && position.X <= destPosition.X)
            {
                direction = 1;
                destPosition.X = origPosition.X + range;
            }

            ssAnimation.Update(gameTime, ref moveAnimation);
            moveAnimation.Position = position;


            if (gameTime.TotalGameTime - previousEnemySoundTime > enemySoundTime)
            {
                soundEngine.PlaySound("zombie moan", this.position);

                // Update the time left next enemy spawn
                previousEnemySoundTime = gameTime.TotalGameTime;
                var soundSeconds = random.Next(5, 8); // random should be a member of the class
                enemySoundTime = TimeSpan.FromSeconds(soundSeconds);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (camMinX <= this.position.X / Layer.Instance.TileDimensions.X && this.position.X / Layer.Instance.TileDimensions.X <= camMaxX &&
                camMinY <= this.position.Y / Layer.Instance.TileDimensions.Y && this.position.Y / Layer.Instance.TileDimensions.Y <= camMaxY)
                moveAnimation.Draw(spriteBatch);
        }
    }
}

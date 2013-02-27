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

        public override void Update(GameTime gameTime, InputManager input, Map map)
        {
            base.Update(gameTime, input, map);
            moveAnimation.IsActive = true;

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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            moveAnimation.Draw(spriteBatch);
        }
    }
}

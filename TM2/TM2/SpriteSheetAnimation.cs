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
    public class SpriteSheetAnimation : Animation
    {
        int frameCounter, switchFrame;
        Vector2 currentFrame;

        public SpriteSheetAnimation()
        {
            frameCounter = 0;
            switchFrame = 100;
        }

        public override void Update(GameTime gameTime, ref Animation a)
        {
            currentFrame = a.CurrentFrame;
            if (a.IsActive)
            {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameCounter >= switchFrame)
                {
                    frameCounter = 0;
                    currentFrame.X++;

                    if (currentFrame.X * a.FrameWidth >= a.Image.Width)
                        currentFrame.X = 0;
                }
            }
            else
            {
                frameCounter = 0;
                //currentFrame.X = 1;
            }
            a.CurrentFrame = currentFrame;
            a.SourceRect = new Rectangle((int)currentFrame.X * a.FrameWidth, (int)currentFrame.Y * a.FrameHeight, a.FrameWidth, a.FrameHeight);
        }
    }
}

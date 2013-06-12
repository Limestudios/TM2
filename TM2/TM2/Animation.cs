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
    public class Animation
    {
        public Texture2D image;
        private string text;
        private SpriteFont font;
        private Color color;
        private Rectangle sourceRect;
        private float rotation, axis;
        private Vector2 origin, position;
        private ContentManager content;
        private bool isActive;
        private float alpha;
        private Vector2 frames, currentFrame, scale;

        public Texture2D Image
        {
            get { return image; }
        }

        public Rectangle SourceRect
        {
            set { sourceRect = value; }
        }

        public Vector2 Frames
        {
            set { frames = value; }
            get { return frames; }
        }

        public Vector2 CurrentFrame
        {
            set { currentFrame = value; }
            get { return currentFrame; }
        }

        public int FrameWidth
        {
            get { return image.Width / (int)frames.X; }
        }

        public int FrameHeight
        {
            get { return image.Height / (int)frames.Y; }
        }

        public virtual float Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public bool IsActive
        {
            set { isActive = value; }
            get { return isActive; }
        }

        public Vector2 Scale
        {
            set { scale = value; }
            get { return scale; }
        }

        public SpriteFont Font
        {
            get { return font; }
            set { font = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public void LoadContent(ContentManager Content, Texture2D image,
            string text, Vector2 position, Color color)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            this.image = image;
            this.text = text;
            this.position = position;
            if (text != String.Empty)
            {
                font = this.content.Load<SpriteFont>("Coolvetica Rg");
                this.color = color;
            }
            rotation = 0.0f;
            axis = 0.0f;
            scale = new Vector2(1, 1);
            alpha = 1.0f;
            isActive = false;

            currentFrame = new Vector2(0, 0);
            if (image != null && frames != Vector2.Zero)
                sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
            else
                sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }


        public void LoadContent(ContentManager Content, Texture2D image,
            string text, Vector2 position, Color color, SpriteFont font)
        {
            content = new ContentManager(Content.ServiceProvider, "Content");
            this.image = image;
            this.text = text;
            this.position = position;
            if (text != String.Empty)
            {
                this.font = font;
                this.color = color;
            }
            rotation = 0.0f;
            axis = 0.0f;
            scale = new Vector2(1, 1);
            alpha = 1.0f;
            isActive = false;

            currentFrame = new Vector2(0, 0);
            if (image != null && frames != Vector2.Zero)
                sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight, FrameWidth, FrameHeight);
            else
                sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }

        public void UnloadContent()
        {
            //have to watch this for memory usage (might have to fix later)
            content.Unload();
            text = String.Empty;
            position = Vector2.Zero;
            sourceRect = Rectangle.Empty;
            image = null;
        }

        public virtual void Update(GameTime gameTime, ref Animation a)
        {

        }

        //not sure if I will keep this virtual
        public void Draw(SpriteBatch spriteBatch)
        {
            if (image != null)
            {
                origin = new Vector2(sourceRect.Width / 2,
                    sourceRect.Height / 2);

                spriteBatch.Draw(image,
                    position + origin,
                    sourceRect,
                    Color.White * alpha,
                    rotation,
                    origin,
                    scale,
                    SpriteEffects.None,
                    0.0f);
            }

            if (text != String.Empty)
            {
                origin = new Vector2(font.MeasureString(text).X / 2,
                    font.MeasureString(text).Y / 2);

                spriteBatch.DrawString(font,
                    text, 
                    position + origin,
                    color * alpha,
                    rotation,
                    origin,
                    scale, 
                    SpriteEffects.None,
                    0.0f);
            }
        }
    }
}

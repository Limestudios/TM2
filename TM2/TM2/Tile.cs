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
    public class Tile
    {
        Layer layer = new Layer();

        public enum State { Solid, Passive, Platform};
        public enum Motion { Static, Horizontal, Vertical };

        State state;
        Motion motion;
        Vector2 position, prevPosition, velocity;
        Texture2D tileImage;
        
        bool increase;
        float range;
        int counter;
        float moveSpeed;
        bool containsEntity;

        Animation animation;

        public Vector2 Position
        {
            get { return position;}
        }

        private Texture2D CropImage(Texture2D tileSheet, Rectangle tileArea)
        {
            Texture2D croppedImage = new Texture2D(tileSheet.GraphicsDevice, tileArea.Width, tileArea.Height);

            Color[] tileSheetData = new Color[tileSheet.Width * tileSheet.Height];
            Color[] croppedImageData = new Color[croppedImage.Width * croppedImage.Height];

            tileSheet.GetData<Color>(tileSheetData);

            int index = 0;
            for (int i = tileArea.Y; i < tileArea.Y + tileArea.Height; i++)
            {
                for (int j = tileArea.X; j < tileArea.X + tileArea.Width; j++)
                {
                    croppedImageData[index] = tileSheetData[i * tileSheet.Width + j];
                    index++;
                }
            }
            croppedImage.SetData<Color>(croppedImageData);

            return croppedImage;
        }

        public void SetTile(State state, Motion motion, Vector2 position, Texture2D tileSheet, Rectangle tileArea)
        {
            this.state = state;
            this.motion = motion;
            this.position = position;
            increase = true;

            tileImage = CropImage(tileSheet, tileArea);
            range = 90;
            counter = 0;
            moveSpeed = 80f;
            animation = new Animation();
            animation.LoadContent(ScreenManager.Instance.Content, tileImage, "", position);
            containsEntity = false;
            velocity = Vector2.Zero;
        }

        public void SetTile(State state, Motion motion, Vector2 position, Texture2D tileSheet, Rectangle tileArea, string text)
        {
            this.state = state;
            this.motion = motion;
            this.position = position;
            increase = true;

            tileImage = CropImage(tileSheet, tileArea);
            range = 90;
            counter = 0;
            moveSpeed = 80f;
            animation = new Animation();
            animation.LoadContent(ScreenManager.Instance.Content, tileImage, text, position);
            containsEntity = false;
            velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime)
        {
            //should make this based on timespan...for now this will work tho (bad coding habits)
            counter++;
            prevPosition = position;

            if (counter >= range)
            {
                counter = 0;
                increase = !increase;
            }

            if (motion == Motion.Horizontal)
            {
                if (increase)
                    velocity.X = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.X = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (motion == Motion.Vertical)
            {
                if (increase)
                    velocity.Y = moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                    velocity.Y = -moveSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            position += velocity;
            this.animation.Position = this.position;
        }

        public void UpdateCollision(ref Entity entity, InputManager inputManager, SoundEngine soundEngine)
        {
            FloatRect rect = new FloatRect(position.X, position.Y, layer.TileDimensions.X, layer.TileDimensions.Y);

            if (entity.OnTile && containsEntity)
            {
                if (!entity.SyncTilePosition)
                {
                    entity.Position += this.velocity;
                    entity.SyncTilePosition = true;
                }

                if (entity.Rect.Right < rect.Left || entity.Rect.Left > rect.Right || entity.Rect.Bottom != rect.Top)
                {
                    containsEntity = false;
                    entity.ActivateGravity = true;
                    entity.CanJump = false;
                }
            }

            if (entity.Rect.Intersects(rect) && state == State.Solid)
            {
                FloatRect preventity = new FloatRect(entity.PrevPosition.X, entity.PrevPosition.Y, entity.Animation.FrameWidth, entity.Animation.FrameHeight);

                FloatRect prevTile = new FloatRect(this.prevPosition.X, this.prevPosition.Y, layer.TileDimensions.X, layer.TileDimensions.Y);

                if (entity.Rect.Bottom >= rect.Top && preventity.Bottom <= prevTile.Top)
                {
                    //bottom collision
                    entity.Position = new Vector2(entity.Position.X, position.Y - entity.Animation.FrameHeight);
                    entity.ActivateGravity = false;
                    entity.OnTile = true;
                    containsEntity = true;
                }
                else if (entity.Rect.Top <= rect.Bottom && preventity.Top >= prevTile.Bottom)
                {
                    //top collision
                    entity.Position = new Vector2(entity.Position.X, position.Y + layer.TileDimensions.Y);
                    entity.ActivateGravity = true;
                }
                else if (entity.Rect.Right >= rect.Left && preventity.Right <= prevTile.Left)
                {
                    entity.Position = new Vector2(position.X - entity.Animation.FrameWidth, entity.Position.Y);
                    entity.Direction = (entity.Direction == 1) ? entity.Direction = 2 : entity.Direction = 1;
                    entity.CanJump = true;
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                }
                else if (entity.Rect.Left <= rect.Right && preventity.Left >= prevTile.Left)
                {
                    entity.Position = new Vector2(position.X + layer.TileDimensions.X, entity.Position.Y);
                    entity.Direction = (entity.Direction == 1) ? entity.Direction = 2 : entity.Direction = 1;
                    entity.CanJump = true;
                    entity.Velocity = new Vector2(0, entity.Velocity.Y);
                }
            }
            else if (entity.Rect.Intersects(rect) && state == State.Platform)
            {
                FloatRect preventity = new FloatRect(entity.PrevPosition.X, entity.PrevPosition.Y, entity.Animation.FrameWidth, entity.Animation.FrameHeight);

                FloatRect prevTile = new FloatRect(this.prevPosition.X, this.prevPosition.Y, layer.TileDimensions.X, layer.TileDimensions.Y);

                if (entity.Rect.Bottom >= rect.Top && preventity.Bottom <= prevTile.Top && inputManager.KeyDown(Keys.Down))
                {
                    //bottom collision
                    //entity.Position = new Vector2(entity.Position.X, position.Y - entity.Animation.FrameHeight);
                    entity.ActivateGravity = true;
                    entity.OnTile = false;
                    containsEntity = false;
                }
                else if (entity.Rect.Bottom >= rect.Top && preventity.Bottom <= prevTile.Top)
                {
                    //bottom collision
                    entity.Position = new Vector2(entity.Position.X, position.Y - entity.Animation.FrameHeight);
                    entity.ActivateGravity = false;
                    entity.OnTile = true;
                    containsEntity = true;
                }
            }

            entity.Animation.Position = entity.Position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}

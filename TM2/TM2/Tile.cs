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

        public enum State { Solid, Passive };
        public enum Motion { Static, Horizontal, Vertical };

        State state;
        Motion motion;
        Vector2 position, prevPosition, velocity;
        Texture2D tileImage;
        
        bool increase;
        float range;
        int counter;
        float moveSpeed;
        bool onTile;

        Animation animation;

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
            range = 50;
            counter = 0;
            moveSpeed = 100f;
            animation = new Animation();
            animation.LoadContent(ScreenManager.Instance.Content, tileImage, "", position);
            onTile = false;
            velocity = Vector2.Zero;
        }

        public void Update(GameTime gameTime, ref Player player)
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
            animation.Position = position;

            FloatRect rect = new FloatRect(position.X, position.Y, layer.TileDimensions.X, layer.TileDimensions.Y);

            if (onTile)
            {
                if (!player.SyncTilePosition)
                {
                    player.Position += velocity;
                    player.SyncTilePosition = true;
                }

                if (player.Rect.Right < rect.Left || player.Rect.Left > rect.Right || player.Rect.Bottom != rect.Top)
                {
                    onTile = false;
                    player.ActivateGravity = true;
                }
            }

            if (player.Rect.Intersects(rect) && state == State.Solid)
            {
                FloatRect prevPlayer = new FloatRect(player.PrevPosition.X, player.PrevPosition.Y, player.Animation.FrameWidth, player.Animation.FrameHeight);

                FloatRect prevTile = new FloatRect(prevPosition.X, prevPosition.Y, layer.TileDimensions.X, layer.TileDimensions.Y);

                if (player.Rect.Bottom >= rect.Top && prevPlayer.Bottom <= prevTile.Top)
                {
                    //bottom collision
                    player.Position = new Vector2(player.Position.X, position.Y - player.Animation.FrameHeight);
                    player.ActivateGravity = false;
                    onTile = true;
                }
                else if (player.Rect.Top >= rect.Bottom && prevPlayer.Top <= prevTile.Bottom)
                {
                    //top collision
                    player.Position = new Vector2(player.Position.X, position.Y + layer.TileDimensions.Y);
                    player.Velocity = new Vector2(player.Velocity.X, 0);
                    player.ActivateGravity = true;
                }
                else
                {
                    player.Position -= player.Velocity;
                }
            }

            player.Animation.Position = player.Position;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}

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
    public class Particle
    {
        public Texture2D Texture { get; set; }        // The texture that will be drawn to represent the particle
        public Vector2 Position { get; set; }        // The current position of the particle        
        public Vector2 Velocity { get; set; }        // The speed of the particle at the current instance
        public float Angle { get; set; }            // The current angle of rotation of the particle
        public float AngularVelocity { get; set; }    // The speed that the angle is changing
        public Color Color { get; set; }            // The color of the particle
        public float Size { get; set; }                // The size of the particle
        public int TTL { get; set; }                // The 'time to live' of the particle
        public float Gravity { get; set; }                // The gravity acting on the particle

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl, float gravity)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
            Gravity = gravity;
        }

        public void Update(Collision col, GameTime gameTime)
        {
            TTL--;
            Angle += 0;
            Position += new Vector2(Velocity.X, Velocity.Y + 0.2f * (float)gameTime.ElapsedGameTime.TotalMilliseconds);

            for (int i = 0; i < col.CollisionMap.Count; i++)
            {
                for (int j = 0; j < col.CollisionMap[i].Count; j++)
                {
                    if (col.CollisionMap[i][j] == "X")
                    {
                        if (Position.X + Velocity.X + Size * 32 <= j * 64)
                        {
                            //no collision
                        }
                        else if (Position.X + Velocity.X + Size * 32 >= j * 64 + 64)
                        {
                            //no collision
                        }
                        else if (Position.Y + Velocity.Y + Size * 32 <= i * 64)
                        {
                            //no collision
                        }
                        else if (Position.Y + Velocity.Y + Size * 32 >= i * 64 + 64)
                        {
                            //no collision
                        }
                        else
                        {
                            TTL = 0;
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteBatch.Draw(Texture, Position, sourceRectangle, Color,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}

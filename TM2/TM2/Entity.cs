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
    public class Entity
    {
        protected ParticleEngine particleEngine;

        protected int health;
        protected List<int> healths;

        protected Texture2D image;
        protected List<Texture2D> images;

        protected Vector2 frames;
        protected List<Vector2> framesList;

        protected float moveSpeed;
        protected List<float> moveSpeeds;

        protected Animation moveAnimation;
        protected SpriteSheetAnimation ssAnimation;
        protected FadeAnimation fAnimation;
        protected float jumpSpeed, gravity;

        protected ContentManager content;

        protected List<string> attributes, contents;

        protected Vector2 position, velocity, prevPosition, destPosition, origPosition;

        protected bool activateGravity;
        protected bool syncTilePosition;
        protected bool onTile, bleeding;
        protected int range;
        protected int direction;

        public bool Bleeding
        {
            set { bleeding = value; }
        }

        public int Direction
        {
            get { return direction; }
            set 
            { 
                direction = value; 
                destPosition.X = (direction == 2) ? destPosition.X = origPosition.X - range :
                    destPosition.X = origPosition.X + range;
            }
        }

        public bool OnTile
        {
            get { return onTile; }
            set { onTile = value; }
        }

        public Vector2 PrevPosition
        {
            get { return prevPosition; }
        }

        public Animation Animation
        {
            get { return moveAnimation; }
        }

        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Collision rectangle (Rect) for player.
        /// </summary>
        public FloatRect Rect
        {
            get { return new FloatRect(position.X, position.Y, moveAnimation.FrameWidth, moveAnimation.FrameHeight); }
        }

        public bool ActivateGravity
        {
            set { activateGravity = value; }
        }

        public bool SyncTilePosition
        {
            get { return syncTilePosition; }
            set { syncTilePosition = value; }
        }

        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public Vector2 Frames
        {
            get { return frames; }
            set { frames = value; }
        }

        public float MoveSpeed
        {
            get { return moveSpeed; }
            set { moveSpeed = value; }
        }

        public virtual void LoadContent(ContentManager content, List<string> attributes, List<string> contents, InputManager input)
        {
            this.content = new ContentManager(content.ServiceProvider, "Content");

            moveAnimation = new Animation();
            ssAnimation = new SpriteSheetAnimation();
            fAnimation = new FadeAnimation();
            images = new List<Texture2D>();
            healths = new List<int>();
            framesList = new List<Vector2>();
            moveSpeeds = new List<float>();

            for (int i = 0; i < attributes.Count; i++)
            {
                switch (attributes[i])
                {
                    case "Image":
                        Texture2D tempImage = this.content.Load<Texture2D>(contents[i]);
                        string[] name = contents[i].Split('/');
                        tempImage.Name = name[name.Count() - 1];
                        image = tempImage;
                        break;
                    case "Frames":
                        string[] framesTemp = contents[i].Split(',');
                        moveAnimation.Frames = new Vector2(int.Parse(framesTemp[0]), int.Parse(framesTemp[1]));
                        break;
                    case "Position":
                        string[] pos = contents[i].Split(',');
                        position = new Vector2(int.Parse(pos[0]), int.Parse(pos[1]));
                        break;
                    case "Health":
                        health = int.Parse(contents[i]);
                        break;
                    case "MoveSpeed" :
                        moveSpeed = float.Parse(contents[i]);
                        break;
                    case "Range":
                        range = int.Parse(contents[i]);
                        break;
                }
            }

            gravity = 100f;
            velocity = Vector2.Zero;
            syncTilePosition = false;
            activateGravity = true;
            moveAnimation.LoadContent(content, image, "", position);
        }

        public virtual void UnloadContent()
        {
            content.Unload();
        }

        public virtual void Update(GameTime gameTime, InputManager input)
        {
            syncTilePosition = false;
            prevPosition = position;
        }

        public virtual void OnCollision(Entity e)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
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
    public class Camera
    {
        private float zoom;
        public Matrix transform;
        public Vector2 currentPosition;
        public Vector2 targetPosition;
        public Vector3 cameraPosition;
        private float rotation;
        private float halfViewportWidth;
        private float halfViewportHeight;

        public Vector2 CurrentPosision
        {
            get { return currentPosition; }
        }

        public float HalfViewportWidth
        {
            get { return halfViewportWidth; }
        }

        public float HalfViewportHeight
        {
            get { return halfViewportHeight; }
        }

        // We only need one Random object no matter how many Cameras we have
        private static readonly Random random = new Random();

        // Are we shaking?
        private bool shaking;

        // The maximum magnitude of our shake offset
        private float shakeMagnitude;

        // The total duration of the current shake
        private float shakeDuration;

        // A timer that determines how far into our shake we are
        private float shakeTimer;

        // The shake offset vector
        private Vector2 shakeOffset;

        public Camera(GraphicsDevice graphicsDevice)
        {
            zoom = 1.0f;
            rotation = 0.0f;
            currentPosition = Vector2.Zero;
            targetPosition = currentPosition;

            halfViewportHeight = new float();
            halfViewportWidth = new float();

            halfViewportWidth = ScreenManager.Instance.Dimensions.X / 2.0f;
            halfViewportHeight = ScreenManager.Instance.Dimensions.Y / 2.0f;

        }

        public float Zoom
        {
            get { return zoom; }
            set { zoom = value; if (zoom < 0.1f) zoom = 0.1f; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public void SetPosition(float newX, float newY, bool forceNow)
        {
            targetPosition.X = newX;
            targetPosition.Y = newY;

            if (forceNow == true)
            {
                currentPosition = targetPosition;
            }
        }

        public void Update(float delta, GameTime gameTime)
        {
            // If we're shaking...
            if (shaking)
            {
                // Move our timer ahead based on the elapsed time
                shakeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                // If we're at the max duration, we're not going to be shaking anymore
                if (shakeTimer >= shakeDuration)
                {
                    shaking = false;
                    shakeTimer = shakeDuration;
                }

                // Compute our progress in a [0, 1] range
                float progress = shakeTimer / shakeDuration;

                // Compute our magnitude based on our maximum value and our progress. This causes
                // the shake to reduce in magnitude as time moves on, giving us a smooth transition
                // back to being stationary. We use progress * progress to have a non-linear fall 
                // off of our magnitude. We could switch that with just progress if we want a linear 
                // fall off.
                float magnitude = shakeMagnitude * (1f - (progress * progress));

                // Generate a new offset vector with three random values and our magnitude
                shakeOffset = new Vector2(NextFloat(), NextFloat()) * magnitude;

                // If we're shaking, add our offset to our position and target
                currentPosition += shakeOffset;
                targetPosition += shakeOffset;
            }

            Int32 mapWidth, mapHeight;

            currentPosition.X = MathHelper.SmoothStep(currentPosition.X, MathHelper.Clamp(targetPosition.X, 0 + halfViewportWidth, Layer.Instance.MapDimensions.X * Layer.Instance.TileDimensions.X - halfViewportWidth), delta * 9.0f);
            currentPosition.Y = MathHelper.SmoothStep(currentPosition.Y, MathHelper.Clamp(targetPosition.Y, 0 - halfViewportHeight, Layer.Instance.MapDimensions.Y * Layer.Instance.TileDimensions.Y - halfViewportHeight), delta * 9.0f);

            mapWidth = (int)ScreenManager.Instance.Dimensions.X * 10;
            mapHeight = (int)ScreenManager.Instance.Dimensions.Y * 3;

            if (currentPosition.X < (int)ScreenManager.Instance.Dimensions.X / 2) currentPosition.X = (int)ScreenManager.Instance.Dimensions.X / 2;
            if (currentPosition.Y < (int)ScreenManager.Instance.Dimensions.Y / 2) currentPosition.Y = (int)ScreenManager.Instance.Dimensions.Y / 2;
            if (currentPosition.X > mapWidth) currentPosition.X = mapWidth;
            if (currentPosition.Y > mapHeight) currentPosition.Y = mapHeight;

        }

        public Matrix GetTransformation(GraphicsDevice graphicsDevice)
        {
            Int32 xpos = (Int32)(-(currentPosition.X));
            Int32 ypos = (Int32)(-(currentPosition.Y));

            transform = Matrix.CreateTranslation(new Vector3(xpos, ypos, 0)) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateScale(new Vector3(Zoom, Zoom, 0)) *
                        Matrix.CreateTranslation(new Vector3(halfViewportWidth, halfViewportHeight, 0));

            return transform;
        }

        public bool IsPointOnscreen(Vector2 pos)
        {
            if (pos.X < (currentPosition.X - halfViewportWidth)) return false;
            if (pos.Y < (currentPosition.Y - halfViewportHeight)) return false;
            if (pos.X > (currentPosition.X + halfViewportWidth)) return false;
            if (pos.Y > (currentPosition.Y + halfViewportHeight)) return false;

            return true;
        }

        /// <summary>
        /// Helper to generate a random float in the range of [-1, 1].
        /// </summary>
        private float NextFloat()
        {
            return (float)random.NextDouble() * 2f - 1f;
        }

        /// <summary>
        /// Shakes the camera with a specific magnitude and duration.
        /// </summary>
        /// <param name="magnitude">The largest magnitude to apply to the shake.</param>
        /// <param name="duration">The length of time (in seconds) for which the shake should occur.</param>
        public void Shake(float magnitude, float duration)
        {
            // We're now shaking
            shaking = true;

            // Store our magnitude and duration
            shakeMagnitude = magnitude;
            shakeDuration = duration;

            // Reset our timer
            shakeTimer = 0f;
        }
    }
}

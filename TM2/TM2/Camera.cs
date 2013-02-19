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
        private float rotation;
        private Int32 viewportWidth = new Int32();
        private Int32 viewportHeight;
        private float halfViewportWidth;
        private float halfViewportHeight;

        public Camera(GraphicsDevice graphicsDevice)
        {
            zoom = 1.0f;
            rotation = 0.0f;
            currentPosition = Vector2.Zero;
            targetPosition = currentPosition;

            halfViewportHeight = new float();
            halfViewportWidth = new float();

            halfViewportWidth = 1280 / 2.0f;
            halfViewportHeight = 720 / 2.0f;

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

        public void Update(float delta)
        {
            Int32 mapWidth, mapHeight;

            currentPosition.X = MathHelper.SmoothStep(currentPosition.X, MathHelper.Clamp(targetPosition.X, 0 + halfViewportWidth, Layer.Instance.MapDimensions.X * Layer.Instance.TileDimensions.X - halfViewportWidth), delta * 9.0f);
            currentPosition.Y = MathHelper.SmoothStep(currentPosition.Y, MathHelper.Clamp(targetPosition.Y, 0 - halfViewportHeight, Layer.Instance.MapDimensions.Y * Layer.Instance.TileDimensions.Y - halfViewportHeight), delta * 9.0f);

            mapWidth = 1280 * 10;
            mapHeight = 720 * 3;

            if (currentPosition.X < 640) currentPosition.X = 640;
            if (currentPosition.Y < 360) currentPosition.Y = 360;
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
    }
}

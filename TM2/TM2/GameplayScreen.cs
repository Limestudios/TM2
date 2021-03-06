﻿using System;
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

using TM2.Helpers;

namespace TM2
{
    public class GameplayScreen : GameScreen
    {
        EntityManager player, enemies;
        Map map;
        AudioManager audio;
        GUIManager guiManager;
        Texture2D highlight, pixel;
        int playerIndex;
        float zoom, frameRate;
        string frameRateAcceptable;
        SpriteFont font;
        protected Camera camera;
        public SoundEngine soundEngine { get; set; }

        /*
        Random random = new Random();

        TimeSpan previousEnemySoundTime, enemySoundTime;
         */

        public override void LoadContent(ContentManager content, InputManager input)
        {
            //make 1x1 pixel dummy texture
            pixel = content.Load<Texture2D>("fade");
            //pixel.SetData(new[] { Color.White });

            camera = new Camera(ScreenManager.Instance.graphicsDevice);
            //viewport = new Viewport(0, 0, (int)ScreenManager.Instance.Dimensions.X, (int)ScreenManager.Instance.Dimensions.Y);

            playerIndex = 0;
            zoom = 1.0f;

            font = content.Load<SpriteFont>("Coolvetica Rg 12");
            base.LoadContent(content, input);

            audio = new AudioManager();
            audio.LoadContent(content, "Map1");
            audio.MusicVolume = 0.5f;
            //audio.PlaySong(0, true);

            map = new Map();
            map.LoadContent(content, map, "Map1");

            player = new EntityManager();
            player.LoadContent("Player", content, "Load/Player.txt", "", input);
            //player.SetPlayer(playerIndex);

            enemies = new EntityManager();
            enemies.LoadContent("Enemy", content, "Load/Enemy.txt", "Level1", input);

            guiManager = new GUIManager();
            guiManager.LoadContent(content, "Map1");

            highlight = content.Load<Texture2D>("highlight");

            soundEngine = new SoundEngine();
            soundEngine.LoadContent(content);
        }

        public override void UnloadContent()
        {
            //base.UnloadContent();
            player.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            map.Update(gameTime, camera, map);
            player.Entities[playerIndex].Update(gameTime, inputManager, map, camera, enemies, soundEngine);

            for (int i = 0; i < enemies.Entities.Count(); i++)
            {
                enemies.Entities[i].Update(gameTime, inputManager, map, camera, enemies, soundEngine);
            }

            Entity entity;

            for (int i = 0; i < player.Entities.Count; i++)
            {
                entity = this.player.Entities[i];
                map.UpdateCollision(ref entity, inputManager, soundEngine);
                this.player.Entities[i] = entity;
            }

            for (int i = 0; i < enemies.Entities.Count; i++)
            {
                entity = this.enemies.Entities[i];
                map.UpdateCollision(ref entity, inputManager, soundEngine);
                this.enemies.Entities[i] = entity;
            }

            guiManager.Update(gameTime);

            if (inputManager.KeyPressed(Keys.Back))
            {
                //probably make it so the screenmanager handles this but for now...
                ScreenManager.Instance.AddScreen(new TitleScreen(), inputManager);
            }

            if (inputManager.KeyPressed(Keys.OemMinus) && playerIndex > 0)
            {
                Vector2 tempPos = this.player.Entities[playerIndex].Position;
                playerIndex--;
                this.player.Entities[playerIndex].Position = tempPos;
                entity = this.player.Entities[playerIndex];
            }

            if (inputManager.KeyPressed(Keys.OemPlus) && playerIndex < player.Entities.Count - 1)
            {
                Vector2 tempPos = new Vector2(this.player.Entities[playerIndex].Position.X, this.player.Entities[playerIndex].Position.Y);
                playerIndex++;
                this.player.Entities[playerIndex].Position = tempPos;
                entity = this.player.Entities[playerIndex];
            }

            player.EntityCollision(enemies, soundEngine);
            camera.Update((float)(gameTime.ElapsedGameTime.TotalSeconds), gameTime);
            camera.SetPosition(player.Entities[playerIndex].Position.X, player.Entities[playerIndex].Position.Y, false);
            if (player.Entities[playerIndex].Shaking)
            {
                camera.Shake(5f, 1f);
            }

            if (inputManager.KeyDown(Keys.T))
            {
                camera.Shake(25f, 1f);
            }

            if (inputManager.KeyPressed(Keys.L))
            {
                map.LoadContent(content, map, "Map1");
            }

            if (inputManager.KeyPressed(Keys.F))
            {
            }

            //update FrameRate
            frameRate = (float)Math.Floor(1 / (double)gameTime.ElapsedGameTime.TotalSeconds);

            if (frameRate < 59f)
            {
                frameRateAcceptable = "FALSE";
            }
            else
            {
                frameRateAcceptable = "TRUE";
            }

            soundEngine.Update(gameTime, player.Entities[playerIndex]);


            if (player.Entities[playerIndex].Health <= 0)
            {
                Type newClass = Type.GetType("TM2.TitleScreen");
                ScreenManager.Instance.AddScreen((GameScreen)Activator.CreateInstance(newClass), inputManager);
                audio.FadeSong(0.0f, new TimeSpan(0, 0, 0, 500));
            }

            camera.Zoom = ScreenManager.Instance.ScreenScale.X;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation(ScreenManager.Instance.graphicsDevice));
            base.Draw(spriteBatch);
            map.Draw(spriteBatch);
            enemies.Draw(spriteBatch);
            player.Entities[playerIndex].Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            guiManager.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Use + / - keys to switch between characters" + "    Audio Info: " + audio.songs[0].Duration.TotalMinutes + " [" + audio.songs[0].PlayCount + "]", new Vector2(126, 10) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Use the T key to make the camera shake!", new Vector2(126, 30) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "FPS: " + frameRate.ToString() + " - " + frameRateAcceptable, new Vector2(126, 50) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);

            int minX = (int)(Math.Round((double)((camera.CurrentPosision.X - camera.HalfViewportWidth) / map.layer.TileDimensions.X)));
            int maxX = (int)(Math.Round((double)((camera.CurrentPosision.X + camera.HalfViewportWidth) / map.layer.TileDimensions.X)));

            int minY = (int)(Math.Round((double)((camera.CurrentPosision.Y - camera.HalfViewportHeight) / map.layer.TileDimensions.Y)));
            int maxY = (int)(Math.Round((double)((camera.CurrentPosision.Y + camera.HalfViewportHeight) / map.layer.TileDimensions.Y)));

            spriteBatch.DrawString(font, "Camera :   X: " + new Vector2(minX, maxX) + " Y: " + new Vector2(minY, maxY), new Vector2(126, 80) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);

            spriteBatch.Draw(pixel, new Rectangle((int)(960 * ScreenManager.Instance.ScreenScale.X), (int)(4 * ScreenManager.Instance.ScreenScale.Y), (int)((100 + this.player.Entities[playerIndex].Image.Width) * ScreenManager.Instance.ScreenScale.X), (int)((266 + this.player.Entities[playerIndex].Image.Height) * ScreenManager.Instance.ScreenScale.Y)), Color.DarkGray * 0.2f);
            spriteBatch.DrawString(font, "PlayerInfo :   " + this.player.Entities[playerIndex].Image.Name, new Vector2(966, 10) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Health :        " + this.player.Entities[playerIndex].Health.ToString(), new Vector2(966, 40) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Image :", new Vector2(966, 70) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.Draw(this.player.Entities[playerIndex].Image, new Rectangle((int)(1052 * ScreenManager.Instance.ScreenScale.X), (int)(80 * ScreenManager.Instance.ScreenScale.Y), (int)(this.player.Entities[playerIndex].Image.Width * ScreenManager.Instance.ScreenScale.X), (int)(this.player.Entities[playerIndex].Image.Height * ScreenManager.Instance.ScreenScale.Y)), Color.White * 0.5f);
            spriteBatch.DrawString(font, "Size : " + this.player.Entities[playerIndex].Image.Width + "px by " + this.player.Entities[playerIndex].Image.Height + "px", new Vector2(1052, 80 + this.player.Entities[playerIndex].Image.Height) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Position :     X : " + Math.Floor((this.player.Entities[playerIndex].Position.X) / 64), new Vector2(966, 110 + this.player.Entities[playerIndex].Image.Height) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Y : " + Math.Floor((this.player.Entities[playerIndex].Position.Y) / 64), new Vector2(1052, 130 + this.player.Entities[playerIndex].Image.Height) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Frames :       Amount : " + this.player.Entities[playerIndex].Animation.Frames.X + " by " + this.player.Entities[playerIndex].Animation.Frames.Y, new Vector2(966, 160 + this.player.Entities[playerIndex].Image.Height) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Size :  " + this.player.Entities[playerIndex].Animation.FrameWidth + "px by " + this.player.Entities[playerIndex].Animation.FrameHeight + "px", new Vector2(1052, 180 + this.player.Entities[playerIndex].Image.Height) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "MoveSpeed :  " + this.player.Entities[playerIndex].MoveSpeed, new Vector2(1052, 200 + this.player.Entities[playerIndex].Image.Height) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(font, "Vel: " + this.player.Entities[playerIndex].Velocity, new Vector2(1052, 230 + this.player.Entities[playerIndex].Image.Height) * ScreenManager.Instance.ScreenScale, Color.Black, 0f, Vector2.Zero, ScreenManager.Instance.ScreenScale, SpriteEffects.None, 1f);

            int frameX = (int)this.player.Entities[playerIndex].Animation.CurrentFrame.X * this.player.Entities[playerIndex].Animation.FrameWidth;
            int frameY = (int)this.player.Entities[playerIndex].Animation.CurrentFrame.Y * this.player.Entities[playerIndex].Animation.FrameHeight;
            spriteBatch.Draw(highlight, new Rectangle((int)((frameX + 1052)* ScreenManager.Instance.ScreenScale.X), (int)((frameY + 80) * ScreenManager.Instance.ScreenScale.Y), (int)(this.player.Entities[playerIndex].Animation.FrameWidth * ScreenManager.Instance.ScreenScale.X), (int)(this.player.Entities[playerIndex].Animation.FrameHeight * ScreenManager.Instance.ScreenScale.Y)), Color.White);
            spriteBatch.End();
        }
    }
}

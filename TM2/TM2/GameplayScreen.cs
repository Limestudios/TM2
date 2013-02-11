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

namespace TM2
{
    public class GameplayScreen : GameScreen
    {
        EntityManager player, enemies;
        Map map;
        AudioManager audio;
        Camera camera;
        Vector2 parallax;
        Viewport viewport;
        GUIManager guiManager;
        private Texture2D highlight;
        int playerIndex;
        float zoom;

        SpriteFont font;

        public override void LoadContent(ContentManager content, InputManager input)
        {
            viewport = new Viewport(0, 0, (int)ScreenManager.Instance.Dimensions.X, (int)ScreenManager.Instance.Dimensions.Y);
            camera = new Camera(viewport);

            playerIndex = 0;
            zoom = 1.0f;

            font = content.Load<SpriteFont>("Coolvetica Rg 12");
            base.LoadContent(content, input);

            audio = new AudioManager();
            audio.LoadContent(content, "Map1");
            audio.PlaySong(0, true);

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
        }

        public override void UnloadContent()
        {
            base.UnloadContent();
            player.UnloadContent();
            map.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            inputManager.Update();
            player.Entities[playerIndex].Update(gameTime, inputManager);
            enemies.Update(gameTime, map);
            map.Update(gameTime);
            //camera.LookAt(new Vector2(player.Entities[playerIndex].Position.X, player.Entities[playerIndex].Position.Y));
            camera.LookAt(new Vector2(64 * 27 / 2, 64 * 13 / 2));

            Entity entity;

            for (int i = 0; i < player.Entities.Count; i++)
            {
                entity = this.player.Entities[playerIndex];
                map.UpdateCollision(ref entity, inputManager);
                this.player.Entities[playerIndex] = entity;
            }


            for (int i = 0; i < enemies.Entities.Count; i++)
            {
                entity = this.enemies.Entities[i];
                map.UpdateCollision(ref entity, inputManager);
                this.enemies.Entities[i] = entity;
            }

            guiManager.Update(gameTime);

            if (inputManager.KeyPressed(Keys.R))
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

            if (inputManager.KeyPressed(Keys.OemOpenBrackets) && zoom > 0.1f)
            {
                if (zoom <= 1.0f)
                    zoom = zoom - 0.1f;
                else
                    zoom--;
                camera.Zoom = zoom;
            }

            if (inputManager.KeyPressed(Keys.OemCloseBrackets) && camera.Zoom < 10.0f)
            {
                if (zoom < 1.0f)
                    zoom = zoom + 0.1f;
                else
                    zoom++;
                camera.Zoom = zoom;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 parallax = new Vector2(1.0f);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetViewMatrix(parallax));
            base.Draw(spriteBatch);
            map.Draw(spriteBatch);
            this.player.Entities[playerIndex].Draw(spriteBatch);
            enemies.Draw(spriteBatch);
            spriteBatch.End();
            spriteBatch.Begin();
            guiManager.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Use + / - keys to switch between characters", new Vector2(126, 10), Color.Black);
            spriteBatch.DrawString(font, "Use [ / ] keys to zoom the camera in and out    Zoom : " + zoom, new Vector2(126, 30), Color.Black);
            spriteBatch.DrawString(font, "PlayerInfo :   " + this.player.Entities[playerIndex].Image.Name , new Vector2(966, 10), Color.Black);
            spriteBatch.DrawString(font, "Health :        " + this.player.Entities[playerIndex].Health.ToString(), new Vector2(966, 40), Color.Black);
            spriteBatch.DrawString(font, "Image :", new Vector2(966, 70), Color.Black);
            spriteBatch.Draw(this.player.Entities[playerIndex].Image, new Rectangle(1052, 80, this.player.Entities[playerIndex].Image.Width, this.player.Entities[playerIndex].Image.Height), Color.White);
            spriteBatch.DrawString(font, "Size : " + this.player.Entities[playerIndex].Image.Width + "px by " + this.player.Entities[playerIndex].Image.Height + "px", new Vector2(1052, 80 + this.player.Entities[playerIndex].Image.Height), Color.Black);
            spriteBatch.DrawString(font, "Position :     X : " + Math.Floor(this.player.Entities[playerIndex].Position.X), new Vector2(966, 110 + this.player.Entities[playerIndex].Image.Height), Color.Black);
            spriteBatch.DrawString(font, "Y : " + Math.Floor(this.player.Entities[playerIndex].Position.Y) + "px", new Vector2(1052, 130 + this.player.Entities[playerIndex].Image.Height), Color.Black);
            spriteBatch.DrawString(font, "Frames :       Amount : " + this.player.Entities[playerIndex].Animation.Frames.X + " by " + this.player.Entities[playerIndex].Animation.Frames.Y, new Vector2(966, 160 + this.player.Entities[playerIndex].Image.Height), Color.Black);
            spriteBatch.DrawString(font, "Size :  " + this.player.Entities[playerIndex].Animation.FrameWidth + "px by " + this.player.Entities[playerIndex].Animation.FrameHeight + "px", new Vector2(1052, 180 + this.player.Entities[playerIndex].Image.Height), Color.Black);
            spriteBatch.DrawString(font, "MoveSpeed :  " + this.player.Entities[playerIndex].MoveSpeed, new Vector2(1052, 200 + this.player.Entities[playerIndex].Image.Height), Color.Black);

            int frameX = (int)this.player.Entities[playerIndex].Animation.CurrentFrame.X * this.player.Entities[playerIndex].Animation.FrameWidth;
            int frameY = (int)this.player.Entities[playerIndex].Animation.CurrentFrame.Y * this.player.Entities[playerIndex].Animation.FrameHeight;
            spriteBatch.Draw(highlight, new Rectangle(frameX + 1052, frameY + 80, this.player.Entities[playerIndex].Animation.FrameWidth, this.player.Entities[playerIndex].Animation.FrameHeight), Color.White);
            spriteBatch.End();
        }
    }
}

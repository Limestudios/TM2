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
    public class Layer
    {
        List<Tile> tiles;
        List<string> motion, solid, platform;
        FileManager fileManager;
        ContentManager content;
        Texture2D tileSheet;
        string[] getMotion;
        Vector2 tileDimensions, mapDimensions;
        private static Layer instance;
        SpriteFont font;
        string nullTile;

        public int minX, maxX, minY, maxY;

        public static Layer Instance
        {
            get
            {
                if (instance == null)
                    instance = new Layer();
                return instance;
            }
        }

        /// <summary>
        /// Dimensions for a single tile in the map.
        /// </summary>
        public Vector2 TileDimensions
        {
            get { return new Vector2(64, 64); }
            set { tileDimensions = value; }
        }

        public Vector2 MapDimensions
        {
            get { return new Vector2(36, 20); }
            set { mapDimensions = value; }
        }

        public void LoadContent(Map map, string layerID)
        {
            tiles = new List<Tile>();
            motion = new List<string>();
            solid = new List<string>();
            platform = new List<string>();
            fileManager = new FileManager();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            font = content.Load<SpriteFont>("Coolvetica Rg");

            fileManager.LoadContent("Load/Maps/" + map.ID + ".txt", layerID);

            int indexY = 0;

            for (int i = 0; i < fileManager.Attributes.Count; i++)
            {
                for (int j = 0; j < fileManager.Attributes[i].Count; j++)
                {
                    switch (fileManager.Attributes[i][j])
                    {
                        case "TileSet" :
                            tileSheet = content.Load<Texture2D>("TileSets/" + fileManager.Contents[i][j]);
                            break;
                        case "TileDimensions":
                            string[] split = fileManager.Contents[i][j].Split(',');
                            this.tileDimensions = new Vector2(int.Parse(split[0]), int.Parse(split[1]));
                            break;
                        case "MapDimensions":
                            string[] split2 = fileManager.Contents[i][j].Split(',');
                            this.mapDimensions = new Vector2(int.Parse(split2[0]), int.Parse(split2[1]));
                            break;
                        case "Solid":
                            solid.Add(fileManager.Contents[i][j]);
                            break;
                        case "Platform":
                            platform.Add(fileManager.Contents[i][j]);
                            break;
                        case "NullTile":
                            nullTile = fileManager.Contents[i][j];
                            break;
                        case "Motion":
                            motion.Add(fileManager.Contents[i][j]);
                            break;
                        case "StartLayer":
                            Tile.Motion tempMotion = Tile.Motion.Static;
                            Tile.State tempState;

                            for (int k = 0; k < fileManager.Contents[i].Count; k++)
                            {
                                if (fileManager.Contents[i][k] != nullTile)
                                {
                                    split = fileManager.Contents[i][k].Split(',');
                                    tiles.Add(new Tile());

                                    if (solid.Contains(fileManager.Contents[i][k]))
                                        tempState = Tile.State.Solid;
                                    else if (platform.Contains(fileManager.Contents[i][k]))
                                        tempState = Tile.State.Platform;
                                    else
                                        tempState = Tile.State.Passive;

                                    foreach (string m in motion)
                                    {
                                        getMotion = m.Split(':');
                                        if (getMotion[0] == fileManager.Contents[i][k])
                                        {
                                            tempMotion = (Tile.Motion)Enum.Parse(typeof(Tile.Motion), getMotion[1]);
                                            break;
                                        }
                                    }

                                    tiles[tiles.Count - 1].SetTile(tempState, tempMotion, new Vector2(k * (int)TileDimensions.X, indexY * (int)TileDimensions.Y), tileSheet,
                                        new Rectangle(int.Parse(split[0]) * (int)TileDimensions.X, int.Parse(split[1]) * (int)TileDimensions.Y, (int)TileDimensions.X, (int)TileDimensions.Y));
                                }
                            }
                            indexY++;
                            break;
                    }
                }
            }
        }

        public void Update(GameTime gameTime, Camera camera, Map map)
        {
            //minX = (int)(MathHelper.Clamp((camera.CurrentPosision.X - camera.HalfViewportWidth) / map.layer.TileDimensions.X - 2, 0, this.MapDimensions.X));
            //maxX = (int)(MathHelper.Clamp((camera.CurrentPosision.X + camera.HalfViewportWidth) / map.layer.TileDimensions.X + 2, 0, this.MapDimensions.X));

            //minY = (int)(MathHelper.Clamp((camera.CurrentPosision.Y - camera.HalfViewportHeight) / map.layer.TileDimensions.Y - 2, 0, this.MapDimensions.Y));
            //maxY = (int)(MathHelper.Clamp((camera.CurrentPosision.Y + camera.HalfViewportHeight) / map.layer.TileDimensions.Y + 2, 0, this.MapDimensions.Y));

            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Update(gameTime);
            }
        }

        public void UpdateCollision(ref Entity entity, InputManager inputManager)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].UpdateCollision(ref entity, inputManager);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Draw(spriteBatch);
            }
        }
    }
}

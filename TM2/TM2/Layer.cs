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
        List<List<Tile>> tiles;
        List<List<string>> attributes, contents;
        List<string> motion, solid;
        FileManager fileManager;
        ContentManager content;
        Texture2D tileSheet;
        string[] getMotion;
        Vector2 tileDimensions, mapDimensions;
        private static Layer instance;

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
            get { return new Vector2(27, 13); }
            set { mapDimensions = value; }
        }

        public void LoadContent(Map map, string layerID)
        {
            tiles = new List<List<Tile>>();
            attributes = new List<List<string>>();
            contents = new List<List<string>>();
            motion = new List<string>();
            solid = new List<string>();
            fileManager = new FileManager();
            content = new ContentManager(ScreenManager.Instance.Content.ServiceProvider, "Content");

            fileManager.LoadContent("Load/Maps/" + map.ID + ".txt", attributes, contents, layerID);

            int indexY = 0;

            for (int i = 0; i < attributes.Count; i++)
            {
                for (int j = 0; j < attributes[i].Count; j++)
                {
                    switch (attributes[i][j])
                    {
                        case "TileSet" :
                            tileSheet = content.Load<Texture2D>("TileSets/" + contents[i][j]);
                            break;
                        case "TileDimensions":
                            string[] split = contents[i][j].Split(',');
                            this.tileDimensions = new Vector2(int.Parse(split[0]), int.Parse(split[1]));
                            break;
                        case "MapDimensions":
                            string[] split2 = contents[i][j].Split(',');
                            this.mapDimensions = new Vector2(int.Parse(split2[0]), int.Parse(split2[1]));
                            break;
                        case "Solid":
                            solid.Add(contents[i][j]);
                            break;
                        case "Motion":
                            motion.Add(contents[i][j]);
                            break;
                        case "StartLayer":
                            List<Tile> tempTiles = new List<Tile>();
                            Tile.Motion tempMotion = Tile.Motion.Static;
                            Tile.State tempState;

                            for (int k = 0; k < contents[i].Count; k++)
                            {
                                split = contents[i][k].Split(',');
                                tempTiles.Add(new Tile());

                                if (solid.Contains(contents[i][k]))
                                    tempState = Tile.State.Solid;
                                else
                                    tempState = Tile.State.Passive;

                                foreach (string m in motion)
                                {
                                    getMotion = m.Split(':');
                                    if (getMotion[0] == contents[i][k])
                                    {
                                        tempMotion = (Tile.Motion)Enum.Parse(typeof(Tile.Motion), getMotion[1]);
                                        break;
                                    }
                                }

                                tempTiles[k].SetTile(tempState, tempMotion, new Vector2(k * (int)TileDimensions.X, indexY * (int)TileDimensions.Y), tileSheet,
                                    new Rectangle(int.Parse(split[0]) * (int)TileDimensions.X, int.Parse(split[1]) * (int)TileDimensions.Y, (int)TileDimensions.X, (int)TileDimensions.Y));
                            }

                            tiles.Add(tempTiles);
                            indexY++;
                            break;
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    tiles[i][j].Update(gameTime);
                }
            }
        }

        public void UpdateCollision(ref Entity entity, InputManager inputManager)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    tiles[i][j].UpdateCollision(ref entity);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                for (int j = 0; j < tiles[i].Count; j++)
                {
                    tiles[i][j].Draw(spriteBatch);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using System.IO;
using ScreenSystemLibrary;
using RunningfromCertainDeath.GameObjects;

namespace RunningfromCertainDeath.RCD_TileEngine
{
    class Level
    {
        // Map creates a list of our tilesprites
        public List <TileSprite> Map { get; set; }

        public GraphicsDeviceManager Graphics { get; set; }

        //Get Set stage  
        public int Stage { get; set; }

        public Vector2 StartingPosition { get; set; }

        // Constructor
        public Level(GraphicsDeviceManager graphics, Vector2 startingPosition)
        {
            Map = new List<TileSprite>();
            this.Graphics = graphics;
            this.StartingPosition = startingPosition;
        }

        public void LoadLevelText(int stageToLoad, ContentManager cm)
        {
            //pull the stage to load from our properties
            this.Stage = stageToLoad;

            //build the name of the xml file that holds our stage. In this case the file is stored in the Levels folder of our project
            //and is named Level and then the number of the level
            string fileName = @"RCD_TileEngine\Levels\Level" + stageToLoad.ToString() + ".txt";

            // Load the level and ensure all of the lines are the same length.
            int width;
            List<string> lines = new List<string>();

            //create a new filestream to read in our file
            using (Stream fStream = TitleContainer.OpenStream(fileName))
            {
                using (StreamReader reader = new StreamReader(fStream))
                {
                    //pull out the first line and see how many characters are in the line
                    string line = reader.ReadLine();
                    width = line.Length;

                    //
                    while (line != null)
                    {
                        lines.Add(line);
                        //if one line is a different size than the others, we throw an error
                        if (line.Length != width)
                            throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                        line = reader.ReadLine();
                    }
                }
            }

            //now we basically loop through our string array and pull out each numerical value
            int yPos = 0;
            foreach (string s in lines)
            {
                int xPos = 0;

                foreach (char c in s)
                {
                    TileSprite ts;

                    switch (c)
                    {
                        //depending on the char, we will create a different tilesprite
                        case '.':
                            //empty tile, do nothing
                            break;
                      
                        case '#': // Platform
                            ts = new TileSprite();
                            ts.TileTexture = cm.Load<Texture2D>(@"Textures/Tiles/platform");
                            ts.Landable = true;
                            ts.Position = new Vector2(xPos * ts.TileWidth, yPos * ts.TileHeight);
                            this.addNewSprite(ts);
                            break;
                        case 'I':   // Item (random)
                            ts = new TileSprite();
                            ts.TileTexture = cm.Load<Texture2D>(@"Textures/Tiles/Items/item_1"); //Item_{0}"); //Random rng = new Random(1-numberOfItems);
                            ts.Landable = false;
                            ts.Position = new Vector2(xPos * ts.TileWidth, yPos * ts.TileHeight);
                            this.addNewSprite(ts);
                            break;
                        case '*':   // Horny Fungi Guy
                            ts = new TileSprite();
                            ts.TileTexture = cm.Load<Texture2D>(@"Textures/Tiles/hornyFungi"); 
                            ts.Landable = false;
                            ts.Position = new Vector2(xPos * ts.TileWidth, yPos * ts.TileHeight);
                            this.addNewSprite(ts);
                            break;
                        case 'E':   // Enemy  //Enemy_{0}"); //Random rng = new Random(1-numberOfEnemy);
                            Texture2D TileTexture = cm.Load<Texture2D>(@"Textures/Tiles/Enemy/enemy_2");   
                            bool Landable = false;
                            Vector2 Position = new Vector2(xPos * 32, yPos * 32);
                            Vector2 frames = new Vector2(3, 4);
                            ts = new TileSprite(false, true, TileTexture, 32, 32, Position, Landable, frames, true, (32*3));
                            ts.CurrentFrame = new Vector2(ts.CurrentFrame.X, 1); // make 1 -> 2 animate to the right
                            this.addNewSprite(ts);
                            break;
                        case 'P':   // Plaque
                            ts = new TileSprite();
                            ts.TileTexture = cm.Load<Texture2D>(@"Textures/Tiles/Plaques/plaque"); //PlaqueIcon_{0} Random (1-12)?
                            ts.Landable = false;
                            ts.Position = new Vector2(xPos * ts.TileWidth, yPos * ts.TileHeight);
                            this.addNewSprite(ts);
                            break;
                        case 'X':   // Door marked Exit...
                            ts = new TileSprite();
                            ts.TileTexture = cm.Load<Texture2D>(@"Textures/Tiles/door"); 
                            ts.Landable = false;
                            ts.Position = new Vector2(xPos * ts.TileWidth, yPos * ts.TileHeight);
                            this.addNewSprite(ts);
                            break;
                        case 'B': //set Boaz starting position
                            //this.StartingPosition = new Vector2(xPos * 32, yPos * 32);
                            //break;
                            Texture2D TileTexture2 = cm.Load<Texture2D>(@"Images\running3");   
                            bool Landable2 = false;
                            Vector2 Position2 = new Vector2(xPos * 32, yPos * 32);
                            Vector2 frames2 = new Vector2(9, 2);
                            ts = new TileSprite(true, true, TileTexture2, 30, 34, Position2, Landable2, frames2, false, (0));
                            ts.CurrentFrame = new Vector2(ts.CurrentFrame.X, 1); // make 1 -> 2 animate to the right
                            this.addNewSprite(ts);
                            break;
                        default:
                            throw new Exception(String.Format("Invalid Tile Loaded"));
                    }
                    xPos++;
                }
                yPos++;
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (TileSprite ts in this.Map)
            {
                ts.Update(gameTime);
            }
        }

        // Draw our tiles
        public void Draw(SpriteBatch spriteBatch)
        {
            //draw tiles
            foreach (TileSprite ts in this.Map)
            {
               // if((ts.Position.X + ts.TileWidth > 0 && ts.Position.X <= graphics.PreferedBackBufferedWidth) &&
                    //(ts.Position.Y + ts.TileHeight > 0 && ts.Position.Y <= graphics.PreferedBackBufferedHeight))
                //{
                    ts.Draw(spriteBatch);
                //}

                /*----------------------------------------------------------------------------
                 * We are performing a check to make sure each tile is in the viewable window 
                 * so we do not draw anything outside of our screen, thus wasting resources.
                 *///-------------------------------------------------------------------------
            }
        }

        

        //  Takes the tile that is passed and adds it to map list.
        private void addNewSprite(TileSprite t)
        {
            this.Map.Add(t);
        }
    }
}

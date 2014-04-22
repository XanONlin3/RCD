using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Xml;
using System.IO;
using RunningfromCertainDeath.GameObjects;
using RunningfromCertainDeath.Animation;
using RunningfromCertainDeath.Input;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace RunningfromCertainDeath.RCD_TileEngine
{
    class TileSprite
    {
        public static int defaultTileSize = 32;

        //texture of the tile
        public Texture2D TileTexture { get; set; }

        //can land on the tile?
        public bool Landable { get; set; }
       
        //width of the tile     
        public int TileWidth { get; set; }
        //this.TileWidth = defaultTileSize;
 
        //height of the tile 
        public int TileHeight { get; set; }
        //this.TileHeight = defaultTileSize;

        //x,y coords of the tile
        public Vector2 position;
        public Vector2 Position
        { 
            set { position = value; }
            get { return position; }
        }

        //for user jumping
        bool jump = false;
        int jumpCtr = 0;
        int gravCtr = 0;
        KeyboardState mPreviousKeyboardState;

        //isAnimated
 //       public bool Animated { get; set; } 

        //------To handel animated tiles--------------------------------------------------
        int frameCounter;
        int switchFrame;

        protected Vector2 frames; // number of frames LEFT/RIGHT number of frames UP/DN
        protected Vector2 currentFrame; //current frame

        public Rectangle sourceRect;
        public Vector2 origin;

        float rotation = 0.0f;
        float scale = 1.0f;

        public Vector2 Frames
        {
            set { frames = value; }
        }
        public Vector2 CurrentFrame
        {
            set { currentFrame = value; }
            get { return currentFrame; }
        }

        public int FrameWidth
        {
            get { return TileTexture.Width / (int)frames.X; }
        }

        public int FrameHeight
        {
            get { return TileTexture.Height / (int)frames.Y; }
        }

        // The state of the Animation
        public bool Active { set; get; }
        public bool User { set; get; }

        // This is for moving tiles (i.e. patroling enemies, moving platform
        public bool Patroling { set; get; }
        public Vector2 velocity;
        public Vector2 Velocity 
        {
            set { velocity = value; }
            get { return velocity; }
        }
        public Vector2 userVelocity;
        public Vector2 UserVelocity
        {
            set { userVelocity = value; }
            get { return userVelocity; }
        }
        public int PatrolDistance { set; get; }
        public int oldDistance { set; get; }
        public bool right;

        //---------------------------------------------------------------------

        //bounding box is a box drawn around the tile for collision detection.
        public Rectangle BoundingBox
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, TileTexture.Width, TileTexture.Height); }
        }

        // Constructor
        public TileSprite()
        {
            Active = false;
            Patroling = false;
            User = false; 
            PatrolDistance = 0;
            velocity = new Vector2(0,0);
            this.TileWidth = defaultTileSize; 
            this.TileHeight = defaultTileSize;
            this.frameCounter = 0;
            this.switchFrame = 100;
            this.frames = new Vector2(1, 1); //default
            this.currentFrame = new Vector2(0, 0); //default
            //if (TileTexture != null)
                sourceRect = new Rectangle(0, 0, defaultTileSize, defaultTileSize);
            
        }

        public TileSprite(bool user, bool active, Texture2D texture, int spriteWidth, int spriteHeight, Vector2 startingPosition, bool isLandable, Vector2 frames, bool isPatrolling, int patrolDistance)
        {
            this.User = user; 
            this.Active = active;
            this.TileTexture = texture;
            this.TileWidth = spriteWidth;
            this.TileHeight = spriteHeight;
            this.position = startingPosition;
            this.Landable = isLandable;
            this.frameCounter = 0;
            this.switchFrame = 100;
            this.frames = frames;
            this.currentFrame = new Vector2(0, 0); //default
            if (texture != null)
                sourceRect = new Rectangle(0, 0, spriteWidth, spriteHeight);

            //this.velocity.X = patrolSpeed;
            this.Patroling = isPatrolling;
            this.PatrolDistance = patrolDistance;

            this.oldDistance = this.PatrolDistance; // fixed = numb of pixels the tile should patrol
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            //UpdateMovement(aCurrentKeyboardState);
            

            if (Active)
            {
                frameCounter += (int)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (frameCounter >= switchFrame)
                {
                    frameCounter = 0;
                    currentFrame.X++;

                    if (currentFrame.X * FrameWidth >= TileTexture.Width)
                        currentFrame.X = 0;
                }
               
            }
            else
            {
                frameCounter = 0;
            }
            sourceRect = new Rectangle((int)currentFrame.X * FrameWidth, (int)currentFrame.Y * FrameHeight,
                        FrameWidth, FrameHeight);
            if (Patroling)
            {
                position += velocity;
                origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2); // center!
                
                if(PatrolDistance <= 0){
                    right = true;
                    velocity.X = 1f;
                }
                else if(PatrolDistance >= oldDistance)
                {
                    right = false;
                    velocity.X = -1f;
                }
                if (right) PatrolDistance += 1; else PatrolDistance -= 1;
            }

            if (User)
            {
                Debug.WriteLine("In User");
               // position += userVelocity;s
                Active = false;
                if (userVelocity.X == 0)
                    currentFrame =new Vector2(0,0);
                if (aCurrentKeyboardState.IsKeyDown(Keys.A) == true)
                {
                    velocity.X = -2f;
                    position.X += velocity.X;
                    Active = true;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.D) == true)
                {
                    velocity.X = 2f;
                    position.X += velocity.X;
                    Active = true;
                }

             /*  if (input.IsKeyDown(Keys.W) && prevKeyState.IsKeyUp(Keys.W) && (jump == false)) //Up
                {
                    if (jumpCtr == 0)
                    {
                        jumpCtr = 10;
                        jump = true;
                    }
                } */

                mPreviousKeyboardState = aCurrentKeyboardState;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(this.TileTexture, this.Position, Color.White);
            if (TileTexture != null)
            {
                if (velocity.X > 0 || userVelocity.X > 0)
                {
                    origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                    spriteBatch.Draw(TileTexture, Position + origin, sourceRect,
                        Color.White, rotation, origin, scale,
                        SpriteEffects.FlipHorizontally, 0.0f);
                }
                else
                {
                    origin = new Vector2(sourceRect.Width / 2, sourceRect.Height / 2);
                    spriteBatch.Draw(TileTexture, Position + origin, sourceRect,
                        Color.White, rotation, origin, scale,
                        SpriteEffects.None, 0.0f);
                }

            }
        }
    }
}

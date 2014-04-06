using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpriteEngine
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D t2dNinja;
        MobileSprite myNinja; 

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            t2dNinja = Content.Load<Texture2D>(@"running3");
            myNinja = new MobileSprite(t2dNinja);
            myNinja.Sprite.AddAnimation("leftstop", 0, 0, 30, 34, 1, 0.1f);
            myNinja.Sprite.AddAnimation("left", 30, 0, 30, 34, 8, 0.1f);
            myNinja.Sprite.AddAnimation("rightstop", 240, 34, 30, 34, 1, 0.1f);
            myNinja.Sprite.AddAnimation("right", 0, 34, 30, 34, 8, 0.1f);
            myNinja.Sprite.CurrentAnimation = "rightstop";
            myNinja.Position = new Vector2(100, 300);
            myNinja.Sprite.AutoRotate = false;
            myNinja.IsPathing = false;
            myNinja.IsMoving = false;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            KeyboardState ks = Keyboard.GetState();
            bool leftkey = ks.IsKeyDown(Keys.A);
            bool rightkey = ks.IsKeyDown(Keys.D);

            if (leftkey)
            {
                if (myNinja.Sprite.CurrentAnimation != "left")
                {
                    myNinja.Sprite.CurrentAnimation = "left";
                }
                myNinja.Sprite.MoveBy(-2, 0);
            }

            if (rightkey)
            {
                if (myNinja.Sprite.CurrentAnimation != "right")
                {
                    myNinja.Sprite.CurrentAnimation = "right";
                }
                myNinja.Sprite.MoveBy(2, 0);
            }

            if (!leftkey && !rightkey)
            {
                if (myNinja.Sprite.CurrentAnimation == "left")
                {
                    myNinja.Sprite.CurrentAnimation = "leftstop";
                }
                if (myNinja.Sprite.CurrentAnimation == "right")
                {
                    myNinja.Sprite.CurrentAnimation = "rightstop";
                }
            }

            myNinja.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            myNinja.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

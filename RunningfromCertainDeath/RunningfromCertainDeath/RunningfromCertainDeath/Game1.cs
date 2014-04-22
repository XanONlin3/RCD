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
using ScreenSystemLibrary;

namespace RunningfromCertainDeath
{
    // MAIN
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ScreenSystem screenSystem;

        Camera camera;

        public Vector2 characterPosition;
        public Rectangle characterRect;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720; //800 makes 32X32 tiles fit perfectly
            Content.RootDirectory = "Content";

            screenSystem = new ScreenSystem(this);
            Components.Add(screenSystem);
        }

        // Initialize
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            screenSystem.AddScreen(new Screens.JKTaLogoScreen(Color.Black, 0.5f));
            Settings.MusicVolume = 1.0f;
            Settings.SoundVolume = 1.0f;

            camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }

        // Load Content
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        // Unload Content
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        // Update
        protected override void Update(GameTime gameTime)
        {

            // TODO: Add your update logic here
            base.Update(gameTime);
        }

        // Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black); //clearcolor

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, 
                                null, null, null, null, 
                                camera.transform);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
